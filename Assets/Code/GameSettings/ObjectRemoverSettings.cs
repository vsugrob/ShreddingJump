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
	private bool _destroyOnAnimationEnd = true;
	public bool DestroyOnAnimationEnd {
		get => _destroyOnAnimationEnd;
		set => _destroyOnAnimationEnd = value;
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
	[SerializeField]
	private bool _enableFlyoff = true;
	public bool EnableFlyoff {
		get => _enableFlyoff;
		set => _enableFlyoff = value;
	}
	[SerializeField]
	private AnimationCurve _flyoffXCurve = new AnimationCurve ( new Keyframe ( 0, 0 ), new Keyframe ( 1, 10 ) );
	public AnimationCurve FlyoffXCurve {
		get => _flyoffXCurve;
		set => _flyoffXCurve = value;
	}
	[SerializeField]
	private AnimationCurve _flyoffYCurve = new AnimationCurve ( new Keyframe ( 0, 0 ), new Keyframe ( 0.3f, 1 ), new Keyframe ( 1, -10 ) );
	public AnimationCurve FlyoffYCurve {
		get => _flyoffYCurve;
		set => _flyoffYCurve = value;
	}
}
