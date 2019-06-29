using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tests {
    public class FragmentedCircleOrderTests {
		[Test]
		public void FragmentedCircle_AddOrdering_PositiveDirIncreasingNonOverlapping () {
			AssertAddRanges ( new [] { 55, 300 } );
			AssertAddRanges ( new [] { 0, 360 } );
			AssertAddRanges (
				new [] {
					0, 1,
					1, 15,
					15, 340,
				}
			);
			AssertAddRanges (
				new [] {
					89, 350,
					350, 355,
					355, 360,
				}
			);
			AssertAddRanges (
				new [] {
					13, 89,
					200, 279,
					279, 311,
					320, 340,
				}
			);
		}

		[Test]
		public void FragmentedCircle_AddOrdering_NegativeDirIncreasingNonOverlapping () {
			AssertAddRanges (
				new [] { 179, 112 },
				new [] { 112, 179 }
			);
			AssertAddRanges (
				new [] {
					12, 7,
					99, 15,
				},
				new [] {
					7, 12,
					15, 99,
				}
			);
			AssertAddRanges (
				new [] {
					67, 3,
					212, 79,
					355, 270,
				},
				new [] {
					3, 67,
					79, 212,
					270, 355,
				}
			);
		}

		[Test]
		public void FragmentedCircle_AddOrdering_MixedNonOverlapping () {
			AssertAddRanges (
				new [] {
					311, 223,
					180, 195,
					12, 100,
				},
				new [] {
					12, 100,
					180, 195,
					223, 311,
				}
			);
			AssertAddRanges (
				new [] {
					-1346, -1480,	// [0;94], [320;360]
					820, 830,		// [100;110]
					115, 115,
				},
				new [] {
					0, 94,
					100, 110,
					115, 115,
					320, 360,
				}
			);
		}

		[Test]
		public void FragmentedCircle_AddOrdering_Overlapping () {
			AssertAddRanges (
				new [] {
					20, 40,
					15, 60,
					25, 80,
				},
				new [] {
					15, 60,
					20, 40,
					25, 80,
				}
			);
		}

		private void AssertAddRanges ( int [] ranges, int [] expectedRanges = null ) {
			Assert.IsTrue ( ranges.Length % 2 == 0 );
			if ( expectedRanges == null )
				expectedRanges = ranges;

			Assert.IsTrue ( expectedRanges.Length % 2 == 0 );
			var circle = FragmentedCircle.CreateDegrees <string> ();
			const string EPrefix = "Element";
			for ( int i = 0, j = 0 ; j < ranges.Length ; ) {
				circle.Add ( EPrefix + i++, ranges [j++], ranges [j++] );
			}

			for ( int i = 0, j = 0 ; i < circle.Count ; i++ ) {
				var f = circle [i];
				var r = f.Range;
				Assert.AreEqual ( r.Start, expectedRanges [j++] );
				Assert.AreEqual ( r.End, expectedRanges [j++] );
			}
		}
	}
}
