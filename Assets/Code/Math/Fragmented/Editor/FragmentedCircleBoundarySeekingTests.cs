using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tests {
    public class FragmentedCircleBoundarySeekingTests {
		[Test]
		public void FragmentedCircle_SeekFragmentBoundary_PositiveDir () {
			AssertSeek ( 0, dir : 1, expectedBoundary : null );
			AssertSeek (
				0, dir : 1, expectedBoundary : 45,
				45, 60,
				90, 90,
				180, 225
			);
			AssertSeek (
				270, dir : 1, expectedBoundary : 45,
				45, 60,
				90, 90,
				180, 225
			);
			AssertSeek (
				360, dir : 1, expectedBoundary : 45,
				45, 60,
				90, 90,
				180, 225
			);
		}

		[Test]
		public void FragmentedCircle_SeekFragmentBoundary_NegativeDir () {
			AssertSeek ( 0, dir : -1, expectedBoundary : null );
			AssertSeek (
				270, dir : -1, expectedBoundary : 225,
				45, 60,
				90, 90,
				180, 225
			);
			AssertSeek (
				30, dir : -1, expectedBoundary : 225,
				45, 60,
				90, 90,
				180, 225
			);
			AssertSeek (
				0, dir : -1, expectedBoundary : 225,
				45, 60,
				90, 90,
				180, 225
			);
		}

		[Test]
		public void FragmentedCircle_SeekFragmentBoundary_ZeroDir () {
			AssertSeek (
				0, dir : 0, expectedBoundary : null,
				45, 60,
				90, 90,
				180, 225
			);
		}

		[Test]
		public void FragmentedCircle_SeekFragmentBoundary_OutOfBounds () {
			AssertSeek (
				-10, dir : 1, expectedBoundary : 45,
				45, 60,
				90, 90,
				180, 225
			);
			AssertSeek (
				370, dir : 1, expectedBoundary : 45,
				45, 60,
				90, 90,
				180, 225
			);
			AssertSeek (
				370, dir : -1, expectedBoundary : 225,
				45, 60,
				90, 90,
				180, 225
			);
			AssertSeek (
				-10, dir : -1, expectedBoundary : 225,
				45, 60,
				90, 90,
				180, 225
			);
		}

		private void AssertSeek ( int start, int dir, int? expectedBoundary, params int [] rangeEnds ) {
			Assert.IsTrue ( rangeEnds.Length % 2 == 0 );
			var fc = FragmentedCircle.CreateDegrees <string> ();
			for ( int i = 0 ; i < rangeEnds.Length ; ) {
				fc.Add ( $"e{i}", rangeEnds [i++], rangeEnds [i++] );
			}

			var index = fc.SeekFragmentBoundary ( start, dir, out var actualBoundary );
			if ( !expectedBoundary.HasValue )
				Assert.IsTrue ( index < 0 );
			else {
				Assert.AreEqual ( expectedBoundary, actualBoundary );
				var range = fc [index].Range;
				if ( dir > 0 ) Assert.AreEqual ( expectedBoundary, range.Start );
				else Assert.AreEqual ( expectedBoundary, range.End );
			}
		}
	}
}
