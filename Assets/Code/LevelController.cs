using UnityEngine;

public class LevelController : MonoBehaviour {
	public BouncingBallCharacter Character { get; private set; }

	private void Start () {
		Character = FindObjectOfType <BouncingBallCharacter> ();
		Character.KillerObstacleHit += Character_KillerObstacleHit;
	}

	private void Character_KillerObstacleHit ( BouncingBallCharacter character, KillerObstacle obstacle ) {
		Time.timeScale = 0;
	}
}
