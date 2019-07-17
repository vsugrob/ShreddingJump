using UnityEngine;

public static class TouchHelper {
	public static bool SimulateTouchWithMouse { get; set; } = !Input.touchSupported;
	public static int MouseButtonIndex { get; set; } = 0;
	private static bool MouseButtonPressed => Input.GetMouseButtonDown ( MouseButtonIndex );
	private static bool MouseButtonHeldDown => Input.GetMouseButton ( MouseButtonIndex );
	private static bool MouseButtonReleased => Input.GetMouseButtonUp ( MouseButtonIndex );
	private static bool MouseTouchIsActive =>
		SimulateTouchWithMouse && ( MouseButtonPressed || MouseButtonHeldDown || MouseButtonReleased );

	static TouchHelper () {
		Input.simulateMouseWithTouches = false;
	}

	public static int TouchCount {
		get {
			var count = Input.touchCount;
			if ( MouseTouchIsActive )
				count++;

			return	count;
		}
	}

	public static Touch GetTouch ( int index ) {
		if ( SimulateTouchWithMouse && index == 0 ) {
			if ( MouseButtonPressed ) {
				var touch = CreateTouchFromMouse ();
				touch.phase = TouchPhase.Began;
				return	touch;
			} else if ( MouseButtonHeldDown ) {
				var touch = CreateTouchFromMouse ();
				touch.phase = TouchPhase.Moved;	// It's inaccurate, since mouse can be stationary, but still sufficient for this project.
				return	touch;
			} else if ( MouseButtonReleased ) {
				var touch = CreateTouchFromMouse ();
				touch.phase = TouchPhase.Ended;
				return	touch;
			}
		}

		if ( MouseTouchIsActive ) {
			// Touch with index 0 is held by mouse input.
			index--;
		}

		return	Input.GetTouch ( index );
	}

	private static Touch CreateTouchFromMouse () {
		var position = Input.mousePosition;
			return	new Touch () {
				phase = TouchPhase.Began,
				position = position,
				rawPosition = position,
				fingerId = -1,
				type = TouchType.Direct
			};
	}
}
