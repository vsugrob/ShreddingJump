using UnityEngine;

public class PlatformContainer : MonoBehaviour {
	public float AngleLocal {
		get => transform.RoundedLocalEulerY ();
		set {
			var euler = transform.localEulerAngles;
			euler.y = value;
			transform.localEulerAngles = euler;
		}
	}

	public static PlatformContainer Create ( Transform parentTf, float angle ) {
		var gameObject = new GameObject ( $"{nameof ( PlatformContainer )}{angle}", typeof ( PlatformContainer ) );
		var tf = gameObject.transform;
		tf.SetParent ( parentTf, worldPositionStays : false );
		var container = gameObject.GetComponent <PlatformContainer> ();
		container.AngleLocal = angle;
		return	container;
	}
}
