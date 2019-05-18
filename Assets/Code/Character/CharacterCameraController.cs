using System;
using UnityEngine;

[RequireComponent ( typeof ( Camera ) )]
public class CharacterCameraController : MonoBehaviour {
	[SerializeField]
	private Transform _targetTransform;
	public Transform TargetTransform {
		get => _targetTransform;
		set => _targetTransform = value;
	}
	[SerializeField]
	private float _targetTopViewportY = 0.85f;
	public float TargetTopViewportY => _targetTopViewportY;
	[SerializeField]
	private float _targetBottomViewportY = 0.5f;
	public float TargetBottomViewportY => _targetBottomViewportY;
	private new Camera camera;
	private float initialLookAngleDeg;
	private float initialDistFromCenter;

	private void Awake () {
		camera = GetComponent <Camera> ();
	}

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

		var targetPos = targetTf.position;
		RotateAroundYAlongTarget ( targetPos );
		SetVerticalRotation ();
		FitInVerticalBoundaries ( targetPos );
	}

	private void FitInVerticalBoundaries ( Vector3 targetPos ) {
		var bottomRay = camera.ViewportPointToRay ( new Vector3 ( 0.5f, TargetBottomViewportY, 0 ) );
		if ( FitInVerticalBoundary ( targetPos, bottomRay.direction, isBottomBoundary: true ) )
			return;

		var topRay = camera.ViewportPointToRay ( new Vector3 ( 0.5f, TargetTopViewportY, 0 ) );
		FitInVerticalBoundary ( targetPos, topRay.direction, isBottomBoundary: false );
	}

	private bool FitInVerticalBoundary ( Vector3 targetPos, Vector3 nBoundary, bool isBottomBoundary ) {
		var cameraPos = transform.position;
		var vToTarget = targetPos - cameraPos;
		int outOfBoundsSign = isBottomBoundary ? 1 : -1;
		var vCross = Vector3.Cross ( nBoundary, vToTarget );
		bool targetOutOfBounds = Math.Sign ( Vector3.Dot ( transform.right, vCross ) ) == outOfBoundsSign;
		if ( !targetOutOfBounds )
			return	false;

		var vToTargetHorz = new Vector3 ( vToTarget.x, 0, vToTarget.z );
		var vCameraHorz = new Vector3 ( -cameraPos.x, 0, -cameraPos.z );
		var vCameraHorzMag = vCameraHorz.magnitude;
		var k = vToTargetHorz.magnitude / vCameraHorzMag;
		var nCameraHorz = vCameraHorz.normalized;
		var vBoundaryCos = Vector3.Dot ( nBoundary, nCameraHorz );
		var vBoundaryMag = vCameraHorzMag / vBoundaryCos;
		var nToTarget = vToTarget.normalized;
		var vToTargetCos = Vector3.Dot ( nToTarget, nCameraHorz );
		var vAlongTargetMag = vCameraHorzMag / vToTargetCos;
		var vBoundaryY = nBoundary.y * vBoundaryMag;
		var vAlongTargetY = nToTarget.y * vAlongTargetMag;
		var yDiffOnYAxis = vAlongTargetY - vBoundaryY;
		var yDiff = yDiffOnYAxis * k;
		transform.position = new Vector3 ( cameraPos.x, cameraPos.y + yDiff, cameraPos.z );
		return	true;
	}

	private void RotateAroundYAlongTarget ( Vector3 targetPos ) {
		var targetAngleAroundY = Mathf.Atan2 ( targetPos.x, targetPos.z );
		var cameraPos = transform.position;
		var cameraCenterPos = new Vector3 ( 0, cameraPos.y, 0 );
		float cameraAngleAroundY = targetAngleAroundY + Mathf.PI;
		transform.SetPositionAndRotation ( cameraCenterPos, Quaternion.Euler ( 0, cameraAngleAroundY * Mathf.Rad2Deg, 0 ) );
		transform.position -= transform.forward * initialDistFromCenter;
	}

	private void SetVerticalRotation () {
		transform.Rotate ( initialLookAngleDeg, 0, 0 );
	}
}
