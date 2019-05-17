using UnityEngine;

[RequireComponent ( typeof ( BouncingBallCharacter ) )]
public class BouncingBallCharacterMouseInput : MonoBehaviour {
	private BouncingBallCharacter character;
	private bool isDragging;
	private Vector3 prevMouseGroundPos;

	private void Awake () {
		character = GetComponent <BouncingBallCharacter> ();
	}

	private void Update () {
		if ( Input.GetMouseButtonDown ( 0 ) ) {
			isDragging = true;
			prevMouseGroundPos = GetMouseInputPlanePosition ();
			return;
		} else if ( Input.GetMouseButtonUp ( 0 ) )
			isDragging = false;

		if ( isDragging ) {
			var curMouseGroundPos = GetMouseInputPlanePosition ();
			var delta = curMouseGroundPos - prevMouseGroundPos;
			character.InputHorizontalMotion += delta.x;
			prevMouseGroundPos = curMouseGroundPos;
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
