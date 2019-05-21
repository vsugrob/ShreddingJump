using UnityEngine;

public static class PhysicsHelper {
	public static Vector3 VerticalGravity => Vector3.Project ( Physics.gravity, Vector3.up );
	public static float VerticalGravityMagnitude => Vector3.Dot ( Physics.gravity, Vector3.up );

	public static bool VerifyCharacterControllerVsMeshColliderHit (
		CharacterController charController,
		ControllerColliderHit hit,
		float skinWidthExtension = 0.05f
	) {
		var skin = charController.skinWidth + skinWidthExtension;
		var halfHeight = ( charController.height + skin ) / 2;
		var radius = charController.radius;
		radius += skin;
		if ( halfHeight < radius )
			halfHeight = radius;

		var d = halfHeight - radius;
		var vSphereOffset = Vector3.up * d;
		var pCenter = charController.transform.TransformPoint ( charController.center );
		var capsule = new Capsule (
			pCenter + vSphereOffset,
			pCenter - vSphereOffset,
			radius
		);
		return	capsule.Contains ( hit.point );
	}

	public static void SetAllChildrenKinematic ( Transform transform, bool isKinematic = true ) {
		var bodies = transform.GetComponentsInChildren <Rigidbody> ();
		for ( int i = 0 ; i < bodies.Length ; i++ ) {
			bodies [i].isKinematic = isKinematic;
		}
	}

	public static void SetAllCollidersEnabled ( Transform transform, bool enabled = true ) {
		var colliders = transform.GetComponentsInChildren <Collider> ();
		for ( int i = 0 ; i < colliders.Length ; i++ ) {
			colliders [i].enabled = enabled;
		}
	}
}
