using UnityEngine;

[RequireComponent ( typeof ( Platform ) )]
public class PlatformRotator : MonoBehaviour {
	[SerializeField]
	private float _angularSpeed = 135;
	public float AngularSpeed {
		get => _angularSpeed;
		set => _angularSpeed = value;
	}
	[SerializeField]
	private bool _useLocalSpace = true;
	public bool UseLocalSpace {
		get => _useLocalSpace;
		set => _useLocalSpace = value;
	}
	[SerializeField]
	private float _startAngle = 0;
	public float StartAngle {
		get => _startAngle;
		set => _startAngle = value;
	}
	[SerializeField]
	private float _endAngle = 135;
	public float EndAngle {
		get => _endAngle;
		set => _endAngle = value;
	}
	[SerializeField]
	private float _minOscillationTime = 0;
	public float MinOscillationTime {
		get => _minOscillationTime;
		set => _minOscillationTime = value;
	}
	[SerializeField]
	private AnimationCurve _motionCurve = new AnimationCurve ( new Keyframe ( 0, 0 ), new Keyframe ( 1, 1 ) );
	public AnimationCurve MotionCurve {
		get => _motionCurve;
		set => _motionCurve = value;
	}
	public float AngleWorld {
		get => transform.eulerAngles.y;
		set {
			var euler = transform.eulerAngles;
			euler.y = value;
			transform.eulerAngles = euler;
		}
	}
	public float AngleLocal {
		get => transform.localEulerAngles.y;
		set {
			var euler = transform.localEulerAngles;
			euler.y = value;
			transform.localEulerAngles = euler;
		}
	}
	public float Angle {
		get => UseLocalSpace ? AngleLocal : AngleWorld;
		set {
			if ( UseLocalSpace )
				AngleLocal = value;
			else
				AngleWorld = value;
		}
	}
	private Platform platform;
	private float startTime = float.NaN;

	private void Start () {
		Init ();
	}

	public void Init () {
		platform = GetComponent <Platform> ();
		if ( platform == null )
			return;

		startTime = Time.fixedTime;
	}

	private void FixedUpdate () {
		if ( platform == null ||
			float.IsNaN ( startTime ) ||
			AngularSpeed <= 0 ||
			MotionCurve == null
		) {
			return;
		}

		var startAngle = StartAngle;
		var endAngle = EndAngle;
		MathHelper.SortMinMax ( ref startAngle, ref endAngle );
		var motionStartAngle = startAngle - platform.StartAngle;
		var motionEndAngle = endAngle - platform.EndAngle;
		if ( motionStartAngle == motionEndAngle )
			return;

		var timeSinceStart = Time.fixedTime - startTime;
		var absDistance = Mathf.Abs ( motionEndAngle - motionStartAngle );
		var travelTime = Mathf.Max ( absDistance / AngularSpeed, MinOscillationTime );
		var turn = timeSinceStart / travelTime;
		var turnIndex = ( int ) turn;
		var t = turn - turnIndex;
		if ( turnIndex % 2 == 1 )
			t = 1 - t;

		var k = MotionCurve.Evaluate ( t );
		Angle = Mathf.Lerp ( motionStartAngle, motionEndAngle, k );
	}

	private void OnDrawGizmos () {
		DrawGizmos ( Color.white, alpha : 0.5f );
	}

	private void OnDrawGizmosSelected () {
		DrawGizmos ( Color.green );
	}

	private void DrawGizmos ( Color color, float alpha = 1 ) {
		color.a = alpha;
		Gizmos.color = color;
		var absDistance = Mathf.Abs ( EndAngle - StartAngle );
		const float SegmentAngularLength = 8;
		const float Radius = 2;
		const float VerticalOffsetBase = 1;
		const float VerticalOffsetPerDegree = 0.5f / 360;
		int numSegments = Mathf.CeilToInt ( absDistance / SegmentAngularLength );
		var angle = StartAngle;
		var y = transform.position.y + VerticalOffsetBase + VerticalOffsetPerDegree * StartAngle;
		Vector3? pPrev = null;
		var angleDelta = SegmentAngularLength;
		if ( EndAngle < StartAngle )
			angleDelta = -angleDelta;

		var parent = transform.parent;
		for ( int i = 0 ; i <= numSegments ; i++, angle += angleDelta ) {
			if ( i == numSegments )
				angle = EndAngle;

			var p = new Vector3 (
				Mathf.Cos ( angle * Mathf.Deg2Rad ) * Radius,
				y,
				-Mathf.Sin ( angle * Mathf.Deg2Rad ) * Radius
			);
			if ( parent != null )
				p = parent.TransformDirection ( p );

			if ( pPrev.HasValue )
				Gizmos.DrawLine ( pPrev.Value, p );

			pPrev = p;
		}
	}
}
