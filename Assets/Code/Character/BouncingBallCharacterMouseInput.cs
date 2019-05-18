using UnityEngine;

[RequireComponent ( typeof ( BouncingBallCharacter ) )]
public class BouncingBallCharacterMouseInput : MonoBehaviour {
	[SerializeField]
	private float _inputPlaneWidth = 6;
	public float InputPlaneWidth => _inputPlaneWidth;
	[SerializeField]
	private float _fullSwipeRotationDeg = 180;
	public float FullSwipeRotationDeg => _fullSwipeRotationDeg;
	private BouncingBallCharacter character;
	private bool isDragging;
	private Vector3 prevMouseInputPlanePos;

	private void Awake () {
		character = GetComponent <BouncingBallCharacter> ();
	}

	private void Update () {
		if ( Input.GetMouseButtonDown ( 0 ) ) {
			isDragging = true;
			prevMouseInputPlanePos = GetMouseInputPlanePosition ();
			return;
		} else if ( Input.GetMouseButtonUp ( 0 ) )
			isDragging = false;

		if ( isDragging ) {
			var curMouseInputPlanePos = GetMouseInputPlanePosition ();
			var delta = curMouseInputPlanePos - prevMouseInputPlanePos;
			var rotation = FullSwipeRotationDeg * delta.x / InputPlaneWidth;
			character.InputHorizontalRotationDeg -= rotation;
			prevMouseInputPlanePos = curMouseInputPlanePos;
		}
	}

	private void OnApplicationFocus ( bool focus ) {
		if ( !focus )
			isDragging = false;
	}

	private Vector3 GetMouseInputPlanePosition () {
		var camera = Camera.main;
		var ray = camera.ScreenPointToRay ( Input.mousePosition );
		var cameraTf = camera.transform;
		var nLook = cameraTf.forward.normalized;
		var cameraPos = cameraTf.position;
		var centerPos = new Vector3 ( 0, cameraPos.y, 0 );
		var vHorz = centerPos - cameraPos;
		var nHorz = vHorz.normalized;
		var lookCos = Vector3.Dot ( nLook, nHorz );
		var vHorzMagnitude = vHorz.magnitude;
		var vLookMagnitude = vHorzMagnitude / lookCos;
		var vLook = nLook * vLookMagnitude;
		var pLookCenter = cameraPos + vLook;
		var inputPlane = new Plane ( -nLook, 0 );
		inputPlane.Translate ( -pLookCenter );
		inputPlane.Raycast ( ray, out var rayDist );
		Debug.DrawRay ( ray.origin, ray.direction * rayDist, Color.yellow );
		return	ray.GetPoint ( rayDist );
	}
}
