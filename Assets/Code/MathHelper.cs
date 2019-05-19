using System;
using UnityEngine;

public static class MathHelper {
	public const float Pi2 = Mathf.PI * 2;
	public static void Swap <T> ( ref T v1, ref T v2 ) {
		var tmp = v1;
		v1 = v2;
		v2 = tmp;
	}

	public static bool SortMinMax <T> ( ref T vMin, ref T vMax )
		where T : IComparable <T>
	{
		if ( vMin.CompareTo ( vMax ) > 0 ) {
			Swap ( ref vMin, ref vMax );
			return	true;
		}

		return	false;
	}

	public static float ToNormAngle ( float angle ) {
		angle = angle % Pi2;
		if ( angle < 0 )
			angle += Pi2;

		return	angle;
	}

	public static float ToNormAngleDeg ( float angle ) {
		angle = angle % 360;
		if ( angle < 0 )
			angle += 360;

		return	angle;
	}

	public static bool StepTowards ( ref float value, float target, float step ) {
		var diff = target - value;
		bool stepIsExcessive = diff > 0 ? diff <= step : diff >= step;
		if ( stepIsExcessive )
			value = target;
		else
			value += step;

		return	stepIsExcessive;
	}

	public static bool StepTowardsAngleDeg ( ref float angle, float targetAngle, float step ) {
		angle = ToNormAngleDeg ( angle );
		targetAngle = ToNormAngleDeg ( targetAngle );
		return	StepTowardsAngleDegNorm ( ref angle, targetAngle, step );
	}

	public static bool StepTowardsAngleDegNorm ( ref float angleNorm, float targetAngleNorm, float step ) {
		var diff = targetAngleNorm - angleNorm;
		bool stepIsExcessive = diff > 0 ? diff <= step : diff >= step;
		if ( stepIsExcessive )
			angleNorm = targetAngleNorm;
		else
			angleNorm += step;

		return	stepIsExcessive;
	}

	public static float ArcBetweenDegNorm ( float startAngleNorm, float endAngleNorm, bool positiveStep ) {
		float diff = endAngleNorm - startAngleNorm;
		if ( diff >= 0 ) {
			if ( !positiveStep )
				diff -= 360;
		} else if ( positiveStep )
			diff += 360;

		return	diff;
	}
}
