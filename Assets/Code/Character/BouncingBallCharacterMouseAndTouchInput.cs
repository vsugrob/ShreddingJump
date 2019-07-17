using UnityEngine;

[RequireComponent ( typeof ( BouncingBallCharacter ) )]
public class BouncingBallCharacterMouseAndTouchInput : MonoBehaviour {
	[SerializeField]
	private float _inputPlaneWidth = 6;
	public float InputPlaneWidth => _inputPlaneWidth;
	[SerializeField]
	private float _fullSwipeRotationDeg = 180;
	public float FullSwipeRotationDeg => _fullSwipeRotationDeg;
	private BouncingBallCharacter character;
	private bool isDragging;
	private int draggingFingerId;
	private float prevMousePosOnInputPlane;
	private float inputPlaneHorzExtentOnViewport;

	private void Awake () {
		character = GetComponent <BouncingBallCharacter> ();
	}

	private void Start () {
		Debug.Log ( $"Input, " +
			$" mousePresent: {Input.mousePresent}," +
			$" SimulateTouchWithMouse: {TouchHelper.SimulateTouchWithMouse}" +
			$" touchSupported: {Input.touchSupported}" +
			$" multiTouchEnabled: {Input.multiTouchEnabled}" +
			$" simulateMouseWithTouches: {Input.simulateMouseWithTouches}"
		);
	}

	private void Update () {
		var camera = Camera.main;
		if ( camera == null )
			return;

		var touchCount = TouchHelper.TouchCount;
		if ( touchCount == 0 )
			return;

		CalculateInputPlaneHorzExtent ( camera );
		for ( int i = 0 ; i < touchCount ; i++ ) {
			var touch = TouchHelper.GetTouch ( i );
			var fingerId = touch.fingerId;
			if ( isDragging && touch.fingerId != draggingFingerId )
				continue; // Multitouch is ignored.

			var phase = touch.phase;
			if ( phase == TouchPhase.Began ) {
				isDragging = true;
				draggingFingerId = fingerId;
				prevMousePosOnInputPlane = GetInputPositionOnInputPlane ( camera, touch );
				// Drag is just started, there's no need to process displacement, as well as other touches.
				return;
			} else if ( phase == TouchPhase.Ended || phase == TouchPhase.Canceled )
				isDragging = false;

			if ( isDragging ) {
				var curMousePosOnInputPlane = GetInputPositionOnInputPlane ( camera, touch );
				var delta = curMousePosOnInputPlane - prevMousePosOnInputPlane;
				prevMousePosOnInputPlane = curMousePosOnInputPlane;
				var rotation = FullSwipeRotationDeg * delta;
				character.InputHorizontalRotationDeg += rotation;
			}
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

	private float GetInputPositionOnInputPlane ( Camera camera, Touch touch ) {
		var vpPos = camera.ScreenToViewportPoint ( touch.position );
		return	( vpPos.x - ( 0.5f - inputPlaneHorzExtentOnViewport ) ) / ( 2 * inputPlaneHorzExtentOnViewport );
	}
}
