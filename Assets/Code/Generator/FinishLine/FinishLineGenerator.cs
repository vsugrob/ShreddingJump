using System;
using UnityEngine;

public class FinishLineGenerator : LevelGenerator {
	[SerializeField]
	private LevelGeneratorSettings _settings;
	public LevelGeneratorSettings Settings {
		get => _settings ?? ( _settings = ScriptableObject.CreateInstance <LevelGeneratorSettings> () );
		set => _settings = value;
	}
	public override LevelGeneratorSettings BasicSettings => Settings;

	protected override void GenerateFloor () {
		var prefab = PrefabDatabase
			.Platforms
			.MatchFlags ( PlatformKindFlags.Platform | PlatformKindFlags.FinishLine )
			.TakeRandomSingleOrDefault ();
		if ( prefab == null )
			return;
		Platform.Instantiate ( prefab, startAngle : 0, platformContainerTf );
		GenerateColumn ();
	}
}
