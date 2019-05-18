using UnityEngine;

public class CharacterCameraController : MonoBehaviour {
	[SerializeField]
	private Transform _targetTransform;
	public Transform TargetTransform {
		get => _targetTransform;
		set => _targetTransform = value;
	}
	[SerializeField]
	private float _targetBottomPos = 0.5f;
	public float TargetBottomPos => _targetBottomPos;
	[SerializeField]
	private float _targetTopPos = 0.85f;
	public float TargetTopPos => _targetTopPos;
	private float initialLookAngleDeg;
	private float initialDistFromCenter;

	private void Start () {
		CaptureInitalValues ();
		// Camera position and orientation are controlled by calculations.
		transform.SetParent ( parent : null, worldPositionStays : true );
	}

	private void CaptureInitalValues () {
		CaptureInitialLookAngle ();
		CaptureInitialDistFromCenter ();
	}

	private void CaptureInitialLookAngle () {
		initialLookAngleDeg = transform.eulerAngles.x;
	}

	private void CaptureInitialDistFromCenter () {
		var pos = transform.position;
		var vFromCenter = new Vector3 ( pos.x, 0, pos.z );
		initialDistFromCenter = vFromCenter.magnitude;
	}

	private void LateUpdate () {
		var targetTf = TargetTransform;
		if ( targetTf == null )
			return;

		// Rotate camera around Y axis along the target.
		var targetPos = targetTf.position;
		var targetAngleAroundY = Mathf.Atan2 ( targetPos.x, targetPos.z );
		var cameraPos = transform.position;
		var cameraCenterPos = new Vector3 ( 0, cameraPos.y, 0 );
		float cameraAngleAroundY = targetAngleAroundY + Mathf.PI;
		transform.SetPositionAndRotation ( cameraCenterPos, Quaternion.Euler ( 0, cameraAngleAroundY * Mathf.Rad2Deg, 0 ) );
		transform.position -= transform.forward * initialDistFromCenter;
		// Set camera vertical rotation.
		transform.Rotate ( initialLookAngleDeg, 0, 0 );
	}
}
