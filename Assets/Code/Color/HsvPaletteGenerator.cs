using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Generates random colors in bounds of HSV color cone https://commons.wikimedia.org/wiki/File:HSV_color_solid_cone_chroma_gray.png.
/// </summary>
/// <typeparam name="TKey">Type of identifier for generated color.</typeparam>
[DebuggerDisplay ( "Count = {Count}" )]
public class HsvPaletteGenerator <TKey> {
	private Dictionary <TKey, HsvColor> palette = new Dictionary <TKey, HsvColor> ();
	public ReadOnlyDictionary <TKey, HsvColor> Palette { get; }
	/// <summary>
	/// Maximum distance between that can be set to generator's <see cref="MinDistance"/> setting.
	/// Calculated as distance between opposite points on top circle of HSV color cone.
	/// </summary>
	public const float MinDistanceMax = 2;

	public HsvPaletteGenerator () {
		this.Palette = new ReadOnlyDictionary <TKey, HsvColor> ( palette );
	}

	/// <summary>
	/// Perform series of color generation operations, pick the best result according to <paramref name="minDistance"/> parameter.
	/// </summary>
	/// <param name="key">Mark of generated color that allows to distinct it from other samples.</param>
	/// <param name="newColor">Resulting color that evaluated in best results according to given parameters.</param>
	/// <param name="bestDistance">Distance from the closest color in the palette.</param>
	/// <param name="minDistance">
	/// <para>
	/// Minimum distance between newly generated and existing colors.
	/// </para>
	/// <para>
	/// This condition might be violated because of several reasons:
	/// A) too many existing colors,
	/// B) <paramref name="probeIterations"/> number is not enough to sample distinct color,
	/// C) random wasn't lucky for us.
	/// </para>
	/// <para>
	/// Parameter value coerced to fall in range from 0 to <see cref="MinDistanceMax"/>.
	/// </para>
	/// </param>
	/// <param name="probeIterations">
	/// <para>
	/// Number of attempts that generator can perform in pursuit of seeking for color
	/// that is distinct enough from existing colors.
	/// </para>
	/// <para>
	/// Generator performs all of the <paramref name="probeIterations"/> iterations and picks best result.
	/// </para>
	/// <para>Coerced to be not less than 1.</para>
	/// </param>
	/// <param name="valueComponentScale">
	/// Scale less than 1 forces generator to think that darker colors are closer to each other.
	/// </param>
	public void AddRandomColor (
		TKey key,
		out HsvColor newColor, out float bestDistance,
		float minDistance = 0.25f, int probeIterations = 20, bool useAllIterations = false,
		float valueComponentScale = 1,
		Func <HsvColor> generateColorFunc = null
	) {
		if ( generateColorFunc == null )
			generateColorFunc = () => HsvColor.Random;

		minDistance = Mathf.Clamp ( minDistance, 0, MinDistanceMax );
		if ( probeIterations < 1 )
			probeIterations = 1;

		newColor = generateColorFunc ();
		if ( palette.Count == 0 ) {
			bestDistance = float.PositiveInfinity;
			palette.Add ( key, newColor );
			return;
		}

		var bestColor = default ( HsvColor );
		var bestMinDistanceSq = float.NaN;
		var minDistanceSq = minDistance * minDistance;
		for ( int i = 0 ; i < probeIterations ; i++ ) {
			var newMinDistanceSq = FindMinDistanceSqToExistingColors ( newColor, valueComponentScale );
			if ( i == 0 || CheckNewMinDistanceIsBetter ( newMinDistanceSq, bestMinDistanceSq, minDistanceSq ) ) {
				bestColor = newColor;
				bestMinDistanceSq = newMinDistanceSq;
				if ( !useAllIterations && bestMinDistanceSq > minDistanceSq )
					break;
			}

			newColor = generateColorFunc ();
		}

		newColor = bestColor;
		bestDistance = Mathf.Sqrt ( bestMinDistanceSq );
		palette.Add ( key, newColor );
	}

	private static bool CheckNewMinDistanceIsBetter ( float newMinDistanceSq, float bestMinDistanceSq, float desiredMinDistanceSq ) {
		var bestVsDesiredDiff = bestMinDistanceSq - desiredMinDistanceSq;
		var newVsDesiredDiff = newMinDistanceSq - desiredMinDistanceSq;
		if ( bestVsDesiredDiff < 0 ) {
			// New distance wins when it's positive or, at least, closer to desired min distance.
			return	newVsDesiredDiff >= 0 || newVsDesiredDiff > bestVsDesiredDiff;
		} else {
			// Best distance was positive. New distance wins only when it's positive and closer to desired min distance.
			return	newVsDesiredDiff >= 0 && newVsDesiredDiff < bestVsDesiredDiff;
		}
	}

	private float FindMinDistanceSqToExistingColors ( HsvColor color, float valueComponentScale ) {
		var minDistanceSq = float.PositiveInfinity;
		foreach ( var existingColor in palette.Values ) {
			var distanceSq = HsvColor.DistanceInColorConeSq ( color, existingColor, valueComponentScale );
			if ( distanceSq < minDistanceSq )
				minDistanceSq = distanceSq;
		}

		return	minDistanceSq;
	}
}
