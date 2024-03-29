﻿using System;
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
		SimpleColorizer.SetColors ( rootTf, palette, runtimeObjectsContainerTf );
		var camera = Camera.main;
		if ( camera != null )
			SimpleColorizer.SetColors ( camera.transform, palette, runtimeObjectsContainerTf );
	}

	private IReadOnlyDictionary <ColorRole, HsvColor> GeneratePalette () {
		var generator = new HsvPaletteGenerator <ColorRole> ();
		generator.AddColor ( ColorRole.Obstacle, ( HsvColor ) Settings.ObstacleColor );
		AddColor ( ColorRole.Background, generator );
		AddColor ( ColorRole.Column, generator );
		AddColor ( ColorRole.Platform, generator );
		AddColor ( ColorRole.Character, generator );
		AddColor ( ColorRole.Character2, generator );
		AddColor ( ColorRole.Background2, generator );
		AddColor ( ColorRole.Background3, generator );
		AddColor ( ColorRole.Background4, generator );
		AddColor ( ColorRole.Background5, generator );
		AddColor ( ColorRole.Flakes, generator );
		return	generator.Palette;
	}

	private void AddColor ( ColorRole role, HsvPaletteGenerator <ColorRole> generator ) {
		if ( role == ColorRole.Unknown || generator.ContainsColor ( role ) )
			return;

		var cs = Settings.GetColorRandomizerSettings ( role );
		HsvColor generateColorFunc () => HsvColor.GenerateRandom (
			cs.TargetHue, cs.TargetHueExponent,
			cs.RandomSaturationExponent, cs.RandomValueExponent,
			cs.SaturationMin, cs.SaturationMax,
			cs.ValueMin, cs.ValueMax
		);
		generator.AddRandomColor (
			role,
			out var hsvColor, out var bestDistance,
			cs.MinColorDistance, cs.ProbeIterations, cs.UseAllIterations,
			cs.ValueComponentScale,
			generateColorFunc
		);
	}
}
