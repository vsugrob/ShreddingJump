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

	public IEnumerable <GameObject> Generate ( GameObject prevFloor, int nextFloorIndex = 0 ) {
		var floorHeight = Random.Range ( Settings.FloorHeightMin, Settings.FloorHeightMax );
		var prevFloorTf = prevFloor.transform;
		var floorY = prevFloorTf.position.y - floorHeight;
		var floorContainer = prevFloorTf.parent;
		var baseAngle = 260;
		int i = 0;
		while ( true ) {
			var floorGo = new GameObject ( $"Floor ({nextFloorIndex})", typeof ( FloorRoot ) );
			var floorTf = floorGo.transform;
			floorTf.SetParent ( floorContainer );
			floorTf.position = Vector3.up * floorY;
			GenerateFloor ( floorTf );
			var floorCompleteTriggerGo = Instantiate ( PrefabDatabase.FloorCompleteTrigger, floorTf );
			floorCompleteTriggerGo.transform.localPosition = Vector3.zero;
			floorY -= floorHeight;
			i++;
			nextFloorIndex++;
			yield return floorGo;
		}
	}

	private void GenerateFloor ( Transform floorTf ) {
		var platformCircle = new PlatformCircle ();
		GenerateHoles ( platformCircle );
		GeneratePlatforms ( floorTf, platformCircle );
	}

	private void GenerateHoles ( PlatformCircle platformCircle ) {
		var holeCount = Random.Range ( Settings.HoleCountMin, Settings.HoleCountMax );
		if ( holeCount > 0 ) {
			AddHoles ( platformCircle, holeCount );
			ShakeHoles ( platformCircle );
		}
	}

	private void AddHoles ( PlatformCircle platformCircle, int holeCount ) {
		var totalWidth = Settings.TotalHoleAngleWidthMax;
		// Add main hole.
		var holeBaseAngle = 0f;
		AddHole ( platformCircle, ref holeBaseAngle, ref totalWidth, Settings.MainHoleAngleWidthMin, Settings.MainHoleAngleWidthMax );
		// Add secondary holes.
		var holesLeft = holeCount;
		while ( --holesLeft > 0 ) {
			// Reserve some width for the rest of the holes.
			var minTotalWidthForOtherHoles = holesLeft * Settings.SecondaryHoleAngleWidthMin;
			var maxWidth = totalWidth - minTotalWidthForOtherHoles;
			if ( maxWidth < Settings.SecondaryHoleAngleWidthMin )
				break;

			AddHole ( platformCircle, ref holeBaseAngle, ref totalWidth, Settings.SecondaryHoleAngleWidthMin, maxWidth );
		}
	}

	private bool AddHole ( PlatformCircle platformCircle, ref float baseAngle, ref float totalWidth, float minWidth, float maxWidth ) {
		var width = RandomHelper.Range ( minWidth, maxWidth, Settings.HoleAngleWidthStep );
		if ( width > totalWidth )
			width = totalWidth;

		var hole = PrefabDatabase.Filter ( PlatformKindFlags.Hole, width, width ).FirstOrDefault ();
		if ( hole == null )
			return	false;

		platformCircle.Add ( hole, baseAngle, baseAngle + hole.AngleWidth );
		baseAngle += hole.AngleWidth;
		totalWidth -= width;
		return	true;
	}

	private void ShakeHoles ( PlatformCircle platformCircle ) {
		// Randomly "shake" hole positions.
		var nextStart = 360f;
		var holeFragments = platformCircle.ToArray ();
		for ( int i = holeFragments.Length - 1 ; i >= 0 ; i-- ) {
			var fragment = holeFragments [i];
			var range = fragment.Range;
			var start = range.Start;
			var width = range.End - start;
			var maxStart = Mathf.FloorToInt ( ( nextStart - width ) / Settings.SecondaryHoleAngleWidthMin ) * Settings.SecondaryHoleAngleWidthMin;
			var newStart = RandomHelper.Range ( start, maxStart, Settings.SecondaryHoleAngleWidthMin );
			platformCircle.Remove ( start );
			var offset = newStart - start;
			range.Start += offset;
			range.End += offset;
			platformCircle.Add ( fragment.Element, range );
			nextStart = range.Start;
		}
	}

	private void GeneratePlatforms ( Transform floorTf, PlatformCircle platformCircle ) {
		while ( platformCircle.TryFindEmptyRange ( out var emptyRange ) ) {
			var start = emptyRange.Start;
			var width = emptyRange.End - start;
			var platformPrefab = PrefabDatabase
				.Filter ( PlatformKindFlags.Platform, Settings.PlatformAngleWidthMin, width )
				.OrderByDescending ( p => p.AngleWidth )
				.FirstOrDefault ();
			if ( platformPrefab == null ) {
				Debug.LogWarning ( $"No suitable platform was found for the range {emptyRange} at {floorTf.name}." );
				// Fill whole range to not revisit it in the next iteration.
				platformCircle.Add ( null, emptyRange );
				continue;
			}

			var platform = Instantiate ( platformPrefab, floorTf );
			platform.transform.localPosition = Vector3.zero;
			platform.StartAngleWorld = start;
			platformCircle.Add ( platform, start, start + platform.AngleWidth );
		}
	}
}
