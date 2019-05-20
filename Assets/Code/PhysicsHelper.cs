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
}
