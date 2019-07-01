using System;
using System.Collections.Generic;

public class FpsCounter {
	private List <float> samples = new List <float>();
	private float _windowIntervalLimit = 0.25f;
	public float WindowIntervalLimit {
		get => _windowIntervalLimit;
		set {
			if ( _windowIntervalLimit < float.Epsilon )
				throw new ArgumentException ( nameof ( value ) );

			_windowIntervalLimit = value;
		}
	}
	private int _windowSamplesLimit = 100;
	public int WindowSamplesLimit {
		get => _windowSamplesLimit;
		set {
			if ( value < 2 )
				throw new ArgumentException ( nameof ( value ) );

			_windowSamplesLimit = value;
		}
	}
	private float fps;
	public float Fps {
		get => fps;
		private set {
			fps = value;
			FpsRounded = ( int ) Math.Round ( fps );
		}
	}
	public int FpsRounded { get; private set; }

	public FpsCounter ( float windowIntervalLimit, int windowSamplesLimit ) {
		this.WindowIntervalLimit = windowIntervalLimit;
		this.WindowSamplesLimit = WindowSamplesLimit;
	}

	public void AddSample ( float timestamp ) {
		samples.Add ( timestamp );
		FitSampleListInWindowIntervalLimit ();
		RemoveExceedingSamples ();
		CalculateFps ();
	}

	private void FitSampleListInWindowIntervalLimit () {
		if ( samples.Count < 2 )
			return;

		var windowStart = samples [0];
		var windowEnd = samples [samples.Count - 1];
		while ( windowEnd - windowStart > WindowIntervalLimit ) {
			samples.RemoveAt ( 0 );
			windowStart = samples [0];
		}
	}

	private void RemoveExceedingSamples () {
		while ( samples.Count > WindowSamplesLimit ) {
			samples.RemoveAt ( 0 );
		}
	}

	private void CalculateFps () {
		if ( samples.Count < 2 ) {
			Fps = 0;
			return;
		}

		var count1 = samples.Count - 1;
		var windowInterval = samples [count1] - samples [0];
		Fps = count1 / windowInterval;
	}
}
