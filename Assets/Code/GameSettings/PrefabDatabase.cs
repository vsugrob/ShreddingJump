using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class PrefabDatabase : ScriptableObject {
	[SerializeField]
	private BouncingBallCharacter _character;
	public BouncingBallCharacter Character {
		get => _character;
		set => _character = value;
	}
	[SerializeField]
	private GameObject _floorCompleteTrigger;
	public GameObject FloorCompleteTrigger {
		get => _floorCompleteTrigger;
		set => _floorCompleteTrigger = value;
	}
	[SerializeField]
	private List <Platform> _allPlatforms = new List <Platform> ();
	public List <Platform> AllPlatforms {
		get => _allPlatforms;
		set => _allPlatforms = value;
	}
	private Dictionary <float, Platform> holesByWidth = new Dictionary <float, Platform> ();
	private ReadOnlyDictionary <float, Platform> holesByWidthRo;
	public ReadOnlyDictionary <float, Platform> HolesByWidth {
		get {
			if ( holesByWidthRo == null )
				holesByWidthRo = new ReadOnlyDictionary <float, Platform> ( holesByWidth );

			return	holesByWidthRo;
		}
	}

	public void InitHoles ( params float [] angleWidths ) {
		InitHoles ( ( IEnumerable <float> ) angleWidths );
	}

	public void InitHoles ( IEnumerable <float> angleWidths ) {
		foreach ( var angleWidth in angleWidths ) {
			if ( holesByWidth.TryGetValue ( angleWidth, out var platform ) )
				continue;

			var platformGo = new GameObject ( $"Hole{angleWidth}", typeof ( Platform ) );
			platform.transform.position = Vector3.right * 1e4f;		// Move it out of sight.
			platform = platformGo.GetComponent <Platform> ();
			platform.Kind = PlatformKindFlags.Hole;
			platform.StartAngle = 0;
			platform.EndAngle = angleWidth;
			holesByWidth.Add ( angleWidth, platform );
		}
	}

	public IEnumerable <Platform> Filter ( Func <Platform, bool> predicate ) {
		return	AllPlatforms
			.Where ( p => p != null && predicate ( p ) )
			.Concat ( holesByWidth.Values.Where ( predicate ) );
	}

	public IEnumerable <Platform> Filter ( PlatformKindFlags requiredFlags ) {
		return	Filter ( p => ( p.Kind & requiredFlags ) == requiredFlags );
	}

	public IEnumerable <Platform> Filter ( PlatformKindFlags requiredFlags, float minAngleWidth, float maxAngleWidth ) {
		return	Filter ( requiredFlags )
			.Where ( p => CheckValueFits ( p.AngleWidth, minAngleWidth, maxAngleWidth ) );
	}

	public IEnumerable <Platform> Filter (
		PlatformKindFlags requiredFlags,
		float minAngleWidth, float maxAngleWidth,
		float minHeight, float maxHeight
	) {
		return	Filter ( requiredFlags, minAngleWidth, maxAngleWidth )
			.Where ( p => CheckValueFits ( p.Height, minHeight, maxHeight ) );
	}

	bool CheckValueFits ( float value, float min, float max ) {
		return	min <= value && value <= max;
	}
}
