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
		generator.AddColor ( ColorRole.Obstacle, ( HsvColor ) Settings.ObstacleColor );
		AddColor ( ColorRole.Background, generator );
		AddColor ( ColorRole.Column, generator );
		AddColor ( ColorRole.Platform, generator );
		AddColor ( ColorRole.Character, generator );
		return	generator.Palette;
	}

	private void AddColor ( ColorRole role, HsvPaletteGenerator <ColorRole> generator ) {
		if ( role == ColorRole.Unknown || generator.ContainsColor ( role ) )
			return;

		var colorSettings = Settings.GetColorRandomizerSettings ( role );
		HsvColor generateColorFunc () => HsvColor.GenerateRandom (
			colorSettings.TargetHue, colorSettings.TargetHueExponent,
			colorSettings.RandomSaturationExponent, colorSettings.RandomValueExponent
		);
		generator.AddRandomColor (
			role,
			out var hsvColor, out var bestDistance,
			colorSettings.MinColorDistance, colorSettings.ProbeIterations, colorSettings.UseAllIterations,
			colorSettings.ValueComponentScale,
			generateColorFunc
		);
	}
}
