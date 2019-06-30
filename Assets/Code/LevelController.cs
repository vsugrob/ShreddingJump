using UnityEngine;
using System.Linq;
using System;

public class LevelController : MonoBehaviour {
	public BouncingBallCharacter Character { get; private set; }
	[SerializeField]
	private Transform _floorsContainer = null;
	public Transform FloorsContainer => _floorsContainer;

	private void Start () {
		Character = FindObjectOfType <BouncingBallCharacter> ();
		Character.KillerObstacleHit += Character_KillerObstacleHit;
		Character.FinishLineHit += Character_FinishLineHit;
		var seed = UnityEngine.Random.Range ( int.MinValue, int.MaxValue );
		seed = 1821742774;
		UnityEngine.Random.InitState ( seed );
#pragma warning disable CS0618 // Type or member is obsolete
		Debug.Log ( $"Random seed: {UnityEngine.Random.seed}" );
#pragma warning restore CS0618 // Type or member is obsolete
		GenerateLevel ();
	}

	private void Character_KillerObstacleHit ( BouncingBallCharacter character, KillerObstacle obstacle ) {
		//Time.timeScale = 0;
	}

	private void Character_FinishLineHit ( BouncingBallCharacter character, FinishLine finishLine ) {
		// TODO: implement.
		//GameSettings.Singleton.FinishLevelSound.PlayOneShot ( someAudioSource );
	}

	private void GenerateLevel () {
		var generator = GetComponent <StandardLevelGenerator> ();
		if ( generator == null )
			return;

		DestroyChildren ( FloorsContainer );
		var dummyFloorInfo = CreateDummyFloor ();
		var generatorEn = generator
			.Generate ( dummyFloorInfo )
			.Take ( 3 );
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

	private FloorInfo CreateDummyFloor () {
		var rooftopY = Character.transform.position.y + Character.JumpHeight / 2;
		var floorRoot = FloorRoot.Create ( FloorsContainer, -1, rooftopY );
		var baseAngle = 0f;
		var platformsContainer = PlatformContainer.Create ( floorRoot.transform, baseAngle );
		var platformCircle = new PlatformCircle ();
		var fullRoundHoleGo = new GameObject ( "InitialHole360", typeof ( Platform ) );
		var holePlatform = fullRoundHoleGo.GetComponent <Platform> ();
		holePlatform.Kind = PlatformKindFlags.Hole;
		holePlatform.StartAngle = 0;
		holePlatform.EndAngle = 360;
		holePlatform.transform.SetParent ( platformsContainer.transform, worldPositionStays : false );
		platformCircle.Add ( holePlatform, Range.Create ( 0, 360f ) );
		return	new FloorInfo ( floorRoot, baseAngle, platformCircle, new PlatformCircle () );
	}
}
