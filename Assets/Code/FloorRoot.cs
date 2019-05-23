using UnityEngine;

public class FloorRoot : MonoBehaviour {
	public static bool TryGetFloorRoot ( GameObject floorChildObject, out FloorRoot floorRoot ) {
		floorRoot = floorChildObject.GetComponentInParent <FloorRoot> ();
		return	floorRoot != null;
	}

	public static bool TryDismantleFloor ( GameObject floorChildObject ) {
		if ( !TryGetFloorRoot ( floorChildObject, out var floorRoot ) )
			return	false;

		floorRoot.DismantleFloor ();
		return	true;
	}

	public void DismantleFloor () {
		var count = transform.childCount;
		for ( int i = 0 ; i < count ; i++ ) {
			var childTf = transform.GetChild ( i );
			PhysicsHelper.SetAllChildrenKinematic ( childTf );
			PhysicsHelper.SetAllCollidersEnabled ( childTf, enabled = false );
			ObjectRemover.StartRemoval ( childTf, GameSettings.Singleton.FloorCompletion );
		}
	}
}
