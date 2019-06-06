using UnityEngine;

public class PlatformContainer : MonoBehaviour {
	public float Angle {
		get => transform.localEulerAngles.y;
		set {
			var euler = transform.localEulerAngles;
			euler.y = value;
			transform.localEulerAngles = euler;
		}
	}
}
