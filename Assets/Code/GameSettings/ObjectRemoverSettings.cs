using System;
using UnityEngine;

[Serializable]
public class ObjectRemoverSettings {
	[SerializeField]
	private float _animationDuration = 1;
	public float AnimationDuration {
		get => _animationDuration;
		set => _animationDuration = value;
	}
	[SerializeField]
	private AnimationCurve _animationCurve = new AnimationCurve ( new Keyframe ( 0, 0 ), new Keyframe ( 1, 1 ) );
	public AnimationCurve AnimationCurve {
		get => _animationCurve;
		set => _animationCurve = value;
	}
	[SerializeField]
	private bool _fadeAlpha = false;
	public bool FadeAlpha {
		get => _fadeAlpha;
		set => _fadeAlpha = value;
	}
	[SerializeField]
	private bool _shrinkScale = true;
	public bool ShrinkScale {
		get => _shrinkScale;
		set => _shrinkScale = value;
	}
}
