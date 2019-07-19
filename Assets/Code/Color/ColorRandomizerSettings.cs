using System;
using UnityEngine;

[Serializable]
public class ColorRandomizerSettings {
	[SerializeField]
	private float _minColorDistance = 0.4f;
	public float MinColorDistance {
		get => _minColorDistance;
		set => _minColorDistance = value;
	}
	[Header ( "Iterations" )]
	[SerializeField]
	private int _probeIterations = 40;
	public int ProbeIterations {
		get => _probeIterations;
		set => _probeIterations = value;
	}
	[SerializeField]
	private bool _useAllIterations = false;
	public bool UseAllIterations {
		get => _useAllIterations;
		set => _useAllIterations = value;
	}
	[Header ( "Hue" )]
	[SerializeField]
	private float _targetHue = 0;
	public float TargetHue {
		get => _targetHue;
		set => _targetHue = value;
	}
	[SerializeField]
	private float _targetHueExponent = 2;
	public float TargetHueExponent {
		get => _targetHueExponent;
		set => _targetHueExponent = value;
	}
	[Header ( "Saturation" )]
	[SerializeField]
	private float _randomSaturationExponent = 1;
	public float RandomSaturationExponent {
		get => _randomSaturationExponent;
		set => _randomSaturationExponent = value;
	}
	[SerializeField]
	private float _saturationMin = 0;
	public float SaturationMin {
		get => _saturationMin;
		set => _saturationMin = value;
	}
	[SerializeField]
	private float _saturationMax = 1;
	public float SaturationMax {
		get => _saturationMax;
		set => _saturationMax = value;
	}
	[Header ( "Value" )]
	[SerializeField]
	private float _randomValueExponent = 1;
	public float RandomValueExponent {
		get => _randomValueExponent;
		set => _randomValueExponent = value;
	}
	[SerializeField]
	private float _valueMin = 0;
	public float ValueMin {
		get => _valueMin;
		set => _valueMin = value;
	}
	[SerializeField]
	private float _valueMax = 1;
	public float ValueMax {
		get => _valueMax;
		set => _valueMax = value;
	}
	[SerializeField]
	private float _valueComponentScale = 0.5f;
	public float ValueComponentScale {
		get => _valueComponentScale;
		set => _valueComponentScale = value;
	}
}
