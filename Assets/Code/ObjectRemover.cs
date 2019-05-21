using UnityEngine;

public class ObjectRemover : MonoBehaviour {
	[SerializeField]
	private float _animationDuration = 1;
	public float AnimationDuration => _animationDuration;
	private float startTime = float.NaN;
	public bool Started => !float.IsNaN ( startTime );
	public bool Finished { get; private set; }
	private Renderer [] renderers;
	private float [] initialAlphas;

	public void StartRemoval () {
		startTime = Time.time;
		renderers = GetComponentsInChildren <Renderer> ();
		initialAlphas = new float [renderers.Length];
		for ( int i = 0 ; i < renderers.Length ; i++ ) {
			var renderer = renderers [i];
			initialAlphas [i] = renderer.sharedMaterial.color.a;
		}
	}

	private void Update () {
		if ( !Started || Finished )
			return;

		var timeSinceStart = Time.time - startTime;
		var t = Mathf.Clamp01 ( timeSinceStart / AnimationDuration );
		if ( !Finished && t == 1 )
			Finished = true;

		SetAlphas ( t );
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

	public static bool StartRemoval ( Transform transform ) {
		var remover = transform.gameObject.AddComponent <ObjectRemover> ();
		remover.StartRemoval ();
		return	true;
	}
}
