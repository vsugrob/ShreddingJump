using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent ( typeof ( FpsCounterComponent ) )]
public class DevStatsOverlay : MonoBehaviour {
	[SerializeField]
	private Text fpsText;
	public Text FpsText => fpsText;
	private FpsCounterComponent fpsCounter;

	private void Start () {
		fpsCounter = GetComponent <FpsCounterComponent> ();
	}

	private void Update () {
		if ( fpsText == null )
			return;

		fpsText.text = fpsCounter.Fps.ToString ( "0.0", CultureInfo.InvariantCulture );
	}
}
