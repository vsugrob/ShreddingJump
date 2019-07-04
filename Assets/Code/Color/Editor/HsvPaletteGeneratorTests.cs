using System.Collections.Generic;
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
			const float MinDistance = 0.25f;
			const int ProbeIterations = 40;
			var colors = new List <HsvColor> ();
			for ( int i = 0 ; i < Keys.Length ; i++ ) {
				var key = Keys [i];
				generator.Add ( key, out var hsvColor, out var bestDistance, MinDistance, ProbeIterations );
				colors.Add ( hsvColor );
			}

			using ( var sw = new System.IO.StreamWriter ( @"c:\Temp\palette.html" ) ) {
				for ( int i = 0 ; i < colors.Count ; i++ ) {
					var hsv = colors [i];
					var rgb = hsv.ToRgb ();
					var rgbHex = ColorUtility.ToHtmlStringRGB ( rgb );
					sw.WriteLine ( $"<div style=\"width : 500px; height : 20px; background-color : #{rgbHex};\">{hsv}</div>" );
				}
			}
		}
	}
}
