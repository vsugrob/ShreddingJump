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
	private AudioClip _bounceSound;
	public AudioClip BounceSound => _bounceSound;
	[SerializeField]
	private AudioClip _crushSound;
	public AudioClip CrushSound => _crushSound;
	[SerializeField]
	private AudioClip _deathSound;
	public AudioClip DeathSound => _deathSound;
	[SerializeField]
	private AudioClip _floorCompleteSound;
	public AudioClip FloorCompleteSound => _floorCompleteSound;
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
	public int FloorStreak { get; private set; }
	public bool IsMeteor => FloorStreak >= GameSettings.Singleton.MeteorFloorStreak;
	private CharacterController charController;
	private AudioSource audioSource;
	private Quaternion initialRotationRelativeToCenter;
	private float initialDistFromCenter;
	private float lastJumpTime = float.NegativeInfinity;
	private Quaternion RotationFromCenter {
		get {
			var pos = transform.position;
			var vHorzFromCenter = new Vector3 ( pos.x, 0, pos.z );
			return	Quaternion.LookRotation ( vHorzFromCenter );
		}
	}

	private void Awake () {
		charController = GetComponent <CharacterController> ();
		audioSource = GetComponentInChildren <AudioSource> ();
	}

	private void Start () {
		CaptureInitalValues ();
	}

	private void CaptureInitalValues () {
		CaptureInitialRotation ();
		CaptureInitialDistFromCenter ();
	}

	private void CaptureInitialRotation () {
		initialRotationRelativeToCenter = RotationFromCenter * transform.rotation;
	}

	private void CaptureInitialDistFromCenter () {
		var pos = transform.position;
		var vFromCenterHorz = new Vector3 ( pos.x, 0, pos.z );
		initialDistFromCenter = vFromCenterHorz.magnitude;
	}

	private void FixedUpdate () {
		PerformVerticalMotion ();
		PerformRotationMotion ();
		SetRotation ();
	}

	private void PerformVerticalMotion () {
		VerticalVelocity += LocalVerticalGravityMagnitude * Time.fixedDeltaTime;
		var motion = Vector3.up * VerticalVelocity * Time.fixedDeltaTime;
		charController.Move ( motion );
	}

	private void PerformRotationMotion () {
		const float AngleStepDegEpsilon = 1f / 180 * 0.5f;
		if ( Mathf.Abs ( InputHorizontalRotationDeg ) < AngleStepDegEpsilon )
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
		InputHorizontalRotationDeg = 0;
	}

	private void SetRotation () {
		transform.rotation = RotationFromCenter * initialRotationRelativeToCenter;
	}

	private Vector3 lastContactPoint;
	private Vector3 lastContactNormal;
	private void OnControllerColliderHit ( ControllerColliderHit hit ) {
		// Filter out false positive collision against MeshCollider.
		float skinWidthExtension = 0.1f;
		var otherCollider = hit.collider;
		if ( otherCollider is MeshCollider meshCollider &&
			 !PhysicsHelper.VerifyCharacterControllerVsMeshColliderHit ( charController, hit, skinWidthExtension )
		) {
			return;
		}

		lastContactPoint = hit.point;
		lastContactNormal = hit.normal;
		var gameObject = hit.gameObject;
		if ( IsMeteor ) {
			// Meteor crushes floor it hits.
			if ( CrushSound != null )
				audioSource.PlayOneShot ( CrushSound );
			FloorRoot.TryDismantleFloor ( gameObject );
		} else {
			var obstacle = gameObject.GetComponent <KillerObstacle> ();
			if ( obstacle != null ) {
				if ( DeathSound != null )
					audioSource.PlayOneShot ( DeathSound );

				KillerObstacleHit?.Invoke ( this, obstacle );
				return;
			}
		}

		var nDotUp = Vector3.Dot ( Vector3.up, hit.normal );
		var maxCos = Mathf.Cos ( JumpMaxPlatformHitAngleRad );
		bool isJumpableSurfaceAngle = nDotUp >= maxCos;
		if ( isJumpableSurfaceAngle ) {
			ClearFloorStreak ();
			Jump ();
		}
	}

	private void ClearFloorStreak () {
		FloorStreak = 0;
	}

	private void Jump () {
		var timeSinceLastJump = Time.fixedTime - lastJumpTime;
		if ( timeSinceLastJump < MinJumpInterval )
			return;

		lastJumpTime = Time.fixedTime;
		VerticalVelocity = JumpVelocity;
		if ( BounceSound != null )
			audioSource.PlayOneShot ( BounceSound );
	}

	public void OnFloorComplete ( FloorCompleteTrigger floorCompleteTrigger ) {
		FloorStreak++;
		if ( FloorCompleteSound != null )
			audioSource.PlayOneShot ( FloorCompleteSound );
	}

	private void OnDrawGizmos () {
		Gizmos.color = Color.magenta;
		Gizmos.DrawRay ( lastContactPoint, lastContactNormal );
	}
}
