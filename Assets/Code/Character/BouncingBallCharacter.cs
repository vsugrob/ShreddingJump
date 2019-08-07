using System;
using UnityEngine;

public delegate void KillerObstacleHit ( BouncingBallCharacter character, KillerObstacle obstacle );
public delegate void FinishLineHit ( BouncingBallCharacter character, FinishLine finishLine );

[SelectionBase]
[RequireComponent ( typeof ( CharacterController ) )]
public class BouncingBallCharacter : MonoBehaviour {
	[SerializeField]
	private float _distanceFromCenter = 2.5f;
	public float DistanceFromCenter {
		get => _distanceFromCenter;
		set => _distanceFromCenter = value;
	}
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
	private float _killerObstacleSafeTouchTime = 0.1f;
	public float KillerObstacleSafeTouchTime => _killerObstacleSafeTouchTime;
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
	public event FinishLineHit FinishLineHit;
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
	private float lastJumpTime = float.NegativeInfinity;
	private Quaternion RotationFromCenter {
		get {
			var pos = transform.position;
			var vHorzFromCenter = new Vector3 ( pos.x, 0, pos.z );
			return	Quaternion.LookRotation ( vHorzFromCenter );
		}
	}
	private float killerObstacleTouchStartTime = float.NaN;

	private void Awake () {
		charController = GetComponent <CharacterController> ();
		audioSource = GetComponentInChildren <AudioSource> ();
	}

	private void Start () {
		CaptureInitalValues ();
	}

	private void CaptureInitalValues () {
		CaptureInitialRotation ();
	}

	private void CaptureInitialRotation () {
		initialRotationRelativeToCenter = RotationFromCenter * transform.rotation;
	}

	public void CalculateDistanceFromCenterInCurrentPosition () {
		var pos = transform.position;
		var vFromCenterHorz = new Vector3 ( pos.x, 0, pos.z );
		DistanceFromCenter = vFromCenterHorz.magnitude;
	}

	public void Restart () {
		VerticalVelocity = 0;
		InputHorizontalRotationDeg = 0;
		FloorStreak = 0;
		lastJumpTime = float.NegativeInfinity;
		killerObstacleTouchStartTime = float.NaN;
		Start ();
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
		if ( Mathf.Abs ( InputHorizontalRotationDeg ) < AngleStepDegEpsilon ) {
			InputHorizontalRotationDeg = 0;
			return;
		}

		InputHorizontalRotationDeg = Mathf.Clamp ( InputHorizontalRotationDeg, -MaxInputHorizontalRotationDeg, MaxInputHorizontalRotationDeg );
		var pos = transform.position;
		var angleAroundY = Mathf.Atan2 ( pos.x, pos.z );
		var inputAngle = InputHorizontalRotationDeg * Mathf.Deg2Rad;
		var targetAngle = angleAroundY + inputAngle;
		var angleStep = Mathf.Sign ( inputAngle ) * RotationStepAngleDeg * Mathf.Deg2Rad;
		bool angleStepIsExcessive = false;
		const int MaxRotationMotionSteps = 100;
		for ( int i = 0 ; i < MaxRotationMotionSteps && !angleStepIsExcessive ; i++ ) {
			angleStepIsExcessive = MathHelper.StepTowards ( ref angleAroundY, targetAngle, angleStep );
			var newPos = new Vector3 (
				Mathf.Sin ( angleAroundY ) * DistanceFromCenter,
				pos.y,
				Mathf.Cos ( angleAroundY ) * DistanceFromCenter
			);
			var motion = newPos - pos;
			charController.Move ( motion );
			pos = newPos;
		}
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
		var finishLine = gameObject.GetComponentInParent <FinishLine> ();
		if ( finishLine != null ) {
			FinishLineHit?.Invoke ( this, finishLine );
			return;
		}

		if ( IsMeteor ) {
			// Meteor crushes floor it hits.
			CrushSound.PlayOneShot ( audioSource );
			FloorRoot.TryDismantle ( gameObject );
		} else {
			var obstacle = gameObject.GetComponentInParent <KillerObstacle> ();
			if ( obstacle != null ) {
				if ( float.IsNaN ( killerObstacleTouchStartTime ) )
					killerObstacleTouchStartTime = Time.fixedTime;

				var timeSinceTouchStart = Time.fixedTime - killerObstacleTouchStartTime;
				if ( timeSinceTouchStart > KillerObstacleSafeTouchTime ) {
					DeathSound.PlayOneShot ( audioSource );
					KillerObstacleHit?.Invoke ( this, obstacle );
				}

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

		killerObstacleTouchStartTime = float.NaN;
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
		BounceSound.PlayOneShot ( audioSource );
	}

	public void OnFloorComplete ( FloorCompleteTrigger floorCompleteTrigger ) {
		FloorStreak++;
		FloorCompleteSound.PlayOneShot ( audioSource );
	}

	private void OnDrawGizmos () {
		Gizmos.color = Color.magenta;
		Gizmos.DrawRay ( lastContactPoint, lastContactNormal );
	}
}
