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
	private List <Platform> _predefinedPlatforms = new List <Platform> ();
	public List <Platform> PredefinedPlatforms {
		get => _predefinedPlatforms;
		set => _predefinedPlatforms = value;
	}
	[SerializeField]
	private List <Column> _predefinedColumns = new List <Column> ();
	public List <Column> PredefinedColumns {
		get => _predefinedColumns;
		set => _predefinedColumns = value;
	}

	public IEnumerable <Platform> Filter ( Func <Platform, bool> predicate ) {
		return	PredefinedPlatforms.Where ( p => p != null && predicate ( p ) );
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
