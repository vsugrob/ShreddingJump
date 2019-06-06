using UnityEngine;
using System.Linq;

public class LevelController : MonoBehaviour {
	public BouncingBallCharacter Character { get; private set; }
	[SerializeField]
	private Transform _floorsContainer = null;
	public Transform FloorsContainer => _floorsContainer;

	private void Start () {
		Character = FindObjectOfType <BouncingBallCharacter> ();
		Character.KillerObstacleHit += Character_KillerObstacleHit;
		var seed = Random.Range ( int.MinValue, int.MaxValue );
		seed = -799463940;
		Random.InitState ( seed );
#pragma warning disable CS0618 // Type or member is obsolete
		Debug.Log ( $"Random seed: {Random.seed}" );
#pragma warning restore CS0618 // Type or member is obsolete
		GenerateLevel ();
	}

	private void Character_KillerObstacleHit ( BouncingBallCharacter character, KillerObstacle obstacle ) {
		//Time.timeScale = 0;
	}

	private void GenerateLevel () {
		var generator = GetComponent <LevelGenerator> ();
		if ( generator == null )
			return;

		DestroyChildren ( FloorsContainer );
		var dummyFloorGo = CreateDummyFloor ();
		var generatorEn = generator
			.Generate ( dummyFloorGo )
			.Take ( 100 );
		foreach ( var floor in generatorEn ) {}
	}
	// TODO: move to HierarchyHelper or smth alike.
	private static void DestroyChildren ( Transform rootTf ) {
		var count = rootTf.childCount;
		for ( int i = 0 ; i < count ; i++ ) {
			var childTf = rootTf.GetChild ( i );
			Destroy ( childTf.gameObject );
		}
	}

	private FloorRoot CreateDummyFloor () {
		var rooftopY = Character.transform.position.y + Character.JumpHeight / 2;
		return	FloorRoot.Create ( FloorsContainer, -1, rooftopY );
	}
}
