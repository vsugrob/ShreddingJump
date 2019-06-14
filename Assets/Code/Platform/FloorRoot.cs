using UnityEngine;

public class FloorRoot : MonoBehaviour {
	public static FloorRoot Create ( Transform parent, int index, float floorY ) {
		var gameObject = new GameObject ( $"Floor ({index})", typeof ( FloorRoot ) );
		var tf = gameObject.transform;
		tf.SetParent ( parent );
		tf.position = Vector3.up * floorY;
		return	gameObject.GetComponent <FloorRoot> ();
	}

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
		if ( !FindRoot ( floorChildObject, out var floorRoot ) ||
			!floorRoot.FindPlatformContainer ( out var container )
		) {
			return	false;
		}

		DismantleChildren ( container.transform );
		/* Disable floor completion from triggering again,
		 * which is possible when jump height is greater than the distance between floors. */
		var floorCompleteTrigger = floorRoot.GetComponentInChildren <FloorCompleteTrigger> ();
		floorCompleteTrigger?.gameObject.SetActive ( false );
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
