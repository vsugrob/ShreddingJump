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

	public IEnumerable <FloorInfo> Generate ( FloorInfo prevFloorInfo, int nextFloorIndex = 0 ) {
		var floorHeight = UnityEngine.Random.Range ( Settings.FloorHeightMin, Settings.FloorHeightMax );
		var prevFloorTf = prevFloorInfo.FloorRoot.transform;
		var floorY = prevFloorTf.position.y - floorHeight;
		var floorContainer = prevFloorTf.parent;
		var baseAngle = 0f;
		var i = 0;
		while ( true ) {
			var floorRoot = FloorRoot.Create ( floorContainer, nextFloorIndex, floorY );
			var floorTf = floorRoot.transform;
			var platformsContainer = PlatformContainer.Create ( floorTf, baseAngle );
			GenerateFloor (
				floorTf, platformsContainer.transform, floorHeight,
				out var platformCircle, out var obstacleCircle
			);
			var floorCompleteTriggerGo = Instantiate ( PrefabDatabase.FloorCompleteTrigger, floorTf );
			floorCompleteTriggerGo.transform.localPosition = Vector3.zero;
			floorY -= floorHeight;
			baseAngle += RandomHelper.Range ( Settings.BaseAngleOffsetMin, Settings.BaseAngleOffsetMax, Settings.BaseAngleOffsetStep );
			i++;
			nextFloorIndex++;
			yield return new FloorInfo ( floorRoot, platformCircle, obstacleCircle );
		}
	}

	private void GenerateFloor (
		Transform floorTf, Transform platformContainerTf, float floorHeight,
		out PlatformCircle platformCircle, out PlatformCircle obstacleCircle
	) {
		platformCircle = new PlatformCircle ();
		GenerateHoles ( platformContainerTf, platformCircle );
		var holeInversionRanges = platformCircle.GetAllEmptyRanges ();
		GeneratePlatforms ( platformContainerTf, platformCircle );
		obstacleCircle = new PlatformCircle ();
		GenerateHorzObstacles ( platformContainerTf, platformCircle, obstacleCircle, holeInversionRanges );
		GenerateColumn ( floorTf, floorHeight );
	}

	private void GenerateHoles ( Transform containerTf, PlatformCircle platformCircle ) {
		var holeCount = UnityEngine.Random.Range ( Settings.HoleCountMin, Settings.HoleCountMax + 1 );
		if ( holeCount == 0 )
			return;

		AddHoles ( containerTf, platformCircle, holeCount );
		SeparateHoles ( platformCircle );
		ShakeHoles ( platformCircle );
		SyncHoleAngles ( platformCircle );
	}

	private int AddHoles ( Transform containerTf, PlatformCircle platformCircle, int holeCount ) {
		var totalWidth = Settings.TotalHoleWidthMax;
		// Add main hole.
		var holeBaseAngle = 0f;
		AddHole ( containerTf, platformCircle, ref holeBaseAngle, ref totalWidth, Settings.MainHoleWidthMin, Settings.MainHoleWidthMax, isMain : true );
		// Add secondary holes.
		int actualCount = 1;
		var holesLeft = holeCount;
		while ( --holesLeft > 0 ) {
			// Reserve some width for the rest of the holes.
			var minTotalWidthForOtherHoles = ( holesLeft - 1 ) * Settings.SecondaryHoleWidthMin;
			var maxWidth = totalWidth - minTotalWidthForOtherHoles;
			if ( maxWidth < Settings.SecondaryHoleWidthMin )
				continue;	// Too many holes, it's not possible to fit them all.

			AddHole ( containerTf, platformCircle, ref holeBaseAngle, ref totalWidth, Settings.SecondaryHoleWidthMin, maxWidth, isMain : false );
			actualCount++;
		}

		return	actualCount;
	}

	private bool AddHole (
		Transform containerTf, PlatformCircle platformCircle,
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

		var holePrefab = PrefabDatabase.Filter ( flags, desiredWidth, desiredWidth ).FirstOrDefault ();
		if ( holePrefab == null )
			return	false;

		var hole = InstantiatePlatform ( holePrefab, baseAngle, containerTf );
		var actualWidth = hole.AngleWidth;
		platformCircle.Add ( hole, baseAngle, baseAngle + actualWidth );
		baseAngle += actualWidth;
		totalWidth -= actualWidth;
		return	true;
	}

	private void SeparateHoles ( PlatformCircle platformCircle ) {
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

	private void ShakeHoles ( PlatformCircle platformCircle ) {
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

	private void SyncHoleAngles ( PlatformCircle platformCircle ) {
		foreach ( var fragment in platformCircle ) {
			var hole = fragment.Element;
			hole.StartAngleLocal = fragment.Range.Start;
		}
	}

	private void GeneratePlatforms ( Transform containerTf, PlatformCircle platformCircle ) {
		while ( platformCircle.TryFindEmptyRange ( out var emptyRange ) ) {
			var start = emptyRange.Start;
			var platformPrefab = PrefabDatabase
				.Filter ( PlatformKindFlags.Platform, Settings.PlatformWidthMin, emptyRange.Width () )
				.OrderByDescending ( p => p.AngleWidth )
				.FirstOrDefault ();
			if ( platformPrefab == null ) {
				Debug.LogWarning ( $"No suitable platform was found for the range {emptyRange} at {containerTf.name}." );
				// Fill whole range to not revisit it in the next iteration.
				platformCircle.Add ( null, emptyRange );
				continue;
			}

			var platform = InstantiatePlatform ( platformPrefab, start, containerTf );
			platformCircle.Add ( platform, start, start + platform.AngleWidth );
		}
	}

	private static Platform InstantiatePlatform ( Platform prefab, float startAngle, Transform parent ) {
		var platform = Instantiate ( prefab, parent );
		platform.transform.localPosition = Vector3.zero;
		platform.StartAngleLocal = startAngle;
		return	platform;
	}

	private void GenerateHorzObstacles (
		Transform platformContainerTf,
		PlatformCircle platformCircle, PlatformCircle obstacleCircle,
		List <Range <float>> platformRanges
	) {
		var obstacleCount = UnityEngine.Random.Range ( Settings.HorzObstacleCountMin, Settings.HorzObstacleCountMax + 1 );
		if ( obstacleCount == 0 )
			return;

		// TODO: cut ranges under holes of previous floor. We don't want player to get sick of falling onto obstacles while following the right path.
		// TODO: generate obstacles over holes.
		var widthLeft = Settings.TotalHorzObstacleWidthMax;
		GenerateHorzObstaclesOverPlatforms ( platformContainerTf, obstacleCircle, platformRanges, ref obstacleCount, ref widthLeft );
	}

	private void GenerateHorzObstaclesOverPlatforms (
		Transform platformContainerTf,
		PlatformCircle obstacleCircle,
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
				platformContainerTf, obstacleCircle, range,
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
		Transform containerTf,
		PlatformCircle obstacleCircle,
		Range <float> targetRange,
		ref float widthLeft,
		out Range <float> occupiedRange
	) {
		var maxWidth = Mathf.Min ( Settings.HorzObstacleWidthMax, widthLeft, targetRange.Width () );
		var desiredWidth = RandomHelper.Range ( Settings.HorzObstacleWidthMin, maxWidth, Settings.HorzObstacleWidthStep );
		var prefab = PrefabDatabase.Filter ( PlatformKindFlags.KillerObstacle | PlatformKindFlags.Platform, desiredWidth, desiredWidth )
			.FirstOrDefault ();
		if ( prefab == null ) {
			occupiedRange = default;
			return	false;
		}

		var actualWidth = prefab.AngleWidth;
		var baseAngle = RandomHelper.Range ( targetRange.Start, targetRange.End - actualWidth, Settings.HorzObstacleWidthStep );
		var instance = InstantiatePlatform ( prefab, baseAngle, containerTf );
		occupiedRange = Range.Create ( baseAngle, baseAngle + actualWidth );
		obstacleCircle.Add ( instance, occupiedRange );
		widthLeft -= actualWidth;
		return	true;
	}

	private Column GenerateColumn ( Transform containerTf, float floorHeight ) {
		var columns = PrefabDatabase.PredefinedColumns;
		if ( columns.Count == 0 ) {
			Debug.LogWarning ( $"No suitable column was found at {containerTf.name}." );
			return	null;
		}

		var prefab = columns [UnityEngine.Random.Range ( 0, columns.Count )];
		var column = Instantiate ( prefab, containerTf );
		var columnTf = column.transform;
		columnTf.localPosition = Vector3.zero;
		var scale = columnTf.localScale;
		scale.y = floorHeight / column.InitialHeight;
		columnTf.localScale = scale;
		return	column;
	}
}
