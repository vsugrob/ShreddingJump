using System.Collections.Generic;
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
	[SerializeField]
	private List <GameObject> _flakes = new List <GameObject> ();
	public List <GameObject> Flakes {
		get => _flakes;
		set => _flakes = value;
	}
}
