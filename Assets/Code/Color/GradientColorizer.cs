using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GradientColorizer : Colorizer {
	[SerializeField]
	private List <GradientStop> _gradientStops = new List <GradientStop> ();
	public List <GradientStop> GradientStops {
		get => _gradientStops;
		set => _gradientStops = value;
	}
	private Gradient _gradient;
	public Gradient Gradient => _gradient ?? InitGradient ();

	private Gradient InitGradient () {
		var stops = GradientStops;
		int count = stops.Count;
		var colorKeys = new GradientColorKey [count];
		for ( int i = 0 ; i < count ; i++ ) {
			var stop = stops [i];
			colorKeys [i].time = stop.Position;
		}

		_gradient = new Gradient {
			colorKeys = colorKeys
		};
		return	_gradient;
	}

	public override bool SetColor ( IReadOnlyDictionary <ColorRole, HsvColor> palette, MaterialSubstitutionCache cache ) {
		var stops = GradientStops;
		int count = stops.Count;
		var colorKeys = Gradient.colorKeys;
		for ( int i = 0 ; i < count ; i++ ) {
			var stop = stops [i];
			if ( !palette.TryGetValue ( stop.Role, out var hsvColor ) )
				continue;

			colorKeys [i].color = ( Color ) hsvColor;
		}

		Gradient.colorKeys = colorKeys;
		return	true;
	}

	[Serializable]
	public struct GradientStop {
		public float Position;
		public ColorRole Role;
	}
}
