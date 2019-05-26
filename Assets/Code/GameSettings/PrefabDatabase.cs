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
}
