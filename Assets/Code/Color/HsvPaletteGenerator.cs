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
	/// 
	/// </summary>
	/// <param name="key"></param>
	/// <param name="newColor"></param>
	/// <param name="actualMinDistance"></param>
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
	public void Add ( TKey key, out HsvColor newColor, out float bestDistance, float minDistance = 0.25f, int probeIterations = 20 ) {
		minDistance = Mathf.Clamp ( minDistance, 0, MinDistanceMax );
		if ( probeIterations < 1 )
			probeIterations = 1;

		newColor = HsvColor.Random;
		if ( palette.Count == 0 ) {
			bestDistance = minDistance;
			palette.Add ( key, newColor );
			return;
		}

		var bestColor = default ( HsvColor );
		var bestDistanceSq = float.NaN;
		var minDistanceSq = minDistance * minDistance;
		for ( int i = 0 ; i < probeIterations ; i++ ) {
			var newMinDistanceSq = FindMinDistanceToExistingColors ( newColor );
			if ( i == 0 || Mathf.Abs ( newMinDistanceSq - minDistanceSq ) < Mathf.Abs ( bestDistanceSq - minDistanceSq ) ) {
				bestColor = newColor;
				bestDistanceSq = newMinDistanceSq;
			}

			newColor = HsvColor.Random;
		}

		newColor = bestColor;
		bestDistance = Mathf.Sqrt ( bestDistanceSq );
	}

	private float FindMinDistanceToExistingColors ( HsvColor color ) {
		var minDistanceSq = float.MaxValue;
		foreach ( var existingColor in palette.Values ) {
			var distanceSq = HsvColor.DistanceInColorConeSq ( color, existingColor );
			if ( distanceSq < minDistanceSq )
				minDistanceSq = distanceSq;
		}

		return	minDistanceSq;
	}
}
