using UnityEngine;

public static class TouchHelper {
	public static bool TreatMouseAsTouch { get; set; } = true;
	public static int MouseButtonIndex { get; set; } = 0;
	private static bool MouseButtonPressed => Input.GetMouseButtonDown ( MouseButtonIndex );
	private static bool MouseButtonHeldDown => Input.GetMouseButton ( MouseButtonIndex );
	private static bool MouseButtonReleased => Input.GetMouseButtonUp ( MouseButtonIndex );
	public static int TouchCount {
		get {
			var count = Input.touchCount;
			if ( TreatMouseAsTouch &&
				 MouseButtonPressed || MouseButtonHeldDown || MouseButtonReleased
			) {
				count++;
			}

			return	count;
		}
	}

	public static Touch GetTouch ( int index ) {
		if ( TreatMouseAsTouch && index == 0 ) {
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
