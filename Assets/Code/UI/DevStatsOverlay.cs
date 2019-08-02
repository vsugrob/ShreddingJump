using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent ( typeof ( FpsCounterComponent ) )]
public class DevStatsOverlay : MonoBehaviour {
	[SerializeField]
	private Text _fpsText;
	public Text FpsText => _fpsText;
	private FpsCounterComponent fpsCounter;
	[SerializeField]
	private Text _characterVerticalVelocityText;
	public Text CharacterVerticalVelocityText => _characterVerticalVelocityText;
	[SerializeField]
	private Text _levelTimeText;
	public Text LevelTimeText => _levelTimeText;
	[SerializeField]
	private Toggle _smoothCheckbox;
	public Toggle SmoothCheckbox => _smoothCheckbox;
	private BouncingBallCharacter character;
	private LevelController levelController;

	private void Start () {
		fpsCounter = GetComponent <FpsCounterComponent> ();
		character = FindObjectOfType <BouncingBallCharacter> ();
		levelController = FindObjectOfType <LevelController> ();
		SmoothCheckbox.isOn = GameSettings.Singleton.SmootInputAndCamera;
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

	public void OnSmoothCheckboxValueChanged ( bool isChecked ) {
		GameSettings.Singleton.SmootInputAndCamera = isChecked;
	}
}
