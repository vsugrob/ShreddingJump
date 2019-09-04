using System;
using UnityEngine;

public class FinishFloorGenerator : FloorGenerator {
	[SerializeField]
	private FloorGeneratorSettings _settings;
	public FloorGeneratorSettings Settings {
		get => _settings ?? ( _settings = ScriptableObject.CreateInstance <FloorGeneratorSettings> () );
		set => _settings = value;
	}
	public override FloorGeneratorSettings BasicSettings => Settings;

	protected override void GenerateFloor () {
		var prefab = PrefabDatabase
			.PredefinedPlatforms
			.MatchFlags ( PlatformKindFlags.Platform | PlatformKindFlags.FinishLine )
			.FindByStyle ( Settings.Style );
		if ( prefab == null )
			return;
		Platform.Instantiate ( prefab, startAngle : 0, platformContainerTf );
		GenerateColumn ();
	}
}
