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
	private float _holeStartAngleWidth = 22.5f;
	public float HoleStartAngleWidth {
		get => _holeStartAngleWidth;
		set => _holeStartAngleWidth = value;
	}
	[SerializeField]
	private float _holeEndAngleWidth = 360;
	public float HoleEndAngleWidth {
		get => _holeEndAngleWidth;
		set => _holeEndAngleWidth = value;
	}
	[SerializeField]
	private float _holeAngleWidthStep = 22.5f;
	public float HoleAngleWidthStep {
		get => _holeAngleWidthStep;
		set => _holeAngleWidthStep = value;
	}
	[SerializeField]
	private List <Platform> _predefinedPlatforms = new List <Platform> ();
	public List <Platform> PredefinedPlatforms {
		get => _predefinedPlatforms;
		set => _predefinedPlatforms = value;
	}
	private List <Platform> allPlatforms = new List <Platform> ();
	private ReadOnlyCollection <Platform> allPlatformsRo = null;
	public ReadOnlyCollection <Platform> AllPlatforms {
		get {
			if ( allPlatformsVersion < 0 )
				InitHoles ();

			if ( allPlatformsVersion != version ) {
				allPlatforms.Clear ();
				allPlatforms.AddRange ( PredefinedPlatforms.Concat ( holesByWidth.Values ) );
				allPlatformsVersion = version;
			}

			if ( allPlatformsRo == null )
				allPlatformsRo = new ReadOnlyCollection <Platform> ( allPlatforms );

			return	allPlatformsRo;
		}
	}
	private int allPlatformsVersion = -1;
	private int version = 0;
	private Dictionary <float, Platform> holesByWidth = new Dictionary <float, Platform> ();
	private ReadOnlyDictionary <float, Platform> holesByWidthRo;
	public ReadOnlyDictionary <float, Platform> HolesByWidth {
		get {
			if ( holesByWidthRo == null )
				holesByWidthRo = new ReadOnlyDictionary <float, Platform> ( holesByWidth );

			return	holesByWidthRo;
		}
	}

	public void Init () {
		InitHoles ();
	}

	public void InitHoles () {
		InitHoles ( HoleStartAngleWidth, HoleEndAngleWidth, HoleAngleWidthStep );
	}

	public void InitHoles ( float startAngleWidth, float endAngleWidth, float step ) {
		if ( endAngleWidth <= startAngleWidth )
			throw new ArgumentException ( $"{nameof ( endAngleWidth )} must be greater than {nameof ( startAngleWidth )}.", nameof ( endAngleWidth ) );
		else if ( step <= float.Epsilon )
			throw new ArgumentException ( $"{nameof ( step )} must be greater than 0.", nameof ( step ) );

		holesByWidth.Clear ();
		for ( float width = startAngleWidth ; width < endAngleWidth ; width += step ) {
			AddHole ( width );
		}

		AddHole ( endAngleWidth );
	}

	public void InitHoles ( params float [] angleWidths ) {
		InitHoles ( ( IEnumerable <float> ) angleWidths );
	}

	public void InitHoles ( IEnumerable <float> angleWidths ) {
		holesByWidth.Clear ();
		foreach ( var angleWidth in angleWidths ) {
			if ( holesByWidth.TryGetValue ( angleWidth, out var platform ) )
				continue;

			AddHole ( angleWidth );
		}
	}

	public void AddHole ( float angleWidth ) {
		var platformGo = new GameObject ( $"Hole{angleWidth}", typeof ( Platform ) );
		platformGo.transform.position = Vector3.right * 1e4f;		// Move it out of sight.
		var platform = platformGo.GetComponent <Platform> ();
		platform.Kind = PlatformKindFlags.Hole;
		platform.StartAngle = 0;
		platform.EndAngle = angleWidth;
		holesByWidth.Add ( angleWidth, platform );
		version++;
	}

	public IEnumerable <Platform> Filter ( Func <Platform, bool> predicate ) {
		return	AllPlatforms.Where ( p => p != null && predicate ( p ) );
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
