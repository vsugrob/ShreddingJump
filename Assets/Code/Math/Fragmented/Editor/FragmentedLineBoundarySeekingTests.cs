using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tests {
    public class FragmentedLineBoundarySeekingTests {
		[Test]
		public void FragmentedLine_SeekFragmentBoundary_PositiveDir () {
			AssertSeek ( 0, dir : 1, expectedBoundary : null );
			AssertSeek (
				0, dir : 1, expectedBoundary : 2,
				2, 3,
				4, 4,
				6, 8
			);
			AssertSeek (
				2, dir : 1, expectedBoundary : 2,
				2, 3,
				4, 4,
				6, 8
			);
			AssertSeek (
				3, dir : 1, expectedBoundary : 4,
				2, 3,
				4, 4,
				6, 8
			);
			AssertSeek (
				4, dir : 1, expectedBoundary : 4,
				2, 3,
				4, 4,
				6, 8
			);
			AssertSeek (
				5, dir : 1, expectedBoundary : 6,
				2, 3,
				4, 4,
				6, 8
			);
			AssertSeek (
				7, dir : 1, expectedBoundary : 9,
				2, 3,
				4, 4,
				6, 8,
				9, 11
			);
			AssertSeek (
				10, dir : 1, expectedBoundary : null,
				2, 3,
				4, 4,
				6, 8,
				9, 11
			);
			AssertSeek (
				11, dir : 1, expectedBoundary : null,
				2, 3,
				4, 4,
				6, 8,
				9, 11
			);
			AssertSeek (
				12, dir : 1, expectedBoundary : null,
				2, 3,
				4, 4,
				6, 8,
				9, 11
			);
		}

		[Test]
		public void FragmentedLine_SeekFragmentBoundary_NegativeDir () {
			AssertSeek ( 0, dir : -1, expectedBoundary : null );
			AssertSeek (
				9, dir : -1, expectedBoundary : 8,
				2, 3,
				4, 4,
				6, 8
			);
			AssertSeek (
				8, dir : -1, expectedBoundary : 8,
				2, 3,
				4, 4,
				6, 8
			);
			AssertSeek (
				7, dir : -1, expectedBoundary : 4,
				2, 3,
				4, 4,
				6, 8
			);
			AssertSeek (
				6, dir : -1, expectedBoundary : 4,
				2, 3,
				4, 4,
				6, 8
			);
			AssertSeek (
				5, dir : -1, expectedBoundary : 4,
				2, 3,
				4, 4,
				6, 8
			);
			AssertSeek (
				4, dir : -1, expectedBoundary : 4,
				2, 3,
				4, 4,
				6, 8
			);
			AssertSeek (
				3, dir : -1, expectedBoundary : 3,
				2, 3,
				4, 4,
				6, 8
			);
			AssertSeek (
				2, dir : -1, expectedBoundary : null,
				2, 3,
				4, 4,
				6, 8
			);
			AssertSeek (
				1, dir : -1, expectedBoundary : null,
				2, 3,
				4, 4,
				6, 8
			);
		}

		[Test]
		public void FragmentedLine_SeekFragmentBoundary_ZeroDir () {
			AssertSeek (
				7, dir : 0, expectedBoundary : null,
				2, 3,
				4, 4,
				6, 8,
				9, 11
			);
		}

		[Test]
		public void FragmentedLine_SeekFragmentBoundary_OutOfBounds () {
			Assert.Throws <ArgumentException> ( () =>
				AssertSeek (
					-120, dir : 1, expectedBoundary : null,
					2, 3,
					4, 4,
					6, 8,
					9, 11
				)
			);
			Assert.Throws <ArgumentException> ( () =>
				AssertSeek (
					120, dir : 1, expectedBoundary : null,
					2, 3,
					4, 4,
					6, 8,
					9, 11
				)
			);
		}

		private void AssertSeek ( int start, int dir, int? expectedBoundary, params int [] rangeEnds ) {
			Assert.IsTrue ( rangeEnds.Length % 2 == 0 );
			var fl = new FragmentedLine <string, int> ( -100, 100 );
			for ( int i = 0 ; i < rangeEnds.Length ; ) {
				fl.Add ( $"e{i}", rangeEnds [i++], rangeEnds [i++] );
			}

			var index = fl.SeekFragmentBoundary ( start, dir, out var actualBoundary );
			if ( !expectedBoundary.HasValue )
				Assert.IsTrue ( index < 0 );
			else {
				Assert.AreEqual ( expectedBoundary, actualBoundary );
				var range = fl [index].Range;
				if ( dir > 0 ) Assert.AreEqual ( expectedBoundary, range.Start );
				else Assert.AreEqual ( expectedBoundary, range.End );
			}
		}
	}
}
