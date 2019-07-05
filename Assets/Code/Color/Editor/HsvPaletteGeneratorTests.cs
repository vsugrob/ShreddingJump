using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Tests {
	public class HsvPaletteGeneratorTests {
		private HsvPaletteGenerator <string> generator;
		private const string Obstacle = nameof ( Obstacle );
		private const string Column = nameof ( Column );
		private const string Platform = nameof ( Platform );
		private const string Background = nameof ( Background );
		private const string Character = nameof ( Character );
		private readonly string [] Keys = new [] { Obstacle, Column, Platform, Background, Character };

		[SetUp]
		public void Init () {
			generator = new HsvPaletteGenerator <string> ();
		}

		[Test]
		public void HsvPaletteGenerator_Add () {
			const float MinDistance = 0.4f;
			const int ProbeIterations = 40;
			const float ValueComponentScale = 0.5f;
			const float RandomSaturationExponent = 0.5f;
			const float RandomValueExponent = 0.5f;
			HsvColor generateColorFunc () => HsvColor.GenerateRandom ( RandomSaturationExponent, RandomValueExponent );
			generator.AddColor ( Obstacle, HsvColors.Red );
			for ( int i = 0 ; i < Keys.Length ; i++ ) {
				var key = Keys [i];
				if ( generator.ContainsColor ( key ) )
					continue;

				generator.AddRandomColor (
					key,
					out var hsvColor, out var bestDistance,
					MinDistance, ProbeIterations, useAllIterations : false,
					ValueComponentScale,
					generateColorFunc
				);
			}

			var colors = generator.Palette.Values.ToArray ();
			using ( var sw = new System.IO.StreamWriter ( @"c:\Temp\palette.html" ) ) {
				sw.WriteLine ( "<table>" );
				for ( int y = 0 ; y < colors.Length ; y++ ) {
					var hsv1 = colors [y];
					var rgb1Hex = ColorUtility.ToHtmlStringRGB ( hsv1.ToRgb () );
					// Write header row.
					if ( y == 0 ) {
						sw.Write ( "<tr><th>&nbsp;</th>" );
						for ( int x = 0 ; x < colors.Length ; x++ ) {
							var hsv2 = colors [x];
							var rgb2Hex = ColorUtility.ToHtmlStringRGB ( hsv2.ToRgb () );
							sw.Write ( $"<th style=\"background-color : #{rgb2Hex};\">#{rgb2Hex}</th>" );
						}

						sw.WriteLine ( "</tr>" );
					}

					sw.WriteLine ( "<tr>" );
					for ( int x = 0 ; x < colors.Length ; x++ ) {
						var hsv2 = colors [x];
						// Write header cell.
						if ( x == 0 )
							sw.Write ( $"<th style=\"background-color : #{rgb1Hex};\">#{rgb1Hex}</th>" );

						var d = HsvColor.DistanceInColorCone ( hsv1, hsv2, ValueComponentScale );
						string style = d < MinDistance && x != y ? "background-color : red;" : "";
						sw.Write ( $"<td style=\"{style}\">{d:0.##}</td>" );
					}

					sw.WriteLine ( "</tr>" );
				}

				sw.WriteLine ( "</table>" );
			}
		}
	}
}
