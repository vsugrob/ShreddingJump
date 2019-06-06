using UnityEngine;

public class FloorCompleteTrigger : MonoBehaviour {
	private void OnTriggerEnter ( Collider other ) {
		var character = ( other as CharacterController )?.GetComponent <BouncingBallCharacter> ();
		if ( character == null )
			return;

		character.OnFloorComplete ( this );
		FloorRoot.TryDismantle ( gameObject );
	}
}
