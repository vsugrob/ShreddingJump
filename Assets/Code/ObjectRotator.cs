using UnityEngine;
using UnityEngine.Animations;

public class ObjectRotator : MonoBehaviour {
	[SerializeField]
	private float _angularSpeed = 135;
	public float AngularSpeedDeg => _angularSpeed;
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
	public float AngleDeg {
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

	private void FixedUpdate () {
		var angleStep = AngularSpeedDeg * Time.fixedDeltaTime;
		if ( !MovingForward )
			angleStep = -angleStep;

		var angle = AngleDeg;
		var minAngle = MinAngleDeg;
		var maxAngle = MaxAngleDeg;
		MathHelper.SortMinMax ( ref minAngle, ref maxAngle );
		bool isOnInnerArc = minAngle <= angle && angle <= maxAngle;
		float targetAngle;
		if ( isOnInnerArc )
			targetAngle = angleStep > 0 ? maxAngle : minAngle;
		else
			targetAngle = angleStep > 0 ? minAngle : maxAngle;

		bool metBoundary = MathHelper.StepTowardsAngleDeg ( ref angle, targetAngle, angleStep );
		if ( metBoundary )
			MovingForward = !MovingForward;

		AngleDeg = angle;
	}
}
