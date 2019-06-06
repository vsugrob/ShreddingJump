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
			var platformsContainerGo = new GameObject ( $"{nameof ( PlatformContainer )}{baseAngle}", typeof ( PlatformContainer ) );
			var platformsContainerTf = platformsContainerGo.transform;
			platformsContainerTf.SetParent ( floorTf, worldPositionStays : false );
			GenerateFloor ( floorTf, platformsContainerTf, floorHeight );
			var floorCompleteTriggerGo = Instantiate ( PrefabDatabase.FloorCompleteTrigger, floorTf );
			floorCompleteTriggerGo.transform.localPosition = Vector3.zero;
			floorY -= floorHeight;
			i++;
			nextFloorIndex++;
			yield return floorGo;
		}
	}

	private void GenerateFloor ( Transform floorTf, Transform platformContainerTf, float floorHeight ) {
		var platformCircle = new PlatformCircle ();
		GenerateHoles ( platformContainerTf, platformCircle );
		GeneratePlatforms ( platformContainerTf, platformCircle );
		GenerateColumn ( floorTf, floorHeight );
	}

	private void GenerateHoles ( Transform containerTf, PlatformCircle platformCircle ) {
		var holeCount = UnityEngine.Random.Range ( Settings.HoleCountMin, Settings.HoleCountMax + 1 );
		if ( holeCount > 0 ) {
			AddHoles ( platformCircle, holeCount );
			SeparateHoles ( platformCircle );
			ShakeHoles ( platformCircle );
			MaterializeHoles ( containerTf, platformCircle );
		}
	}

	private int AddHoles ( PlatformCircle platformCircle, int holeCount ) {
		var totalWidth = Settings.TotalHoleWidthMax;
		// Add main hole.
		var holeBaseAngle = 0f;
		AddHole ( platformCircle, ref holeBaseAngle, ref totalWidth, Settings.MainHoleWidthMin, Settings.MainHoleWidthMax );
		// Add secondary holes.
		int actualCount = 1;
		var holesLeft = holeCount;
		while ( --holesLeft > 0 ) {
			// Reserve some width for the rest of the holes.
			var minTotalWidthForOtherHoles = ( holesLeft - 1 ) * Settings.SecondaryHoleWidthMin;
			var maxWidth = totalWidth - minTotalWidthForOtherHoles;
			if ( maxWidth < Settings.SecondaryHoleWidthMin )
				continue;	// Too many holes, it's not possible to fit them all.

			AddHole ( platformCircle, ref holeBaseAngle, ref totalWidth, Settings.SecondaryHoleWidthMin, maxWidth );
			actualCount++;
		}

		return	actualCount;
	}

	private bool AddHole ( PlatformCircle platformCircle, ref float baseAngle, ref float totalWidth, float minWidth, float maxWidth ) {
		var desiredWidth = RandomHelper.Range ( minWidth, maxWidth, Settings.HoleWidthStep );
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
			var maxStart = Mathf.FloorToInt ( ( nextStart - range.Width () ) / Settings.SecondaryHoleWidthMin ) * Settings.SecondaryHoleWidthMin;
			if ( maxStart < start ) {
				nextStart = range.Start;
				continue;
			}

			var newStart = RandomHelper.Range ( start, maxStart, Settings.SecondaryHoleWidthMin );
			if ( newStart != start ) {
				platformCircle.Remove ( start );
				range = range.Add ( newStart - start );
				platformCircle.Add ( fragment.Element, range );
			}

			nextStart = range.Start;
		}
	}

	private void MaterializeHoles ( Transform containerTf, PlatformCircle platformCircle ) {
		foreach ( var fragment in platformCircle ) {
			var hole = InstantiatePlatform ( fragment.Element, fragment.Range.Start, containerTf );
			fragment.Element = hole;
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
		platform.StartAngleWorld = startAngle;
		return	platform;
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
