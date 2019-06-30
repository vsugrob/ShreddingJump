using System;
using UnityEngine;

public class LevelStartMenu : MonoBehaviour {
	public event Action StartLevel;

	public void OnStartButtonClick () {
		StartLevel?.Invoke ();
	}
}
