using UnityEngine;

public class ObjectRemover : MonoBehaviour {
	[SerializeField]
	private ObjectRemoverSettings _settings;
	public ObjectRemoverSettings Settings {
		get => _settings;
		set => _settings = value;
	}
	private float startTime = float.NaN;
	public bool Started => !float.IsNaN ( startTime );
	public bool Finished { get; private set; }
	private Renderer [] renderers;
	private float [] initialAlphas;
	private Vector3 initialScale;

	public void StartRemoval () {
		startTime = Time.time;
		renderers = GetComponentsInChildren <Renderer> ();
		if ( Settings.FadeAlpha )
			CaptureInitialAlpha ();

		if ( Settings.ShrinkScale )
			initialScale = transform.localScale;
	}

	private void Update () {
		if ( !Started || Finished )
			return;

		var timeSinceStart = Time.time - startTime;
		var t = Mathf.Clamp01 ( timeSinceStart / Settings.AnimationDuration );
		t = Settings.AnimationCurve.Evaluate ( t );
		if ( !Finished && t == 1 )
			Finished = true;

		if ( Settings.FadeAlpha )
			SetAlphas ( t );

		if ( Settings.ShrinkScale )
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

	public static bool StartRemoval ( Transform transform, ObjectRemoverSettings settings ) {
		var remover = transform.gameObject.AddComponent <ObjectRemover> ();
		remover.Settings = settings;
		remover.StartRemoval ();
		return	true;
	}
}
