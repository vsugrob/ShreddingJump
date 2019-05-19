using System;
using UnityEngine;

public delegate void KillerObstacleHit ( BouncingBallCharacter character, KillerObstacle obstacle );

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
	private float _minJumpInterval = 0.2f;
	public float MinJumpInterval => _minJumpInterval;
	[SerializeField]
	private float _gravityScale = 2.75f;
	public float GravityScale => _gravityScale;
	[SerializeField]
	private float _rotationStepAngleDeg = 18;
	public float RotationStepAngleDeg => _rotationStepAngleDeg;
	[SerializeField]
	private AudioClip _bounceClip;
	public AudioClip BounceClip => _bounceClip;
	[SerializeField]
	private AudioClip _deathClip;
	public AudioClip DeathClip => _deathClip;
	public event KillerObstacleHit KillerObstacleHit;
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
			return	Mathf.Sqrt ( -2 * JumpHeight / LocalVerticalGravityMagnitude );
		}
	}
	public float JumpVelocity => -LocalVerticalGravityMagnitude * JumpAscencionTime;
	private float _verticalVelocity;
	public float VerticalVelocity {
		get { return	_verticalVelocity; }
		set { _verticalVelocity = Mathf.Clamp ( value, -MaxVelocity, MaxVelocity ); }
	}
	public float LocalVerticalGravityMagnitude => PhysicsHelper.VerticalGravityMagnitude * GravityScale;
	public float InputHorizontalRotationDeg { get; set; }
	private const float MaxInputHorizontalRotationDeg = 180;
	private CharacterController charController;
	private AudioSource audioSource;
	private float initialDistFromCenter;
	private float lastJumpTime = float.NegativeInfinity;

	private void Awake () {
		charController = GetComponent <CharacterController> ();
		audioSource = GetComponentInChildren <AudioSource> ();
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
		VerticalVelocity += LocalVerticalGravityMagnitude * Time.fixedDeltaTime;
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
		const float AngleStepEpsilon = ( Mathf.PI / 180 ) * 0.5f;
		if ( Mathf.Abs ( angleStep ) > AngleStepEpsilon ) {
			bool angleStepIsExcessive;
			do {
				angleStepIsExcessive = MathHelper.StepTowards ( ref angleAroundY, targetAngle, angleStep );
				var newPos = new Vector3 (
					Mathf.Sin ( angleAroundY ) * initialDistFromCenter,
					pos.y,
					Mathf.Cos ( angleAroundY ) * initialDistFromCenter
				);
				var motion = newPos - pos;
				charController.Move ( motion );
				pos = newPos;
			} while ( !angleStepIsExcessive );
		}

		InputHorizontalRotationDeg = 0;
	}

	private Vector3 lastContactPoint;
	private Vector3 lastContactNormal;
	private void OnControllerColliderHit ( ControllerColliderHit hit ) {
		lastContactPoint = hit.point;
		lastContactNormal = hit.normal;

		var gameObject = hit.gameObject;
		var obstacle = gameObject.GetComponent <KillerObstacle> ();
		if ( obstacle != null ) {
			if ( DeathClip != null )
				audioSource.PlayOneShot ( DeathClip );

			KillerObstacleHit?.Invoke ( this, obstacle );
			return;
		}

		var nDotUp = Vector3.Dot ( Vector3.up, hit.normal );
		var maxCos = Mathf.Cos ( JumpMaxPlatformHitAngleRad );
		bool isJumpableSurfaceAngle = nDotUp >= maxCos;
		if ( isJumpableSurfaceAngle )
			Jump ();
	}

	private void Jump () {
		var timeSinceLastJump = Time.fixedTime - lastJumpTime;
		if ( timeSinceLastJump < MinJumpInterval )
			return;

		lastJumpTime = Time.fixedTime;
		VerticalVelocity = JumpVelocity;
		if ( BounceClip != null )
			audioSource.PlayOneShot ( BounceClip );
	}

	private void OnDrawGizmos () {
		Gizmos.color = Color.magenta;
		Gizmos.DrawRay ( lastContactPoint, lastContactNormal );
	}
}
