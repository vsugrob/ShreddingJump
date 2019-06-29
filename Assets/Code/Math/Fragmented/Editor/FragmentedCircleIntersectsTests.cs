using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tests {
    public class FragmentedCircleIntersectsTests {
		[Test]
		public void FragmentedCircle_IntersectsIncludeTouch () {
			AssertIntersects ( 0, 0, includeTouch : true, expectedResult : false );
			AssertIntersects ( 4, 0, includeTouch : true, expectedResult : false );
			AssertIntersects ( -120, 400, includeTouch : true, expectedResult : false );
			AssertIntersects (
				0, 0, includeTouch : true, expectedResult : false,
				10, 10
			);
			AssertIntersects (
				160, 160, includeTouch : true, expectedResult : false,
				1, 150
			);
			AssertIntersects (
				3, 3, includeTouch : true, expectedResult : false,
				5, 60,
				70, 90
			);
			AssertIntersects (
				2, 5, includeTouch : true, expectedResult : true,
				5, 60,
				70, 90
			);
			AssertIntersects (
				5, 5, includeTouch : true, expectedResult : true,
				5, 60,
				70, 90
			);
			AssertIntersects (
				6, 6, includeTouch : true, expectedResult : true,
				5, 60,
				70, 90
			);
			AssertIntersects (
				5, 6, includeTouch : true, expectedResult : true,
				5, 60,
				70, 90
			);
			AssertIntersects (
				5, 60, includeTouch : true, expectedResult : true,
				5, 60,
				70, 90
			);
			AssertIntersects (
				4, 61, includeTouch : true, expectedResult : true,
				5, 60,
				70, 90
			);
			AssertIntersects (
				65, 65, includeTouch : true, expectedResult : false,
				5, 60,
				70, 90
			);
			AssertIntersects (
				0, 360, includeTouch : true, expectedResult : true,
				5, 60,
				70, 90
			);
			AssertIntersects (
				-400, -780, includeTouch : true, expectedResult : true,
				5, 60,
				70, 90
			);
		}

		[Test]
		public void FragmentedCircle_IntersectsExcludeTouch () {
			AssertIntersects ( 90, 90, includeTouch : false, expectedResult : false );
			AssertIntersects ( 30, 270, includeTouch : false, expectedResult : false );
			AssertIntersects ( 600, 1090, includeTouch : false, expectedResult : false );
			AssertIntersects (
				20, 20, includeTouch : false, expectedResult : false,
				3, 3
			);
			AssertIntersects (
				50, 50, includeTouch : false, expectedResult : false,
				80, 250
			);
			AssertIntersects (
				80, 80, includeTouch : false, expectedResult : false,
				80, 250
			);
			AssertIntersects (
				81, 81, includeTouch : false, expectedResult : true,
				80, 250
			);
			AssertIntersects (
				250, 250, includeTouch : false, expectedResult : false,
				80, 250
			);
			AssertIntersects (
				251, 251, includeTouch : false, expectedResult : false,
				80, 250
			);
			AssertIntersects (
				50, 80, includeTouch : false, expectedResult : false,
				80, 250
			);
			AssertIntersects (
				250, 290, includeTouch : false, expectedResult : false,
				80, 250
			);
			AssertIntersects (
				50, 90, includeTouch : false, expectedResult : true,
				80, 250
			);
			AssertIntersects (
				240, 260, includeTouch : false, expectedResult : true,
				80, 250
			);
		}

		private void AssertIntersects ( float arcStart, float arcEnd, bool includeTouch, bool expectedResult, params int [] circleRangeEnds ) {
			for ( int i = -720 ; i <= 720 ; i += 360 ) {
				AssertIntersectsDirect ( arcStart + i, arcEnd + i, includeTouch, expectedResult, circleRangeEnds );
			}
		}

		private void AssertIntersectsDirect ( float arcStart, float arcEnd, bool includeTouch, bool expectedResult, params int [] circleRangeEnds ) {
			Assert.IsTrue ( circleRangeEnds.Length % 2 == 0 );
			var circle = FragmentedCircle.CreateDegrees <string> ();
			for ( int i = 0, j = 0 ; j < circleRangeEnds.Length ; ) {
				circle.Add ( "Element" + i++, circleRangeEnds [j++], circleRangeEnds [j++] );
			}

			Assert.AreEqual ( expectedResult, circle.Intersects ( Range.Create ( arcStart, arcEnd ), includeTouch ) );
		}
	}
}
