using System.Collections.Generic;
using NUnit.Framework;

namespace Tests {
    public class FragmentedCircleEmptyRangeTests {
		[Test]
		public void FragmentedCircle_EmptyRanges () {
			AssertEmptyRanges (
				new int [0],
				new [] { 0, 360 }
			);
			AssertEmptyRanges (
				new [] {
					200, 215,
					290, 295,
				},
				new [] {
					0, 200,
					215, 290,
					295, 360,
				}
			);
			AssertEmptyRanges (
				new [] {
					-1346, -1480,	// [0;94], [320;360]
					820, 830,		// [100;110]
					115, 116,
				},
				new [] {
					94, 100,
					110, 115,
					116, 320,
				}
			);
		}

		private void AssertEmptyRanges ( int [] ranges, int [] expectedEmptyRanges ) {
			Assert.IsTrue ( ranges.Length % 2 == 0 );
			Assert.IsTrue ( expectedEmptyRanges.Length % 2 == 0 );
			var circle = FragmentedCircle.CreateDegrees <string> ();
			for ( int i = 0, j = 0 ; j < ranges.Length ; ) {
				circle.Add ( "Element" + i++, ranges [j++], ranges [j++] );
			}

			var actualEmptyRanges = new List <int> ();
			for ( int i = 0 ; i < 1000 ; i++ ) {
				if ( !circle.TryFindEmptyRange ( out var range ) )
					break;

				int s = ( int ) range.Start, e = ( int ) range.End;
				actualEmptyRanges.Add ( s );
				actualEmptyRanges.Add ( e );
				circle.Add ( "Empty" + i, s, e );
			}

			CollectionAssert.AreEqual ( expectedEmptyRanges, actualEmptyRanges );
		}
	}
}
