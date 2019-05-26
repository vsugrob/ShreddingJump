using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PrefabDatabase : ScriptableObject {
	[SerializeField]
	private GameObject _character;
	public GameObject Character {
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
	private List <GameObject> _platforms = new List <GameObject> ();
	public List <GameObject> Platforms {
		get => _platforms;
		set => _platforms = value;
	}
	[SerializeField]
	private List <GameObject> _walls = new List <GameObject> ();
	public List <GameObject> Walls {
		get => _walls;
		set => _walls = value;
	}
	[SerializeField]
	private List <GameObject> _platformObstacles = new List <GameObject> ();
	public List <GameObject> PlatformObstacles {
		get => _platformObstacles;
		set => _platformObstacles = value;
	}
	public GameObject RandomPlatform => RandomElement ( Platforms );
	public GameObject RandomWall => RandomElement ( Walls );
	public GameObject RandomPlatformObstacle => RandomElement ( PlatformObstacles );

	private static TElement RandomElement <TElement> ( IList <TElement> list ) {
		return	list [Random.Range ( 0, list.Count )];
	}
}
