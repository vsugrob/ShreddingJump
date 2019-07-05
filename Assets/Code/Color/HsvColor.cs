using UnityEngine;

public struct HsvColor {
	public float H;
	public float S;
	public float V;
	public HsvColor Norm => new HsvColor ( NormalizeHue ( H ), Mathf.Clamp01 ( S ), Mathf.Clamp01 ( V ) );
	/// <summary>
	/// 3d-position in bounds of HSV color cone https://commons.wikimedia.org/wiki/File:HSV_color_solid_cone_chroma_gray.png.
	/// Basic principles of HSV: https://en.wikipedia.org/wiki/HSL_and_HSV#Basic_principle.
	/// </summary>
	public Vector3 PositionInColorCone => GetPositionInColorCone ();
	public static HsvColor Random => new HsvColor ( UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value );

	public HsvColor ( float h, float s, float v ) {
		this.H = h;
		this.S = s;
		this.V = v;
	}

	public static HsvColor FromRgb ( float r, float g, float b ) {
		return	FromRgb ( new Color ( r, g, b ) );
	}

	public static HsvColor FromRgb ( Color rgbColor ) {
		Color.RGBToHSV ( rgbColor, out var h, out var s, out var v );
		return	new HsvColor ( h, s, v );
	}

	public Color ToRgb ( bool hdr = false ) {
		return	Color.HSVToRGB ( H, S, V, hdr );
	}

	/// <summary>
	/// 3d-position in bounds of HSV color cone https://commons.wikimedia.org/wiki/File:HSV_color_solid_cone_chroma_gray.png.
	/// Basic principles of HSV: https://en.wikipedia.org/wiki/HSL_and_HSV#Basic_principle.
	/// </summary>
	/// <param name="valueComponentScale">Scale less than 1 makes dark colors closer to each other.</param>
	public Vector3 GetPositionInColorCone ( float valueComponentScale = 1 ) {
		var a = H * Mathf.PI * 2;
		var v = Mathf.Clamp01 ( V );
		var sv = Mathf.Clamp01 ( S ) * v;
		return	new Vector3 (
			Mathf.Cos ( a ) * sv,
			Mathf.Sin ( a ) * sv,
			( v - 1 ) * valueComponentScale
		);
	}

	public static float DistanceInColorConeSq ( HsvColor c1, HsvColor c2, float valueComponentScale = 1 ) {
		var p1 = c1.GetPositionInColorCone ( valueComponentScale );
		var p2 = c2.GetPositionInColorCone ( valueComponentScale );
		var dx = p2.x - p1.x;
		var dy = p2.y - p1.y;
		var dz = p2.z - p1.z;
		return	dx * dx + dy * dy + dz * dz;
	}

	public static float DistanceInColorCone ( HsvColor c1, HsvColor c2, float valueComponentScale = 1 ) {
		return	Mathf.Sqrt ( DistanceInColorConeSq ( c1, c2, valueComponentScale ) );
	}

	/// <param name="temperatureExponent">
	/// Hue component of generated color is transformed with pow() function around zero (red color) with given exponent.
	/// Higher values produce warmer colors.
	/// </param>
	/// <param name="saturationExponent">
	/// Saturation component of generated color is transformed with pow() function with given exponent.
	/// Values less than 1 results in more saturated colors.
	/// </param>
	/// <param name="valueExponent">
	/// Value component of generated color is transformed with pow() function with given exponent.
	/// Values less than 1 results in lighter colors.
	/// </param>
	public static HsvColor GenerateRandom ( float temperatureExponent, float saturationExponent, float valueExponent ) {
		var r = UnityEngine.Random.value * 2 - 1;
		return	new HsvColor (
			NormalizeHue ( Mathf.Pow ( Mathf.Abs ( r ), temperatureExponent ) * 0.5f * Mathf.Sign ( r ) ),
			Mathf.Pow ( UnityEngine.Random.value, saturationExponent ),
			Mathf.Pow ( UnityEngine.Random.value, valueExponent )
		);
	}

	public static float NormalizeHue ( float hue ) {
		hue %= 1;
		if ( hue < 0 )
			hue = 1 + hue;

		return	hue;
	}

	#region Operators
	public static HsvColor operator * ( HsvColor c, float s ) {
		return	new HsvColor ( c.H * s, c.S * s, c.V * s );
	}

	public static HsvColor operator * ( float s, HsvColor c ) {
		return	new HsvColor ( c.H * s, c.S * s, c.V * s );
	}

	public static HsvColor operator / ( HsvColor c, float s ) {
		return	new HsvColor ( c.H / s, c.S / s, c.V / s );
	}

	public static bool operator == ( HsvColor c1, HsvColor c2 ) {
		return	c1.H == c2.H && c1.S == c2.S && c1.V == c2.V;
	}

	public static bool operator != ( HsvColor c1, HsvColor c2 ) {
		return	c1.H != c2.H || c1.S != c2.S || c1.V != c2.V;
	}
	#endregion Operators

	#region Conversion
	public static explicit operator Color ( HsvColor c ) {
		return	c.ToRgb ();
	}

	public static explicit operator HsvColor ( Color c ) {
		return	FromRgb ( c );
	}

	public static implicit operator Vector3 ( HsvColor c ) {
		return	new Vector3 ( c.H, c.S, c.V );
	}

	public static implicit operator HsvColor ( Vector3 v ) {
		return	new HsvColor ( v.x, v.y, v.z );
	}
	#endregion Conversion

	public override bool Equals ( object obj ) {
		if ( obj is null )
			return	false;

		var other = ( HsvColor ) obj;
		return	H == other.H && S == other.S && V == other.V;
	}

	public override int GetHashCode () {
		var hc = H.GetHashCode ();
		var sc = S.GetHashCode ();
		var vc = V.GetHashCode ();
		return	hc |
				( sc << 8  ) | ( sc >> 24 ) |
				( vc << 16 ) | ( vc >> 16 );
	}

	public override string ToString () {
		return	$"(H: {H:0.###}, S: {S:0.###}, V: {V:0.###})";
	}
}
