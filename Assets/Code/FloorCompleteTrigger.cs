using UnityEngine;

public class FloorCompleteTrigger : MonoBehaviour {
	private void OnTriggerEnter ( Collider other ) {
		var character = ( other as CharacterController )?.GetComponent <BouncingBallCharacter> ();
		if ( character == null )
			return;

		DismantleFloor ();
	}

	private void DismantleFloor () {
		var floorRoot = GetComponentInParent <FloorRoot> ();
		if ( floorRoot == null )
			return;

		var rootTf = floorRoot.transform;
		var count = rootTf.childCount;
		for ( int i = 0 ; i < count ; i++ ) {
			var childTf = rootTf.GetChild ( i );
			PhysicsHelper.SetAllChildrenKinematic ( childTf );
			PhysicsHelper.SetAllCollidersEnabled ( childTf, enabled = false );
		}
	}
}
