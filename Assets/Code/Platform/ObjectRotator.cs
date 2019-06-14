using UnityEngine;
using UnityEngine.Animations;

public class ObjectRotator : MonoBehaviour {
	[SerializeField]
	private float _angularSpeed = 135;
	public float AngularSpeedDeg => _angularSpeed;
	[SerializeField]
	private bool _enableBoundaries = true;
	public bool EnableBoundaries => _enableBoundaries;
	[SerializeField]
	private bool _useLocalSpace = true;
	public bool UseLocalSpace => _useLocalSpace;
	[SerializeField]
	private float _minAngle = 0;
	public float MinAngleDeg => _minAngle;
	[SerializeField]
	private float _maxAngle = 135;
	public float MaxAngleDeg => _maxAngle;
	[SerializeField]
	private Axis _axis = Axis.Y;
	public Axis Axis => _axis;
	[SerializeField]
	private bool _movingForward = true;
	public bool MovingForward {
		get => _movingForward;
		set => _movingForward = value;
	}
	public float AngleWorld {
		get {
			var euler = transform.eulerAngles;
			switch ( Axis ) {
			case Axis.X: return	euler.x;
			case Axis.Y: return	euler.y;
			case Axis.Z: return	euler.z;
			default: return	0;
			}
		}
		set {
			var euler = transform.eulerAngles;
			switch ( Axis ) {
			case Axis.X: euler.x = value; break;
			case Axis.Y: euler.y = value; break;
			case Axis.Z: euler.z = value; break;
			}

			transform.eulerAngles = euler;
		}
	}
	public float AngleLocal {
		get {
			var euler = transform.localEulerAngles;
			switch ( Axis ) {
			case Axis.X: return	euler.x;
			case Axis.Y: return	euler.y;
			case Axis.Z: return	euler.z;
			default: return	0;
			}
		}
		set {
			var euler = transform.localEulerAngles;
			switch ( Axis ) {
			case Axis.X: euler.x = value; break;
			case Axis.Y: euler.y = value; break;
			case Axis.Z: euler.z = value; break;
			}

			transform.localEulerAngles = euler;
		}
	}

	private void FixedUpdate () {
		var angleStep = AngularSpeedDeg * Time.fixedDeltaTime;
		if ( !MovingForward )
			angleStep = -angleStep;

		var angle = UseLocalSpace ? AngleLocal : AngleWorld;
		if ( EnableBoundaries ) {
			var positiveStep = angleStep >= 0;
			angle = MathHelper.ToNormAngleDeg ( angle );
			var minAngle = MathHelper.ToNormAngleDeg ( MinAngleDeg );
			var maxAngle = MathHelper.ToNormAngleDeg ( MaxAngleDeg );
			float arcToMin = MathHelper.ArcBetweenDegNorm ( angle, minAngle, positiveStep );
			float arcToMax = MathHelper.ArcBetweenDegNorm ( angle, maxAngle, positiveStep );
			float targetAngle;
			if ( positiveStep )
				targetAngle = arcToMin < arcToMax ? minAngle : maxAngle;
			else
				targetAngle = arcToMin < arcToMax ? maxAngle : minAngle;

			var metBoundary = MathHelper.StepTowardsAngleDegNorm ( ref angle, targetAngle, angleStep );
			if ( metBoundary )
				MovingForward = !MovingForward;
		} else
			angle += angleStep;

		if ( UseLocalSpace )
			AngleLocal = angle;
		else
			AngleWorld = angle;
	}
}
