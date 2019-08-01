using UnityEngine;

public class CameraAngleParallax : MonoBehaviour {
	[SerializeField]
	private float _unitsPerFullRound = 10;
	public float UnitsPerFullRound {
		get => _unitsPerFullRound;
		set => _unitsPerFullRound = value;
	}
	[SerializeField]
	private float _horizontalMotionSpeedDegrees = 0;
	public float HorizontalMotionSpeedDegrees {
		get => _horizontalMotionSpeedDegrees;
		set => _horizontalMotionSpeedDegrees = value;
	}
	private float initialX;
	private float angleDisplacement;

	private void Start () {
		initialX = transform.localPosition.x;
	}

	private void LateUpdate () {
		var cam = Camera.main;
		if ( cam == null ) return;
		angleDisplacement += HorizontalMotionSpeedDegrees * Time.fixedDeltaTime;
		var angle = MathHelper.ToNormAngleDeg ( cam.transform.eulerAngles.y + angleDisplacement );
		float k = angle / 360;
		var pos = transform.localPosition;
		pos.x = initialX - k * UnitsPerFullRound;
		transform.localPosition = pos;
	}
}
