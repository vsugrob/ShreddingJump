using UnityEngine;

public class CameraAngleParallax : MonoBehaviour {
	[SerializeField]
	private float _unitsPerFullRound = 10;
	public float UnitsPerFullRound {
		get => _unitsPerFullRound;
		set => _unitsPerFullRound = value;
	}
	private float initialX;

	private void Start () {
		initialX = transform.localPosition.x;
	}

	private void LateUpdate () {
		var cam = Camera.main;
		if ( cam == null ) return;
		var angle = MathHelper.ToNormAngleDeg ( cam.transform.eulerAngles.y );
		float t = angle / 360;
		var pos = transform.localPosition;
		pos.x = initialX + t * UnitsPerFullRound;
		transform.localPosition = pos;
	}
}
