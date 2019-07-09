using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelColorizer : MonoBehaviour {
	[SerializeField]
	private LevelColorizerSettings _settings;
	public LevelColorizerSettings Settings {
		get => _settings ?? ( _settings = ScriptableObject.CreateInstance <LevelColorizerSettings> () );
		set => _settings = value;
	}

	public void ColorizeLevel ( Transform rootTf, Transform runtimeObjectsContainerTf ) {
		var palette = GeneratePalette ();
		RendererColorizer.SetColors ( rootTf, palette, runtimeObjectsContainerTf );
		var camera = Camera.main;
		if ( camera != null ) {
			camera.backgroundColor = ( Color ) palette [ColorRole.Background];
			camera.clearFlags = CameraClearFlags.SolidColor;
		}
	}

	private IReadOnlyDictionary <ColorRole, HsvColor> GeneratePalette () {
		var generator = new HsvPaletteGenerator <ColorRole> ();
		var roles = ( ColorRole [] ) Enum.GetValues ( typeof ( ColorRole ) );
		HsvColor generateColorFunc () => HsvColor.GenerateRandom (
			Settings.TargetHue, Settings.TargetHueExponent,
			Settings.RandomSaturationExponent, Settings.RandomValueExponent
		);
		generator.AddColor ( ColorRole.Obstacle, ( HsvColor ) Settings.ObstacleColor );
		for ( int i = 0 ; i < roles.Length ; i++ ) {
			var role = roles [i];
			if ( role == ColorRole.Unknown || generator.ContainsColor ( role ) )
				continue;

			generator.AddRandomColor (
				role,
				out var hsvColor, out var bestDistance,
				Settings.MinColorDistance, Settings.ProbeIterations, Settings.UseAllIterations,
				Settings.ValueComponentScale,
				generateColorFunc
			);
		}

		return	generator.Palette;
	}
}
