using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
	[SerializeField]
	private LevelGeneratorSettings _settings;
	public LevelGeneratorSettings Settings {
		get => _settings ?? ( _settings = new LevelGeneratorSettings () );
		set => _settings = value;
	}
	[SerializeField]
	private PrefabDatabase _prefabDatabase;
	public PrefabDatabase PrefabDatabase {
		get => _prefabDatabase ?? ( _prefabDatabase = new PrefabDatabase () );
		set => _prefabDatabase = value;
	}

	public IEnumerable <GameObject> Generate ( float lastFloorY ) {
		int i = 0;
		while ( true ) {

			i++;
			yield return	null;
		}
	}
}
