using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
	[SerializeField]
	private LevelGeneratorSettings _settings;
	public LevelGeneratorSettings Settings {
		get => _settings ?? ( _settings = ScriptableObject.CreateInstance <LevelGeneratorSettings> () );
		set => _settings = value;
	}
	[SerializeField]
	private PrefabDatabase _prefabDatabase;
	public PrefabDatabase PrefabDatabase {
		get => _prefabDatabase ?? ( _prefabDatabase = ScriptableObject.CreateInstance <PrefabDatabase> () );
		set => _prefabDatabase = value;
	}
	private FloorInfo prevFloorInfo;
	private PlatformCircle platformCircle, obstacleCircle;
	private float floorHeight, floorY;
	private Transform floorTf, platformContainerTf;

	public IEnumerable <FloorInfo> Generate ( FloorInfo prevFloorInfo, int nextFloorIndex = 0 ) {
		this.prevFloorInfo = prevFloorInfo;
		floorHeight = UnityEngine.Random.Range ( Settings.FloorHeightMin, Settings.FloorHeightMax );
		var prevFloorTf = prevFloorInfo.FloorRoot.transform;
		floorY = prevFloorTf.position.y - floorHeight;
		var floorContainerTf = prevFloorTf.parent;
		var baseAngle = 0f;
		while ( true ) {
			var floorRoot = FloorRoot.Create ( floorContainerTf, nextFloorIndex, floorY );
			floorTf = floorRoot.transform;
			var platformsContainer = PlatformContainer.Create ( floorTf, baseAngle );
			platformContainerTf = platformsContainer.transform;
			ProcessPrevFloorInfo ( baseAngle );
			GenerateFloor ();
			var floorCompleteTriggerGo = Instantiate ( PrefabDatabase.FloorCompleteTrigger, floorTf );
			floorCompleteTriggerGo.transform.localPosition = Vector3.zero;
			this.prevFloorInfo = new FloorInfo ( floorRoot, baseAngle, platformCircle, obstacleCircle );
			floorY -= floorHeight;
			baseAngle += RandomHelper.Range ( Settings.BaseAngleOffsetMin, Settings.BaseAngleOffsetMax, Settings.BaseAngleOffsetStep );
			nextFloorIndex++;
			yield return	this.prevFloorInfo;
		}
	}

	private void ProcessPrevFloorInfo ( float baseAngle ) {
		// Transform prev floor coordinates to match with current floor.
		var invOffset = prevFloorInfo.BaseAngle - baseAngle;
		prevFloorInfo.PlatformCircle.Shift ( invOffset );
		prevFloorInfo.ObstacleCircle.Shift ( invOffset );
	}

	private void GenerateFloor () {
		platformCircle = new PlatformCircle ();
		GenerateHoles ();
		var holeInversionRanges = platformCircle.GetAllEmptyRanges ();
		GeneratePlatforms ();
		obstacleCircle = new PlatformCircle ();
		GenerateHorzObstacles ( holeInversionRanges );
		GenerateColumn ();
	}

	private void GenerateHoles () {
		var holeCount = UnityEngine.Random.Range ( Settings.HoleCountMin, Settings.HoleCountMax + 1 );
		if ( holeCount == 0 )
			return;

		AddHoles ( holeCount );
		SeparateHoles ();
		ShakeHoles ();
		SyncHoleAngles ();
	}

	private int AddHoles ( int holeCount ) {
		var totalWidth = Settings.TotalHoleWidthMax;
		// Add main hole.
		var holeBaseAngle = 0f;
		AddHole ( ref holeBaseAngle, ref totalWidth, Settings.MainHoleWidthMin, Settings.MainHoleWidthMax, isMain : true );
		// Add secondary holes.
		int actualCount = 1;
		var holesLeft = holeCount;
		while ( --holesLeft > 0 ) {
			// Reserve some width for the rest of the holes.
			var minTotalWidthForOtherHoles = ( holesLeft - 1 ) * Settings.SecondaryHoleWidthMin;
			var maxWidth = totalWidth - minTotalWidthForOtherHoles;
			if ( maxWidth < Settings.SecondaryHoleWidthMin )
				continue;	// Too many holes, it's not possible to fit them all.

			AddHole ( ref holeBaseAngle, ref totalWidth, Settings.SecondaryHoleWidthMin, maxWidth, isMain : false );
			actualCount++;
		}

		return	actualCount;
	}

	private bool AddHole (
		ref float baseAngle, ref float totalWidth,
		float minWidth, float maxWidth,
		bool isMain
	) {
		var desiredWidth = RandomHelper.Range ( minWidth, maxWidth, Settings.HoleWidthStep );
		if ( desiredWidth > totalWidth )
			desiredWidth = totalWidth;

		var flags = PlatformKindFlags.Hole;
		if ( isMain )
			flags |= PlatformKindFlags.Main;

		var holePrefab = PrefabDatabase.Platforms
			.MatchFlags ( flags )
			.WidthBetween ( desiredWidth, desiredWidth )
			.FirstOrDefault ();
		if ( holePrefab == null )
			return	false;

		var hole = InstantiatePlatform ( holePrefab, baseAngle, platformContainerTf );
		var actualWidth = hole.AngleWidth;
		platformCircle.Add ( hole, baseAngle, baseAngle + actualWidth );
		baseAngle += actualWidth;
		totalWidth -= actualWidth;
		return	true;
	}

	private void SeparateHoles () {
		if ( platformCircle.Count < 2 )
			return;

		var holeFragments = platformCircle.ToArray ();
		platformCircle.Clear ();
		var lastFragment = holeFragments [holeFragments.Length - 1];
		var spaceLeft = 360 - lastFragment.Range.End;
		var offset = 0f;
		for ( int i = 1 ; i < holeFragments.Length ; i++ ) {
			var nextOffset = offset + Settings.SpaceBetweenHolesMin;
			if ( nextOffset <= spaceLeft )
				offset = nextOffset;

			var fragment = holeFragments [i];
			var range = fragment.Range;
			range = range.Shift ( offset );
			holeFragments [i] = new LineFragment <Platform, float> ( fragment.Element, range );
		}

		platformCircle.AddRange ( holeFragments );
	}

	private void ShakeHoles () {
		// Randomly "shake" hole positions.
		var nextStart = 360f;
		var holeFragments = platformCircle.ToArray ();
		for ( int i = holeFragments.Length - 1 ; i >= 1 ; i-- ) {
			nextStart -= Settings.SpaceBetweenHolesMin;
			var fragment = holeFragments [i];
			var range = fragment.Range;
			var start = range.Start;
			var maxStart = Mathf.FloorToInt ( ( nextStart - range.Width () ) / Settings.SecondaryHoleWidthMin ) * Settings.SecondaryHoleWidthMin;
			if ( maxStart < start ) {
				nextStart = range.Start;
				continue;
			}

			var newStart = RandomHelper.Range ( start, maxStart, Settings.SecondaryHoleWidthMin );
			if ( newStart != start ) {
				platformCircle.Remove ( start );
				range = range.Shift ( newStart - start );
				platformCircle.Add ( fragment.Element, range );
			}

			nextStart = range.Start;
		}
	}

	private void SyncHoleAngles () {
		foreach ( var fragment in platformCircle ) {
			var hole = fragment.Element;
			hole.StartAngleLocal = fragment.Range.Start;
		}
	}

	private void GeneratePlatforms () {
		while ( platformCircle.TryFindEmptyRange ( out var emptyRange ) ) {
			var start = emptyRange.Start;
			var platformPrefab = PrefabDatabase
				.Platforms
				.MatchFlags ( PlatformKindFlags.Platform )
				.WidthBetween ( Settings.PlatformWidthMin, emptyRange.Width () )
				.OrderByDescending ( p => p.AngleWidth )
				.FirstOrDefault ();
			if ( platformPrefab == null ) {
				Debug.LogWarning ( $"No suitable platform was found for the range {emptyRange} at {platformContainerTf.name}." );
				// Fill whole range to not revisit it in the next iteration.
				platformCircle.Add ( null, emptyRange );
				continue;
			}

			var platform = InstantiatePlatform ( platformPrefab, start, platformContainerTf );
			platformCircle.Add ( platform, start, start + platform.AngleWidth );
		}
	}

	private static Platform InstantiatePlatform ( Platform prefab, float startAngle, Transform parent ) {
		var platform = Instantiate ( prefab, parent );
		platform.transform.localPosition = Vector3.zero;
		platform.StartAngleLocal = startAngle;
		return	platform;
	}

	private void GenerateHorzObstacles ( List <Range <float>> platformRanges ) {
		var obstacleCount = UnityEngine.Random.Range ( Settings.HorzObstacleCountMin, Settings.HorzObstacleCountMax + 1 );
		if ( obstacleCount == 0 )
			return;

		if ( !Settings.AllowObstaclesUnderHoles ) {
			// We don't want player to get sick of falling onto obstacles while following the right path.
			CutRangesUnderPreviousFloorHoles ( platformRanges );
		}
		// TODO: generate obstacles over holes.
		var widthLeft = Settings.TotalHorzObstacleWidthMax;
		GenerateHorzObstaclesOverPlatforms ( platformRanges, ref obstacleCount, ref widthLeft );
		MakeObstaclesOverPlatformMoving ();
	}

	private void CutRangesUnderPreviousFloorHoles ( List <Range <float>> platformRanges ) {
		var prevHoleRanges = prevFloorInfo.PlatformCircle
			.Where ( f => ( f.Element.Kind & PlatformKindFlags.Hole ) != PlatformKindFlags.None )
			.Select ( f => f.Range );
		foreach ( var holeRange in prevHoleRanges ) {
			for ( int i = 0 ; i < platformRanges.Count ; ) {
				Range.SubtractOrdered ( platformRanges [i], holeRange, out var r1, out var r2 );
				if ( r1.HasValue ) {
					int step = 1;
					platformRanges.RemoveAt ( i );
					if ( r2.HasValue ) {
						platformRanges.Insert ( i, r2.Value );
						step = 2;
					}

					platformRanges.Insert ( i, r1.Value );
					i += step;
				} else
					platformRanges.RemoveAt ( i );
			}
		}
	}

	private void GenerateHorzObstaclesOverPlatforms (
		List <Range <float>> allowedRanges,
		ref int obstacleCount, ref float widthLeft
	) {
		while ( obstacleCount-- > 0 && allowedRanges.Count > 0 && widthLeft > 0 ) {
			int index = UnityEngine.Random.Range ( 0, allowedRanges.Count );
			var range = allowedRanges [index];
			bool rangeIsValid = true;
			if ( range.Width () < Settings.HorzObstacleWidthMin ) {
				// Obstacle doesn't fit, this range is useless for us.
				rangeIsValid = false;
			}

			if ( !RandomlyInsertHorzObstacle (
				range,
				ref widthLeft, out var occupiedRange
			) ) {
				/* By some reason we wasn't able to instantiate obstacle at the given range.
				 * Remove it to avoid infinite loop. */
				rangeIsValid = false;
			}

			allowedRanges.RemoveAt ( index );
			if ( !rangeIsValid )
				continue;

			occupiedRange = occupiedRange.Grow ( Settings.MinSpaceBetweenHorzObstacles );
			Range.SubtractOrdered ( range, occupiedRange, out var r1, out var r2 );
			if ( r2.HasValue ) allowedRanges.Insert ( index, r2.Value );
			if ( r1.HasValue ) allowedRanges.Insert ( index, r1.Value );
		}
	}

	private bool RandomlyInsertHorzObstacle (
		Range <float> targetRange,
		ref float widthLeft,
		out Range <float> occupiedRange
	) {
		var maxWidth = Mathf.Min ( Settings.HorzObstacleWidthMax, widthLeft, targetRange.Width () );
		var desiredWidth = RandomHelper.Range ( Settings.HorzObstacleWidthMin, maxWidth, Settings.HorzObstacleWidthStep );
		var prefab = PrefabDatabase
			.Platforms
			.MatchFlags ( PlatformKindFlags.KillerObstacle | PlatformKindFlags.Platform )
			.WidthBetween ( desiredWidth, desiredWidth )
			.FirstOrDefault ();
		if ( prefab == null ) {
			occupiedRange = default;
			return	false;
		}

		var actualWidth = prefab.AngleWidth;
		var baseAngle = RandomHelper.Range ( targetRange.Start, targetRange.End - actualWidth, Settings.HorzObstacleWidthStep );
		var instance = InstantiatePlatform ( prefab, baseAngle, platformContainerTf );
		occupiedRange = Range.Create ( baseAngle, baseAngle + actualWidth );
		obstacleCircle.Add ( instance, occupiedRange );
		widthLeft -= actualWidth;
		return	true;
	}

	private void MakeObstaclesOverPlatformMoving () {
		if ( obstacleCircle.Count == 0 )
			return;

		var holeFrags = platformCircle.Where ( f => ( f.Element.Kind & PlatformKindFlags.Hole ) != PlatformKindFlags.None );
		var holeCircle = FragmentedCircle.CreateDegrees <Platform> ();
		holeCircle.AddRange ( holeFrags );
		FragmentedCircle <Platform> prevFloorHoleCircle = null;
		if ( !Settings.AllowObstaclesMoveUnderHoles ) {
			prevFloorHoleCircle = FragmentedCircle.CreateDegrees <Platform> ();
			holeFrags = prevFloorInfo.PlatformCircle
				.Where ( f => ( f.Element.Kind & PlatformKindFlags.Hole ) != PlatformKindFlags.None );
			prevFloorHoleCircle.AddRange ( holeFrags );
		}

		var movingFragments = new List <LineFragment <Platform, float>> ();
		for ( int i = 0 ; i < obstacleCircle.Count ; i++ ) {
			var fragment = obstacleCircle [i];
			var platform = fragment.Element;
			if ( null != platform.GetComponent <PlatformRotator> () ) {
				// Obstacle is already moving.
				continue;
			}

			if ( UnityEngine.Random.value > Settings.ObstacleOverPlatformMovingChance )
				continue;

			var range = fragment.Range;
			float minBound, maxBound;
			if ( obstacleCircle.Count <= 1 ) {
				/* Fragment boundary seeking algorithm produces inverted arc when there's only one obstacle.
				 * Min boundary settles at range.End and max boundary at range.Start, which results in hole at the place of original obstacle.
				 * With all of that being said, it's better to define boundaries as 360 degree arc. */
				minBound = range.Start;
				maxBound = minBound + 360;
			} else {
				obstacleCircle.SeekFragmentBoundary ( range.Start, -1, out minBound );
				obstacleCircle.SeekFragmentBoundary ( range.End  ,  1, out maxBound );
				if ( range.Start == minBound && range.End == maxBound ) {
					// There's no space for oscillation.
					continue;
				}
			}

			// We don't want obstacles to behave unpredictably: they must not trespass boundaries between platforms and holes.
			IntersectWithFreeSpaceArc ( holeCircle, range, ref minBound, ref maxBound, out var resultsInNoSpace );
			if ( resultsInNoSpace )
				continue;

			if ( prevFloorHoleCircle != null ) {
				// Moving obstacles should not enter space under the holes of previous floor.
				IntersectWithFreeSpaceArc ( prevFloorHoleCircle, range, ref minBound, ref maxBound, out resultsInNoSpace );
				if ( resultsInNoSpace )
					continue;
			}

			var arc = CircleMath.ArcEndsToArc ( Range.Create ( minBound, maxBound ), dir : 1, pi2 : 360 );
			minBound = arc.Start;
			maxBound = arc.End;
			var rotator = platform.gameObject.AddComponent <PlatformRotator> ();
			rotator.StartAngle = minBound;
			rotator.EndAngle = maxBound;
			rotator.AngularSpeed = UnityEngine.Random.Range ( Settings.MovingObstacleAngularSpeedMin, Settings.MovingObstacleAngularSpeedMax );
			rotator.MotionCurve = Settings.MovingObstacleMotionCurve;
			rotator.MinOscillationTime = Settings.MovingObstacleMinOscillationTime;
			movingFragments.Add ( fragment );
		}

		// Separate overlapping rotators.
		if ( movingFragments.Count >= 2 ) {
			var fragment = movingFragments [movingFragments.Count - 1];
			var rotator0 = fragment.Element.GetComponent <PlatformRotator> ();
			var range0 = fragment.Range;
			for ( int i = 0 ; i < movingFragments.Count ; i++ ) {
				fragment = movingFragments [i];
				var rotator1 = fragment.Element.GetComponent <PlatformRotator> ();
				var range1 = fragment.Range;
				if ( rotator0.EndAngle % 360 == range1.Start &&
					 rotator1.StartAngle % 360 == range0.End
				) {
					// Make rotating platforms meet at their bisector instead of overlapping.
					var arc = CircleMath.ArcEndsToArc ( Range.Create ( range0.End, range1.Start ), dir : 1, pi2 : 360 );
					var halfDistance = arc.End - arc.Middle ();
					rotator0.EndAngle -= halfDistance;
					rotator1.StartAngle += halfDistance;
				}

				rotator0 = rotator1;
				range0 = range1;
			}
		}
	}

	private static bool IntersectWithFreeSpaceArc (
		FragmentedCircle <Platform> occlusionCircle, Range <float> platformRange,
		ref float minBound, ref float maxBound,
		out bool resultsInNoSpace
	) {
		resultsInNoSpace = false;
		if ( occlusionCircle.Count != 0 ) {
			if ( occlusionCircle.Intersects ( platformRange ) ) {
				resultsInNoSpace = true;
				return	true;
			}

			occlusionCircle.SeekFragmentBoundary ( platformRange.Start, -1, out var minOccluderBound );
			occlusionCircle.SeekFragmentBoundary ( platformRange.End  ,  1, out var maxOccluderBound );
			CircleMath.IntersectArcs (
				360,
				Range.Create ( minBound, maxBound ), dir1 : 1,
				Range.Create ( minOccluderBound, maxOccluderBound ), dir2 : 1,
				out var intersectionArc1, out var intersectionArc2
			);
			// Both arcs grew up from the same point, therefore there is always at least one intersection.
			if ( intersectionArc2.HasValue ) {
				// There are two intersections. Only one of them contains initial range.
				if ( intersectionArc2.Value.Contains ( platformRange ) )
					intersectionArc1 = intersectionArc2;
			}

			minBound = intersectionArc1.Value.Start;
			maxBound = intersectionArc1.Value.End;
			if ( platformRange.Start == minBound && platformRange.End == maxBound ) {
				// There's no space for oscillation.
				resultsInNoSpace = true;
			}

			return	true;
		}

		return	false;
	}

	private Column GenerateColumn () {
		var columns = PrefabDatabase.PredefinedColumns;
		if ( columns.Count == 0 ) {
			Debug.LogWarning ( $"No suitable column was found at {floorTf.name}." );
			return	null;
		}

		var prefab = columns [UnityEngine.Random.Range ( 0, columns.Count )];
		var column = Instantiate ( prefab, floorTf );
		var columnTf = column.transform;
		columnTf.localPosition = Vector3.zero;
		var scale = columnTf.localScale;
		scale.y = floorHeight / column.InitialHeight;
		columnTf.localScale = scale;
		return	column;
	}
}
