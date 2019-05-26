using UnityEngine;

public class Platform : MonoBehaviour {
	[SerializeField]
	private float _startAngle = 0;
	public float StartAngle => _startAngle;
	[SerializeField]
	private float _endAngle = 45;
	public float EndAngle => _endAngle;
	[SerializeField]
	private float _height = 0.5f;
	public float Height => _height;
	[SerializeField]
	private bool _dismantleChildren = false;
	public bool DismantleChildren => _dismantleChildren;
	public float AngleWidth => EndAngle - StartAngle;
	public float StartAngleWorld {
		get => StartAngle + transform.eulerAngles.y;
		set => transform.eulerAngles = new Vector3 ( 0, value - StartAngle, 0 );
	}
	public float EndAngleWorld {
		get => EndAngle + transform.eulerAngles.y;
		set => transform.eulerAngles = new Vector3 ( 0, value - EndAngle, 0 );
	}

	private void Start () {
		MathHelper.SortMinMax ( ref _startAngle, ref _endAngle );
	}
}
