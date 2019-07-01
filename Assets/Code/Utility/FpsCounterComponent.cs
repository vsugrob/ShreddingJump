using UnityEngine;

public class FpsCounterComponent : MonoBehaviour {
    private FpsCounter counter;
	[SerializeField]
	private float _windowIntervalLimit = 0.25f;
	public float WindowIntervalLimit {
		get => _windowIntervalLimit;
		set {
			if ( counter != null ) counter.WindowIntervalLimit = value;
			_windowIntervalLimit = value;
		}
	}
	[SerializeField]
	private int _windowSamplesLimit = 100;
	public int WindowSamplesLimit {
		get => _windowSamplesLimit;
		set {
			if ( counter != null ) counter.WindowSamplesLimit = value;
			_windowSamplesLimit = value;
		}
	}
	public float Fps => counter.Fps;
	public int FpsRounded => counter.FpsRounded;

	private void Start () {
		counter = new FpsCounter ( WindowIntervalLimit, WindowSamplesLimit );
	}

	private void Update () {
		counter.AddSample ( Time.realtimeSinceStartup );
	}
}
