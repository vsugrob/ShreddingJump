using UnityEngine;

public class ObjectRemover : MonoBehaviour {
	[SerializeField]
	private float _animationDuration = 1;
	public float AnimationDuration => _animationDuration;
	[SerializeField]
	private bool _fadeAlpha = false;
	public bool FadeAlpha {
		get => _fadeAlpha;
		private set => _fadeAlpha = value;
	}
	[SerializeField]
	private bool _shrinkScale = true;
	public bool ShrinkScale {
		get => _shrinkScale;
		private set => _shrinkScale = value;
	}
	private float startTime = float.NaN;
	public bool Started => !float.IsNaN ( startTime );
	public bool Finished { get; private set; }
	private Renderer [] renderers;
	private float [] initialAlphas;
	private Vector3 initialScale;

	public void StartRemoval () {
		startTime = Time.time;
		renderers = GetComponentsInChildren<Renderer> ();
		if ( FadeAlpha )
			CaptureInitialAlpha ();

		if ( ShrinkScale )
			initialScale = transform.localScale;
	}

	private void Update () {
		if ( !Started || Finished )
			return;

		var timeSinceStart = Time.time - startTime;
		var t = Mathf.Clamp01 ( timeSinceStart / AnimationDuration );
		if ( !Finished && t == 1 )
			Finished = true;

		if ( FadeAlpha )
			SetAlphas ( t );

		if ( ShrinkScale )
			SetScale ( t );
	}

	private void CaptureInitialAlpha () {
		initialAlphas = new float [renderers.Length];
		for ( int i = 0 ; i < renderers.Length ; i++ ) {
			var renderer = renderers [i];
			initialAlphas [i] = renderer.sharedMaterial.color.a;
		}
	}

	private void SetAlphas ( float t ) {
		for ( int i = 0 ; i < renderers.Length ; i++ ) {
			var renderer = renderers [i];
			var material = renderer.material;
			var color = material.color;
			color.a = Mathf.Lerp ( initialAlphas [i], 0, t );
			material.color = color;
			renderer.material = material;
		}
	}

	private void SetScale ( float t ) {
		transform.localScale = Vector3.Lerp ( initialScale, Vector3.zero, t );
	}

	public static bool StartRemoval ( Transform transform, bool fadeAlpha = false, bool shrinkScale = true ) {
		var remover = transform.gameObject.AddComponent <ObjectRemover> ();
		remover.FadeAlpha = fadeAlpha;
		remover.ShrinkScale = shrinkScale;
		remover.StartRemoval ();
		return	true;
	}
}
