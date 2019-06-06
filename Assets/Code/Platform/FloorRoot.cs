using UnityEngine;

public class FloorRoot : MonoBehaviour {
	public static bool FindRoot ( GameObject floorChildObject, out FloorRoot floorRoot ) {
		floorRoot = floorChildObject.GetComponentInParent <FloorRoot> ();
		return	floorRoot != null;
	}

	public bool FindPlatformContainer ( out PlatformContainer container ) {
		var count = transform.childCount;
		for ( int i = 0 ; i < count ; i++ ) {
			var childTf = transform.GetChild ( i );
			container = childTf.GetComponent <PlatformContainer> ();
			if ( container != null )
				return	true;
		}

		container = null;
		return	false;
	}

	public static bool FindPlatformContainer ( GameObject floorChildObject, out PlatformContainer container ) {
		container = null;
		return	FindRoot ( floorChildObject, out var floorRoot ) && floorRoot.FindPlatformContainer ( out container );
	}

	public static bool TryDismantle ( GameObject floorChildObject ) {
		if ( !FindPlatformContainer ( floorChildObject, out var container ) )
			return	false;

		DismantleChildren ( container.transform );
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
		if ( childTf.GetComponent <NotDismantlable> () != null )
			return	false;

		PhysicsHelper.SetAllChildrenKinematic ( childTf );
		PhysicsHelper.SetAllCollidersEnabled ( childTf, enabled : false );
		ObjectRemover.StartRemoval ( childTf, GameSettings.Singleton.FloorCompletion );
		return	true;
	}
}
