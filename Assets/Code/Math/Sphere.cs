using UnityEngine;

public class Sphere {
	public Vector3 Center { get; set; }
	public float Radius { get; set; }

	public Sphere ( Vector3 position, float rad ) {
		this.Center = position;
		this.Radius = rad;
	}

	public bool Contains ( Vector3 point ) {
		return	Contains ( Center, Radius, point );
	}

	public static bool Contains ( Sphere sphere, Vector3 point ) {
		return	Contains ( sphere.Center, sphere.Radius, point );
	}

	public static bool Contains ( Vector3 spherePos, float radius, Vector3 point ) {
		float dist = ( spherePos - point ).sqrMagnitude;
		return	dist <= radius * radius;
	}
}
