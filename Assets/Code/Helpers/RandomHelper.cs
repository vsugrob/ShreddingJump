using System;
using UnityEngine;

public static class RandomHelper {
	public static float Range ( float min, float max, float step ) {
		if ( max < min )
			throw new ArgumentException ( $"{nameof ( max )} must be greater or equal to {nameof ( min )}.", nameof ( max ) );
		else if ( step <= float.Epsilon )
			throw new ArgumentException ( $"{nameof ( step )} must be greater than 0.", nameof ( step ) );

		var width = max - min;
		var count = Mathf.CeilToInt ( width / step ) + 1;
		var r = UnityEngine.Random.Range ( 0, count + 1 );
		return	Mathf.Clamp ( r * step + min, min, max );
	}
}
