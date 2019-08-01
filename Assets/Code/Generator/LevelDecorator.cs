using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelDecorator : MonoBehaviour {
	[SerializeField]
	private PrefabDatabase _prefabDatabase;
	public PrefabDatabase PrefabDatabase {
		get => _prefabDatabase ?? ( _prefabDatabase = ScriptableObject.CreateInstance <PrefabDatabase> () );
		set => _prefabDatabase = value;
	}

	public void Decorate ( IList <FloorInfo> floors ) {
		for ( int i = 0 ; i < floors.Count ; i++ ) {
			var floor = floors [i];
			var prefab = PrefabDatabase.Flakes.FirstOrDefault ();
			if ( prefab == null )
				continue;

			var flakes = Instantiate ( prefab, floor.FloorRoot.transform );
			flakes.transform.localPosition = Vector3.zero;
		}
	}
}
