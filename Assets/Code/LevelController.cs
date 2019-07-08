using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {
	public BouncingBallCharacter Character { get; private set; }
	public LevelStartMenu LevelStartPanel { get; private set; }
	[SerializeField]
	private Transform _floorsContainer = null;
	public Transform FloorsContainer => _floorsContainer;
	[SerializeField]
	private Transform _runtimeObjectsContainer = null;
	public Transform RuntimeObjectsContainer => _runtimeObjectsContainer;
	[SerializeField]
	private int _floorCount = 30;
	public int FloorCount => _floorCount;

	private void Start () {
		Time.timeScale = 0;
		// Setup Character.
		Character = FindObjectOfType <BouncingBallCharacter> ();
		Character.KillerObstacleHit += Character_KillerObstacleHit;
		Character.FinishLineHit += Character_FinishLineHit;
		// Setup ui.
		LevelStartPanel = FindObjectOfType <LevelStartMenu> ();
		LevelStartPanel.StartLevel += LevelStartPanel_StartLevel;
		// Setup random seed.
		var seed = UnityEngine.Random.Range ( int.MinValue, int.MaxValue );
		seed = 1821742774;
		UnityEngine.Random.InitState ( seed );
#pragma warning disable CS0618 // Type or member is obsolete
		Debug.Log ( $"Random seed: {UnityEngine.Random.seed}" );
#pragma warning restore CS0618 // Type or member is obsolete
	}

	private void LevelStartPanel_StartLevel () {
		LevelStartPanel.gameObject.SetActive ( false );
		GenerateLevel ();
		Character.Restart ();
		Time.timeScale = 1;
	}

	private void Character_KillerObstacleHit ( BouncingBallCharacter character, KillerObstacle obstacle ) {
		Time.timeScale = 0;
		ShowLevelStartWindow ();
	}

	private void Character_FinishLineHit ( BouncingBallCharacter character, FinishLine finishLine ) {
		Time.timeScale = 0;
		// TODO: implement.
		//GameSettings.Singleton.FinishLevelSound.PlayOneShot ( someAudioSource );
		ShowLevelStartWindow ();
	}

	private void ShowLevelStartWindow () {
		LevelStartPanel.gameObject.SetActive ( true );
	}

	private void GenerateLevel () {
		var stdGen = GetComponent <StandardLevelGenerator> ();
		if ( stdGen == null )
			return;
		// Cleanup existing objects.
		DestroyChildren ( FloorsContainer );
		DestroyChildren ( RuntimeObjectsContainer );
		// Generate level.
		var dummyFloorInfo = CreateDummyFloor ();
		var genEn = stdGen
			.Generate ( dummyFloorInfo )
			.Take ( FloorCount - 1 );
		var finishGen = GetComponent <FinishLineGenerator> ();
		if ( finishGen != null )
			genEn = finishGen.Generate ( genEn )
				.Take ( FloorCount );

		genEn.Consume ();
		// Generate palette.
		var palette = GeneratePalette ();
		// Colorize level.
		RendererColorizer.SetColors ( FloorsContainer, palette, RuntimeObjectsContainer );
		RendererColorizer.SetColors ( Character.transform, palette, RuntimeObjectsContainer );
		var camera = Camera.main;
		if ( camera != null ) {
			camera.backgroundColor = ( Color ) palette [ColorRole.Background];
			camera.clearFlags = CameraClearFlags.SolidColor;
		}
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
	// TODO: move to level generator? Or separate component "LevelColorizer"?
	private IReadOnlyDictionary <ColorRole, HsvColor> GeneratePalette () {
		var generator = new HsvPaletteGenerator <ColorRole> ();
		var roles = ( ColorRole [] ) Enum.GetValues ( typeof ( ColorRole ) );
		// TODO: move to settings.
		const float MinDistance = 0.4f;
		const int ProbeIterations = 40;
		const bool UseAllIterations = false;
		const float ValueComponentScale = 0.5f;
		const float TargetHue = 0;
		const float TargetHueExponent = 2;
		const float RandomSaturationExponent = 1;
		const float RandomValueExponent = 1;
		HsvColor generateColorFunc () {
			return HsvColor.GenerateRandom ( TargetHue, TargetHueExponent, RandomSaturationExponent, RandomValueExponent );
		}

		ColorUtility.TryParseHtmlString ( "#E92F2F", out var obstacleColor );
		generator.AddColor ( ColorRole.Obstacle, ( HsvColor ) obstacleColor );
		for ( int i = 0 ; i < roles.Length ; i++ ) {
			var role = roles [i];
			if ( role == ColorRole.Unknown || generator.ContainsColor ( role ) )
				continue;

			generator.AddRandomColor (
				role,
				out var hsvColor, out var bestDistance,
				MinDistance, ProbeIterations, UseAllIterations,
				ValueComponentScale,
				generateColorFunc
			);
		}

		return	generator.Palette;
	}
}
