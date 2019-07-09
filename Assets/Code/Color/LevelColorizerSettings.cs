using UnityEngine;

[CreateAssetMenu]
public class LevelColorizerSettings : ScriptableObject {
	[Header ( "Palette" )]
	[SerializeField]
	private Color _obstacleColor = new Color32 ( r : 233, g : 47, b : 47, a : 255 );	// #E92F2F
	public Color ObstacleColor {
		get => _obstacleColor;
		set => _obstacleColor = value;
	}
	[SerializeField]
	private float _minColorDistance = 0.4f;
	public float MinColorDistance {
		get => _minColorDistance;
		set => _minColorDistance = value;
	}
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
	[SerializeField]
	private float _valueComponentScale = 0.5f;
	public float ValueComponentScale {
		get => _valueComponentScale;
		set => _valueComponentScale = value;
	}
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
	[SerializeField]
	private float _randomSaturationExponent = 1;
	public float RandomSaturationExponent {
		get => _randomSaturationExponent;
		set => _randomSaturationExponent = value;
	}
	[SerializeField]
	private float _randomValueExponent = 1;
	public float RandomValueExponent {
		get => _randomValueExponent;
		set => _randomValueExponent = value;
	}
}
