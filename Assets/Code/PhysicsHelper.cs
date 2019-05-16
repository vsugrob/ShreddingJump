using UnityEngine;

public static class PhysicsHelper {
	public static Vector3 VerticalGravity => Vector3.Project ( Physics.gravity, Vector3.up );
	public static float VerticalGravityMagnitude => Vector3.Dot ( Physics.gravity, Vector3.up );
}
