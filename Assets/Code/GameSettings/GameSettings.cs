﻿using UnityEngine;

public class GameSettings : MonoBehaviour {
	[SerializeField]
	private int _meteorFloorStreak = 3;
	public int MeteorFloorStreak => _meteorFloorStreak;
	[SerializeField]
	private AudioClip _finishLevelSound;
	public AudioClip FinishLevelSound => _finishLevelSound;
	[SerializeField]
	private ObjectRemoverSettings _floorCompletion = new ObjectRemoverSettings ();
	public ObjectRemoverSettings FloorCompletion => _floorCompletion;
	[SerializeField]
	private bool _smootInputAndCamera = true;
	public bool SmootInputAndCamera {
		get => _smootInputAndCamera;
		set => _smootInputAndCamera = value;
	}

	#region Singleton
	private static GameSettings _singleton;
	public static GameSettings Singleton {
		get {
			if ( _singleton == null ) {
				_singleton = FindObjectOfType <GameSettings> ();
				if ( _singleton == null ) {
					var gameObject = new GameObject ( nameof ( GameSettings ) );
					_singleton = gameObject.AddComponent <GameSettings> ();
				}
			}

			return	_singleton;
		}
	}
	#endregion Singleton
}
