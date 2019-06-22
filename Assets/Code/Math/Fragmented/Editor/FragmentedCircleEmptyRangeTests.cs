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

		private void AssertEmptyRanges ( int [] rangeEnds, int [] expectedEmptyRangeEnds ) {
			Assert.IsTrue ( rangeEnds.Length % 2 == 0 );
			Assert.IsTrue ( expectedEmptyRangeEnds.Length % 2 == 0 );
			var circle = FragmentedCircle.CreateDegrees <string> ();
			for ( int i = 0, j = 0 ; j < rangeEnds.Length ; ) {
				circle.Add ( "Element" + i++, rangeEnds [j++], rangeEnds [j++] );
			}
			var emptyRanges = circle.GetAllEmptyRanges ();
			// Test TryFindEmptyRange().
			var actualEmptyRangeEnds = new List <int> ();
			for ( int i = 0 ; i < 1000 ; i++ ) {
				if ( !circle.TryFindEmptyRange ( out var range ) )
					break;

				int s = ( int ) range.Start, e = ( int ) range.End;
				actualEmptyRangeEnds.Add ( s );
				actualEmptyRangeEnds.Add ( e );
				circle.Add ( "Empty" + i, s, e );
			}

			CollectionAssert.AreEqual ( expectedEmptyRangeEnds, actualEmptyRangeEnds );
			// Test GetAllEmptyRanges().
			actualEmptyRangeEnds.Clear ();
			for ( int i = 0 ; i < emptyRanges.Count ; i++ ) {
				var emptyRange = emptyRanges [i];
				actualEmptyRangeEnds.Add ( ( int ) emptyRange.Start );
				actualEmptyRangeEnds.Add ( ( int ) emptyRange.End );
			}

			CollectionAssert.AreEqual ( expectedEmptyRangeEnds, actualEmptyRangeEnds );
		}
	}
}
