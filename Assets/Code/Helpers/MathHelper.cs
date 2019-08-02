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

	public static float LerpPow ( float a, float b, float t, float tPower ) {
		return	Mathf.Lerp ( a, b, Mathf.Pow ( t, tPower ) );
	}

	public static float ToNormAngle ( float angle ) {
		angle %= Pi2;
		if ( angle < 0 )
			angle += Pi2;

		return	angle;
	}

	public static float ToNormAngle ( float angle, float pi ) {
		angle %= pi * 2;
		if ( angle < 0 )
			angle += pi;

		return	angle;
	}

	public static float ToNormAngleDeg ( float angle ) {
		angle %= 360;
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
		var diff = ArcBetweenDegNorm ( angleNorm, targetAngleNorm, step >= 0 );
		bool stepIsExcessive = diff > 0 ? diff <= step : diff >= step;
		if ( stepIsExcessive )
			angleNorm = targetAngleNorm;
		else
			angleNorm += step;

		return	stepIsExcessive;
	}

	public static float ArcBetweenDeg ( float startAngle, float endAngle, bool positiveStep ) {
		startAngle = ToNormAngleDeg ( startAngle );
		endAngle = ToNormAngleDeg ( endAngle );
		return	ArcBetweenDegNorm ( startAngle, endAngle, positiveStep );
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

	/// <summary>
	/// Calculates shortest rotation from <paramref name="aAngle"/> to <paramref name="bAngle"/>.
	/// </summary>
	/// <param name="aAngle">Initial angle.</param>
	/// <param name="bAngle">Final angle.</param>
	/// <returns>Angle which falls in range [-Pi;Pi].</returns>
	public static float ShortestArc ( float aAngle, float bAngle, float pi ) {
		return	ShortestArcNorm ( ToNormAngle ( aAngle, pi ), ToNormAngle ( bAngle, pi ), pi );
	}

	/// <summary>
	/// Calculates shortest rotation from <paramref name="aNormAngle"/> to <paramref name="bNormAngle"/>.
	/// </summary>
	/// <param name="aNormAngle">Initial angle, must be in range [0;2 * Pi].</param>
	/// <param name="bNormAngle">Final angle, must be in range [0;2 * Pi].</param>
	/// <returns>Angle which falls in range [-Pi;Pi].</returns>
	public static float ShortestArcNorm ( float aNormAngle, float bNormAngle, float pi ) {
		var dAngle = bNormAngle - aNormAngle;
		if ( dAngle > pi )
			dAngle -= pi * 2;
		else if ( dAngle < -pi )
			dAngle += pi * 2;

		return	dAngle;
	}
}
