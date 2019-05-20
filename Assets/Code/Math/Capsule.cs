using UnityEngine;

public class Capsule {
	public Vector3 Pos1 { get; set; }
	public Vector3 Pos2 { get; set; }
	public float Radius { get; set; }
	public Vector3 Direction => Pos2-Pos1;
	public Vector3 Center => ( Pos1 + Pos2 ) / 2f;
	public Capsule ( Vector3 p1, Vector3 p2, float radius ) {
		this.Pos1 = p1;
		this.Pos2 = p2;
		this.Radius = radius;
	}

	public Capsule ( Vector3 center, Vector3 direction, float length, float radius ) {
		var d2 = direction.normalized * length / 2f;
		Pos1 = center - d2;
		Pos2 = center + d2;
		Radius = radius;
	}

	public bool Contains ( Vector3 point ) {
		if ( Sphere.Contains ( Pos1, Radius, point ) || Sphere.Contains ( Pos2, Radius, point ) )
			return	true;

		var pDir = point - Pos1;
		float dot = Vector3.Dot ( Direction, pDir );
		float lengthsq = Direction.sqrMagnitude;
		if ( dot < 0f || dot > lengthsq )
			return	false;

		float dsq = pDir.x * pDir.x + pDir.y * pDir.y + pDir.z * pDir.z - dot * dot / lengthsq;
		return	dsq <= Radius * Radius;
	}
}
