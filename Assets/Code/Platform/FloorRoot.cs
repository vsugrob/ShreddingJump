using UnityEngine;

public class FloorRoot : MonoBehaviour {
	public static bool TryGetFloorRoot ( GameObject floorChildObject, out FloorRoot floorRoot ) {
		floorRoot = floorChildObject.GetComponentInParent <FloorRoot> ();
		return	floorRoot != null;
	}

	public static bool TryDismantleFloor ( GameObject floorChildObject ) {
		if ( !TryGetFloorRoot ( floorChildObject, out var floorRoot ) )
			return	false;

		DismantleChildren ( floorRoot.transform );
		return	true;
	}

	private static void DismantleChildren ( Transform transform ) {
		var count = transform.childCount;
		for ( int i = 0 ; i < count ; i++ ) {
			var childTf = transform.GetChild ( i );
			var platform = childTf.GetComponent <Platform> ();
			if ( platform != null && platform.DismantleChildren )
				DismantleChildren ( childTf );
			else
				DismantleObject ( childTf );
		}
	}

	private static bool DismantleObject ( Transform childTf ) {
		if ( childTf.GetComponent <Column> () != null ||
			 childTf.GetComponent <NotDismantlable> () != null
		) {
			return	false;
		}

		PhysicsHelper.SetAllChildrenKinematic ( childTf );
		PhysicsHelper.SetAllCollidersEnabled ( childTf, enabled : false );
		ObjectRemover.StartRemoval ( childTf, GameSettings.Singleton.FloorCompletion );
		return	true;
	}
}
