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
	private float prevMousePosOnInputPlane;
	private float inputPlaneHorzExtentOnViewport;

	private void Awake () {
		character = GetComponent <BouncingBallCharacter> ();
	}

	private void Update () {
		var camera = Camera.main;
		if ( camera == null )
			return;

		CalculateInputPlaneHorzExtent ( camera );
		if ( Input.GetMouseButtonDown ( 0 ) ) {
			isDragging = true;
			prevMousePosOnInputPlane = GetMousePositionOnInputPlane ( camera );
			return;
		} else if ( Input.GetMouseButtonUp ( 0 ) )
			isDragging = false;

		if ( isDragging ) {
			var curMousePosOnInputPlane = GetMousePositionOnInputPlane ( camera );
			var delta = curMousePosOnInputPlane - prevMousePosOnInputPlane;
			prevMousePosOnInputPlane = curMousePosOnInputPlane;
			var rotation = FullSwipeRotationDeg * delta;
			character.InputHorizontalRotationDeg += rotation;
		}
	}

	private void OnApplicationFocus ( bool focus ) {
		if ( !focus )
			isDragging = false;
	}

	private void CalculateInputPlaneHorzExtent ( Camera camera ) {
		var cameraTf = camera.transform;
		var nLook = cameraTf.forward;
		var cameraPos = cameraTf.position;
		var centerPos = new Vector3 ( 0, cameraPos.y, 0 );
		var vHorz = centerPos - cameraPos;
		var nHorz = vHorz.normalized;
		var lookCos = Vector3.Dot ( nLook, nHorz );
		var vHorzMagnitude = vHorz.magnitude;
		var vLookMagnitude = vHorzMagnitude / lookCos;
		var vLook = nLook * vLookMagnitude;
		var pLookCenter = cameraPos + vLook;
		var extentPos = pLookCenter + cameraTf.right * InputPlaneWidth * 0.5f;
		var vpPos = camera.WorldToViewportPoint ( extentPos );
		inputPlaneHorzExtentOnViewport = vpPos.x - 0.5f;
	}

	private float GetMousePositionOnInputPlane ( Camera camera ) {
		var vpPos = camera.ScreenToViewportPoint ( Input.mousePosition );
		return	( vpPos.x - ( 0.5f - inputPlaneHorzExtentOnViewport ) ) / ( 2 * inputPlaneHorzExtentOnViewport );
	}
}
