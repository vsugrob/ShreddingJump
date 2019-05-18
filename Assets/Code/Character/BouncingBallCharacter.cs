﻿using System;
using UnityEngine;

[SelectionBase]
[RequireComponent ( typeof ( CharacterController ) )]
public class BouncingBallCharacter : MonoBehaviour {
	[SerializeField]
	private float _maxVelocity = 20;
	public float MaxVelocity => _maxVelocity;
	[SerializeField]
	private float _jumpMaxPlatformHitAngleDeg = 45;
	public float JumpMaxPlatformHitAngleRad => _jumpMaxPlatformHitAngleDeg * Mathf.Deg2Rad;
	[SerializeField]
	private float _jumpHeight = 2;
	public float JumpHeight => _jumpHeight;
	[SerializeField]
	private float _rotationStepAngleDeg = 18;
	public float RotationStepAngleDeg => _rotationStepAngleDeg;
	public float JumpAscencionTime {
		get {
			/* System of equations:
			 * 0 = v0 + g * t,
			 * h = v0 * t + (g * t^2) / 2
			 * Express v0 in 1st equation:
			 * v0 = -g * t
			 * Substitute v0 into 2nd equation:
			 * h = -g * t^2 + (g * t^2) / 2
			 * h = -2 * g * (t^2) / 2 + (g * t^2) / 2
			 * h = (-2 * g * t^2 + g * t^2) / 2
			 * 2 * h = -g * t^2
			 * -2 * h / g = t^2
			 * Sqrt ( -2 * h / g ) = t
			 * By knowing t and g we can calculate v0 with the first equation v0 = g * t. */
			var g = PhysicsHelper.VerticalGravityMagnitude;
			var t = Mathf.Sqrt ( -2 * JumpHeight / g );
			return	t;
		}
	}
	public float JumpVelocity => -PhysicsHelper.VerticalGravityMagnitude * JumpAscencionTime;
	private float _verticalVelocity;
	public float VerticalVelocity {
		get { return	_verticalVelocity; }
		set { _verticalVelocity = Mathf.Clamp ( value, -MaxVelocity, MaxVelocity ); }
	}
	public float InputHorizontalRotationDeg { get; set; }
	private const float MaxInputHorizontalRotationDeg = 2000;
	private CharacterController charController;
	private float initialDistFromCenter;

	private void Awake () {
		charController = GetComponent <CharacterController> ();
	}

	private void Start () {
		CaptureInitalValues ();
	}

	private void CaptureInitalValues () {
		CaptureInitialAngle ();
		CaptureInitialDistFromCenter ();
	}

	private void CaptureInitialAngle () {
	}

	private void CaptureInitialDistFromCenter () {
		var pos = transform.position;
		var vFromCenterHorz = new Vector3 ( pos.x, 0, pos.z );
		initialDistFromCenter = vFromCenterHorz.magnitude;
	}

	private void FixedUpdate () {
		PerformVerticalMotion ();
		PerformRotationMotion ();
	}

	private void PerformVerticalMotion () {
		VerticalVelocity += PhysicsHelper.VerticalGravityMagnitude * Time.fixedDeltaTime;
		var motion = Vector3.up * VerticalVelocity * Time.fixedDeltaTime;
		charController.Move ( motion );
	}

	private void PerformRotationMotion () {
		if ( InputHorizontalRotationDeg == 0 )
			return;

		InputHorizontalRotationDeg = Mathf.Clamp ( InputHorizontalRotationDeg, -MaxInputHorizontalRotationDeg, MaxInputHorizontalRotationDeg );
		var pos = transform.position;
		var angleAroundY = Mathf.Atan2 ( pos.x, pos.z );
		var inputAngle = InputHorizontalRotationDeg * Mathf.Deg2Rad;
		int inputAngleSign = Math.Sign ( inputAngle );
		var targetAngle = angleAroundY + inputAngle;
		var angleStep = Mathf.Sign ( inputAngle ) * RotationStepAngleDeg * Mathf.Deg2Rad;
		bool angleStepIsExcessive;
		do {
			var angleDiff = targetAngle - angleAroundY;
			if ( inputAngle > 0 )
				angleStepIsExcessive = angleDiff <= angleStep;
			else
				angleStepIsExcessive = angleDiff >= angleStep;

			if ( angleStepIsExcessive )
				angleAroundY = targetAngle;
			else
				angleAroundY += angleStep;

			var newPos = new Vector3 (
				Mathf.Sin ( angleAroundY ) * initialDistFromCenter,
				pos.y,
				Mathf.Cos ( angleAroundY ) * initialDistFromCenter
			);
			var motion = newPos - pos;
			charController.Move ( motion );
			pos = newPos;
		} while ( !angleStepIsExcessive );

		InputHorizontalRotationDeg = 0;
	}

	private Vector3 lastContactPoint;
	private Vector3 lastContactNormal;
	private void OnControllerColliderHit ( ControllerColliderHit hit ) {
		lastContactPoint = hit.point;
		lastContactNormal = hit.normal;
		var nDotUp = Vector3.Dot ( Vector3.up, hit.normal );
		var maxCos = Mathf.Cos ( JumpMaxPlatformHitAngleRad );
		bool isJumpableSurfaceAngle = nDotUp >= maxCos;
		if ( isJumpableSurfaceAngle )
			Jump ();
	}

	private void Jump () {
		VerticalVelocity = JumpVelocity;
	}

	private void OnDrawGizmos () {
		Gizmos.color = Color.magenta;
		Gizmos.DrawRay ( lastContactPoint, lastContactNormal );
	}
}
