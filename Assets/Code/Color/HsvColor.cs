using UnityEngine;

public struct HsvColor {
	public float H;
	public float S;
	public float V;
	public HsvColor Norm => new HsvColor ( H % 1, Mathf.Clamp01 ( S ), Mathf.Clamp01 ( V ) );

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

	public static float DistanceSq ( HsvColor c1, HsvColor c2 ) {
		// Hue is circular.
		var dh = Mathf.Abs ( c2.H % 1 - c1.H % 1 );
		if ( dh > 0.5f ) dh = 1 - 0.5f;
		var ds = c2.S - c1.S;
		var dv = c2.V - c1.V;
		return	dh * dh + ds * ds + dv * dv;
	}

	public static float Distance ( HsvColor c1, HsvColor c2 ) {
		return	Mathf.Sqrt ( DistanceSq ( c1, c2 ) );
	}

	public static float DistanceSq ( HsvColor c1, HsvColor c2, HsvColor s ) {
		var dh = Mathf.Abs ( ( c2.H - c1.H ) * s.H );
		if ( dh > 0.5f ) dh = 1 - 0.5f;
		var ds = ( c2.S - c1.S ) * s.S;
		var dv = ( c2.V - c1.V ) * s.V;
		return	dh * dh + ds * ds + dv * dv;
	}

	public static float Distance ( HsvColor c1, HsvColor c2, HsvColor s ) {
		return	Mathf.Sqrt ( DistanceSq ( c1, c2, s ) );
	}

	#region operators
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
	#endregion operators

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
