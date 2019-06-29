using System;
using UnityEngine;

public static class EulerAnglesExt {
	/// <summary>
	/// Angles returned by <see cref="Transform.eulerAngles"/> and <see cref="Transform.localEulerAngles"/> are inaccurate.
	/// This results in comparison faults when, for example, you try to set localEulerAngles.Y to 4 and get 3.99999952 in return.
	/// </summary>
	/// <param name="angle">Input angle to be rounded.</param>
	/// <returns>Angle rounded to 4-digit precision after dot.</returns>
	public static float RoundEulerAngle ( float angle ) {
		return	( float ) Math.Round ( angle, 4 );
	}

	/// <summary>
	/// Angles returned by <see cref="Transform.localEulerAngles"/> are inaccurate.
	/// This results in comparison faults when, for example, you try to set localEulerAngles.Y to 4 and get 3.99999952 in return.
	/// </summary>
	/// <param name="transform">Transform whose angles will be read.</param>
	/// <returns>X, y and z local euler angles rounded to 4-digit precision after dot.</returns>
	public static Vector3 RoundedLocalEulerAngles ( this Transform transform ) {
		var a = transform.localEulerAngles;
		return	new Vector3 (
			RoundEulerAngle ( a.x ),
			RoundEulerAngle ( a.y ),
			RoundEulerAngle ( a.z )
		);
	}

	/// <summary>
	/// Angles returned by <see cref="Transform.eulerAngles"/> are inaccurate.
	/// This results in comparison faults when, for example, you try to set localEulerAngles.Y to 4 and get 3.99999952 in return.
	/// </summary>
	/// <param name="transform">Transform whose angles will be read.</param>
	/// <returns>X, y and z euler angles rounded to 4-digit precision after dot.</returns>
	public static Vector3 RoundedEulerAngles ( this Transform transform ) {
		var a = transform.eulerAngles;
		return	new Vector3 (
			RoundEulerAngle ( a.x ),
			RoundEulerAngle ( a.y ),
			RoundEulerAngle ( a.z )
		);
	}

	#region RoundedLocalEulerXYZ
	/// <summary>
	/// Angles returned by <see cref="Transform.localEulerAngles"/> are inaccurate.
	/// This results in comparison faults when, for example, you try to set localEulerAngles.X to 4 and get 3.99999952 in return.
	/// </summary>
	/// <param name="transform">Transform whose angles will be read.</param>
	/// <returns>X local euler angle rounded to 4-digit precision after dot.</returns>
	public static float RoundedLocalEulerX ( this Transform transform ) {
		return	RoundEulerAngle ( transform.localEulerAngles.x );
	}

	/// <summary>
	/// Angles returned by <see cref="Transform.localEulerAngles"/> are inaccurate.
	/// This results in comparison faults when, for example, you try to set localEulerAngles.Y to 4 and get 3.99999952 in return.
	/// </summary>
	/// <param name="transform">Transform whose angles will be read.</param>
	/// <returns>Y local euler angle rounded to 4-digit precision after dot.</returns>
	public static float RoundedLocalEulerY ( this Transform transform ) {
		return	RoundEulerAngle ( transform.localEulerAngles.y );
	}

	/// <summary>
	/// Angles returned by <see cref="Transform.localEulerAngles"/> are inaccurate.
	/// This results in comparison faults when, for example, you try to set localEulerAngles.Z to 4 and get 3.99999952 in return.
	/// </summary>
	/// <param name="transform">Transform whose angles will be read.</param>
	/// <returns>Z local euler angle rounded to 4-digit precision after dot.</returns>
	public static float RoundedLocalEulerZ ( this Transform transform ) {
		return	RoundEulerAngle ( transform.localEulerAngles.z );
	}
	#endregion RoundedLocalEulerXYZ

	#region RoundedEulerXYZ
	/// <summary>
	/// Angles returned by <see cref="Transform.eulerAngles"/> are inaccurate.
	/// This results in comparison faults when, for example, you try to set eulerAngles.X to 4 and get 3.99999952 in return.
	/// </summary>
	/// <param name="transform">Transform whose angles will be read.</param>
	/// <returns>X euler angle rounded to 4-digit precision after dot.</returns>
	public static float RoundedEulerX ( this Transform transform ) {
		return	RoundEulerAngle ( transform.eulerAngles.x );
	}

	/// <summary>
	/// Angles returned by <see cref="Transform.eulerAngles"/> are inaccurate.
	/// This results in comparison faults when, for example, you try to set eulerAngles.Y to 4 and get 3.99999952 in return.
	/// </summary>
	/// <param name="transform">Transform whose angles will be read.</param>
	/// <returns>Y euler angle rounded to 4-digit precision after dot.</returns>
	public static float RoundedEulerY ( this Transform transform ) {
		return	RoundEulerAngle ( transform.eulerAngles.y );
	}

	/// <summary>
	/// Angles returned by <see cref="Transform.eulerAngles"/> are inaccurate.
	/// This results in comparison faults when, for example, you try to set eulerAngles.Z to 4 and get 3.99999952 in return.
	/// </summary>
	/// <param name="transform">Transform whose angles will be read.</param>
	/// <returns>Z euler angle rounded to 4-digit precision after dot.</returns>
	public static float RoundedEulerZ ( this Transform transform ) {
		return	RoundEulerAngle ( transform.eulerAngles.z );
	}
	#endregion RoundedEulerXYZ
}
