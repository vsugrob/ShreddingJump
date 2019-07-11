using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent ( typeof ( FpsCounterComponent ) )]
public class DevStatsOverlay : MonoBehaviour {
	[SerializeField]
	private Text fpsText;
	public Text FpsText => fpsText;
	private FpsCounterComponent fpsCounter;
	[SerializeField]
	private Text characterVerticalVelocityText;
	public Text CharacterVerticalVelocityText => characterVerticalVelocityText;
	[SerializeField]
	private Text levelTimeText;
	public Text LevelTimeText => levelTimeText;
	private BouncingBallCharacter character;
	private LevelController levelController;

	private void Start () {
		fpsCounter = GetComponent <FpsCounterComponent> ();
		character = FindObjectOfType <BouncingBallCharacter> ();
		levelController = FindObjectOfType <LevelController> ();
	}

	private void Update () {
		if ( FpsText != null )
			FpsText.text = fpsCounter.Fps.ToString ( "0.0", CultureInfo.InvariantCulture );

		if ( CharacterVerticalVelocityText != null && character != null )
			CharacterVerticalVelocityText.text = $"Vert. Vel: {character.VerticalVelocity:0.0}".ToString ( CultureInfo.InvariantCulture );

		if ( LevelTimeText != null && !float.IsInfinity ( levelController.TimeSinceLevelStart ) )
			LevelTimeText.text = levelController.TimeSinceLevelStart.ToString ( "0.0", CultureInfo.InvariantCulture ) + "s";
	}

	public void OnColorizeButtonClick () {
		var levelController = FindObjectOfType <LevelController> ();
		if ( levelController == null )
			return;

		levelController.ColorizeLevel ();
	}
}
