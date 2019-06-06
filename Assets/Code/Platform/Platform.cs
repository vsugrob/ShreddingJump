using UnityEngine;

public class Platform : MonoBehaviour {
	[SerializeField, EnumFlags]
	private PlatformKindFlags _kind = PlatformKindFlags.Platform;
	public PlatformKindFlags Kind {
		get => _kind;
		set => _kind = value;
	}
	[SerializeField]
	private float _startAngle = 0;
	public float StartAngle {
		get => _startAngle;
		set => _startAngle = value;
	}
	[SerializeField]
	private float _endAngle = 45;
	public float EndAngle {
		get => _endAngle;
		set => _endAngle = value;
	}
	[SerializeField]
	private float _height = 0.5f;
	public float Height {
		get => _height;
		set => _height = value;
	}
	[SerializeField]
	private bool _dismantleChildren = false;
	public bool DismantleChildren {
		get => _dismantleChildren;
		set => _dismantleChildren = value;
	}
	public float AngleWidth => Mathf.Abs ( EndAngle - StartAngle );
	public float StartAngleWorld {
		get => StartAngle + transform.eulerAngles.y;
		set => transform.eulerAngles = new Vector3 ( 0, value - StartAngle, 0 );
	}
	public float EndAngleWorld {
		get => EndAngle + transform.eulerAngles.y;
		set => transform.eulerAngles = new Vector3 ( 0, value - EndAngle, 0 );
	}
	public float StartAngleLocal {
		get => StartAngle + transform.localEulerAngles.y;
		set => transform.localEulerAngles = new Vector3 ( 0, value - StartAngle, 0 );
	}
	public float EndAngleLocal {
		get => EndAngle + transform.localEulerAngles.y;
		set => transform.localEulerAngles = new Vector3 ( 0, value - EndAngle, 0 );
	}
	public float AngleLocal {
		get => transform.localEulerAngles.y;
		set {
			var euler = transform.localEulerAngles;
			euler.y = value;
			transform.localEulerAngles = euler;
		}
	}

	private void Start () {
		MathHelper.SortMinMax ( ref _startAngle, ref _endAngle );
	}
}
