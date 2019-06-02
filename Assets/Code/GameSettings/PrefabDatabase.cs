using System;
using System.Collections.Generic;
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

	public IEnumerable <Platform> Filter ( Func <Platform, bool> predicate ) {
		return	AllPlatforms.Where ( p => p != null && predicate ( p ) );
	}

	public IEnumerable <Platform> Filter ( PlatformKindFlags kindMask ) {
		return	Filter ( p => ( p.Kind & kindMask ) != PlatformKindFlags.None );
	}
}
