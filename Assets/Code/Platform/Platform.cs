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
	public float MinAngle => Mathf.Min ( StartAngle, EndAngle );
	public float MaxAngle => Mathf.Max ( StartAngle, EndAngle );
	public float MinAngleWorld => MinAngle + transform.eulerAngles.y;
	public float MaxAngleWorld => MaxAngle + transform.eulerAngles.y;
}
