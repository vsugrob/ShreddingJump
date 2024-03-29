﻿using System;
using UnityEngine;

[CreateAssetMenu]
public class LevelColorizerSettings : ScriptableObject {
	[Header ( "Palette" )]
	[SerializeField]
	private Color _obstacleColor = new Color32 ( r : 233, g : 47, b : 47, a : 255 );	// #E92F2F
	public Color ObstacleColor {
		get => _obstacleColor;
		set => _obstacleColor = value;
	}
	[SerializeField]
	private ColorRandomizerSettings _column = new ColorRandomizerSettings ();
	public ColorRandomizerSettings Column {
		get => _column;
		set => _column = value;
	}
	[SerializeField]
	private ColorRandomizerSettings _platform = new ColorRandomizerSettings ();
	public ColorRandomizerSettings Platform {
		get => _platform;
		set => _platform = value;
	}
	[SerializeField]
	private ColorRandomizerSettings _background = new ColorRandomizerSettings ();
	public ColorRandomizerSettings Background {
		get => _background;
		set => _background = value;
	}
	[SerializeField]
	private ColorRandomizerSettings _background2 = new ColorRandomizerSettings ();
	public ColorRandomizerSettings Background2 {
		get => _background2;
		set => _background2 = value;
	}
	[SerializeField]
	private ColorRandomizerSettings _background3 = new ColorRandomizerSettings ();
	public ColorRandomizerSettings Background3 {
		get => _background3;
		set => _background3 = value;
	}
	[SerializeField]
	private ColorRandomizerSettings _background4 = new ColorRandomizerSettings ();
	public ColorRandomizerSettings Background4 {
		get => _background4;
		set => _background4 = value;
	}
	[SerializeField]
	private ColorRandomizerSettings _background5 = new ColorRandomizerSettings ();
	public ColorRandomizerSettings Background5 {
		get => _background5;
		set => _background5 = value;
	}
	[SerializeField]
	private ColorRandomizerSettings _flakes = new ColorRandomizerSettings ();
	public ColorRandomizerSettings Flakes {
		get => _flakes;
		set => _flakes = value;
	}
	[SerializeField]
	private ColorRandomizerSettings _character = new ColorRandomizerSettings ();
	public ColorRandomizerSettings Character {
		get => _character;
		set => _character = value;
	}
	[SerializeField]
	private ColorRandomizerSettings _character2 = new ColorRandomizerSettings ();
	public ColorRandomizerSettings Character2 {
		get => _character2;
		set => _character2 = value;
	}

	public ColorRandomizerSettings GetColorRandomizerSettings ( ColorRole role ) {
		switch ( role ) {
		case ColorRole.Column: return	Column;
		case ColorRole.Platform: return	Platform;
		case ColorRole.Background: return	Background;
		case ColorRole.Character: return	Character;
		case ColorRole.Character2: return	Character2;
		case ColorRole.Background2: return	Background2;
		case ColorRole.Background3: return	Background3;
		case ColorRole.Background4: return	Background4;
		case ColorRole.Background5: return	Background5;
		case ColorRole.Flakes: return	Flakes;
		default: throw new ArgumentException ( $"Unknown color role {role}.", nameof ( role ) );
		}
	}
}
