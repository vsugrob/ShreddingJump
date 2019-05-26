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
	private List <Platform> _platforms = new List <Platform> ();
	public List <Platform> Platforms {
		get => _platforms;
		set => _platforms = value;
	}
	[SerializeField]
	private List <Platform> _walls = new List <Platform> ();
	public List <Platform> Walls {
		get => _walls;
		set => _walls = value;
	}
	[SerializeField]
	private List <Platform> _platformObstacles = new List <Platform> ();
	public List <Platform> PlatformObstacles {
		get => _platformObstacles;
		set => _platformObstacles = value;
	}
	public Platform RandomPlatform => RandomElement ( Platforms );
	public Platform RandomWall => RandomElement ( Walls );
	public Platform RandomPlatformObstacle => RandomElement ( PlatformObstacles );

	private static TElement RandomElement <TElement> ( IList <TElement> list ) {
		return	list [Random.Range ( 0, list.Count )];
	}
}
