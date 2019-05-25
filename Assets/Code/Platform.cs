using UnityEngine;

public class Platform : MonoBehaviour {
	[SerializeField]
	private float _startAngle = 0;
	public float StartAngle => _startAngle;
	[SerializeField]
	private float _endAngle = 45;
	public float EndAngle => _endAngle;
	[SerializeField]
	private bool _dismantleChildren = false;
	public bool DismantleChildren => _dismantleChildren;
}
