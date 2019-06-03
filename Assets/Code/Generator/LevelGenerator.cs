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

	public IEnumerable <GameObject> Generate ( GameObject prevFloor, int nextFloorIndex = 0 ) {
		var floorHeight = UnityEngine.Random.Range ( Settings.FloorHeightMin, Settings.FloorHeightMax );
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
		GenerateHoles ( floorTf, platformCircle );
		GeneratePlatforms ( floorTf, platformCircle );
	}

	private void GenerateHoles ( Transform floorTf, PlatformCircle platformCircle ) {
		var holeCount = UnityEngine.Random.Range ( Settings.HoleCountMin, Settings.HoleCountMax + 1 );
		if ( holeCount > 0 ) {
			AddHoles ( platformCircle, holeCount );
			SeparateHoles ( platformCircle );
			ShakeHoles ( platformCircle );
			MaterializeHoles ( floorTf, platformCircle );
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
		var desiredWidth = RandomHelper.Range ( minWidth, maxWidth, Settings.HoleAngleWidthStep );
		if ( desiredWidth > totalWidth )
			desiredWidth = totalWidth;

		var holePrefab = PrefabDatabase.Filter ( PlatformKindFlags.Hole, desiredWidth, desiredWidth ).FirstOrDefault ();
		if ( holePrefab == null )
			return	false;

		var actualWidth = holePrefab.AngleWidth;
		platformCircle.Add ( holePrefab, baseAngle, baseAngle + actualWidth );
		baseAngle += actualWidth;
		totalWidth -= desiredWidth;
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
			range = range.Add ( offset );
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
			var maxStart = Mathf.FloorToInt ( ( nextStart - range.Width () ) / Settings.SecondaryHoleAngleWidthMin ) * Settings.SecondaryHoleAngleWidthMin;
			if ( maxStart < start ) {
				nextStart = range.Start;
				continue;
			}

			var newStart = RandomHelper.Range ( start, maxStart, Settings.SecondaryHoleAngleWidthMin );
			platformCircle.Remove ( start );
			range = range.Add ( newStart - start );
			platformCircle.Add ( fragment.Element, range );
			nextStart = range.Start;
		}
	}

	private void MaterializeHoles ( Transform floorTf, PlatformCircle platformCircle ) {
		foreach ( var fragment in platformCircle ) {
			var hole = InstantiatePlatform ( fragment.Element, fragment.Range.Start, floorTf );
			fragment.Element = hole;
		}
	}

	private void GeneratePlatforms ( Transform floorTf, PlatformCircle platformCircle ) {
		while ( platformCircle.TryFindEmptyRange ( out var emptyRange ) ) {
			var start = emptyRange.Start;
			var platformPrefab = PrefabDatabase
				.Filter ( PlatformKindFlags.Platform, Settings.PlatformAngleWidthMin, emptyRange.Width () )
				.OrderByDescending ( p => p.AngleWidth )
				.FirstOrDefault ();
			if ( platformPrefab == null ) {
				Debug.LogWarning ( $"No suitable platform was found for the range {emptyRange} at {floorTf.name}." );
				// Fill whole range to not revisit it in the next iteration.
				platformCircle.Add ( null, emptyRange );
				continue;
			}

			var platform = InstantiatePlatform ( platformPrefab, start, floorTf );
			platformCircle.Add ( platform, start, start + platform.AngleWidth );
		}
	}

	private static Platform InstantiatePlatform ( Platform prefab, float startAngle, Transform parent ) {
		var platform = Instantiate ( prefab, parent );
		platform.transform.localPosition = Vector3.zero;
		platform.StartAngleWorld = startAngle;
		return	platform;
	}
}
