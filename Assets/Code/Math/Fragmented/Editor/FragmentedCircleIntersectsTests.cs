using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tests {
    public class FragmentedCircleIntersectsTests {
		[Test]
		public void FragmentedCircle_Intersects () {
			AssertIntersects ( 0, 0, false );
			AssertIntersects ( 4, 0, false );
			AssertIntersects ( -120, 400, false );
			AssertIntersects (
				0, 0, false,
				10, 10
			);
			AssertIntersects (
				160, 160, false,
				1, 150
			);
			AssertIntersects (
				3, 3, false,
				5, 60,
				70, 90
			);
			AssertIntersects (
				2, 5, true,
				5, 60,
				70, 90
			);
			AssertIntersects (
				5, 5, true,
				5, 60,
				70, 90
			);
			AssertIntersects (
				6, 6, true,
				5, 60,
				70, 90
			);
			AssertIntersects (
				5, 6, true,
				5, 60,
				70, 90
			);
			AssertIntersects (
				5, 60, true,
				5, 60,
				70, 90
			);
			AssertIntersects (
				4, 61, true,
				5, 60,
				70, 90
			);
			AssertIntersects (
				65, 65, false,
				5, 60,
				70, 90
			);
			AssertIntersects (
				0, 360, true,
				5, 60,
				70, 90
			);
			AssertIntersects (
				-400, -780, true,
				5, 60,
				70, 90
			);
		}

		private void AssertIntersects ( float arcStart, float arcEnd, bool expectedResult, params int [] circleRangeEnds ) {
			for ( int i = -720 ; i <= 720 ; i += 360 ) {
				AssertIntersectsDirect ( arcStart + i, arcEnd + i, expectedResult, circleRangeEnds );
			}
		}

		private void AssertIntersectsDirect ( float arcStart, float arcEnd, bool expectedResult, params int [] circleRangeEnds ) {
			Assert.IsTrue ( circleRangeEnds.Length % 2 == 0 );
			var circle = FragmentedCircle.CreateDegrees <string> ();
			for ( int i = 0, j = 0 ; j < circleRangeEnds.Length ; ) {
				circle.Add ( "Element" + i++, circleRangeEnds [j++], circleRangeEnds [j++] );
			}

			Assert.AreEqual ( expectedResult, circle.Intersects ( Range.Create ( arcStart, arcEnd ) ) );
		}
	}
}
