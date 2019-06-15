using System.Collections.Generic;
using NUnit.Framework;

namespace Tests {
    public class FragmentedCircleAddArcTests {
		[Test]
		public void FragmentedCircle_AddArcInBoundsNoSplit () {
			AssertAddArc (
				0, 0, dir : 1,
				0, 0
			);
			AssertAddArc (
				0, 10, dir : 1,
				0, 10
			);
			AssertAddArc (
				10, 0, dir : -1,
				0, 10
			);
			AssertAddArc (
				20, 220, dir : 1,
				20, 220
			);
			AssertAddArc (
				220, 20, dir : -1,
				20, 220
			);
			AssertAddArc (
				0, 360, dir : 1,
				0, 360
			);
			AssertAddArc (
				360, 0, dir : -1,
				0, 360
			);
		}

		[Test]
		public void FragmentedCircle_AddArcInBoundsSplit () {
			AssertAddArc (
				20, 40, dir : -1,
				0, 20,
				40, 360
			);
			AssertAddArc (
				40, 20, dir : 1,
				0, 20,
				40, 360
			);
		}

		[Test]
		public void FragmentedCircle_AddArcOutOfBoundsNoSplit () {
			AssertAddArc (
				360, 400, dir : 1,
				0, 40
			);
			AssertAddArc (
				400, 360, dir : -1,
				0, 40
			);
			AssertAddArc (
				400, 500, dir : 1,
				40, 140
			);
			AssertAddArc (
				500, 400, dir : -1,
				40, 140
			);
			AssertAddArc (
				-360, -200, dir : 1,
				0, 160
			);
			AssertAddArc (
				-200, -360, dir : -1,
				0, 160
			);
		}

		[Test]
		public void FragmentedCircle_AddArcOutOfBoundsSplit () {
			AssertAddArc (
				420, 440, dir : -1,
				0, 60,
				80, 360
			);
			AssertAddArc (
				440, 420, dir : 1,
				0, 60,
				80, 360
			);
			AssertAddArc (
				-440, -420, dir : -1,
				0, 280,
				300, 360
			);
			AssertAddArc (
				-420, -440, dir : 1,
				0, 280,
				300, 360
			);
		}

		private void AssertAddArc ( float arcEnd1, float arcEnd2, int dir, float s1, float e1, float? s2 = null, float? e2 = null ) {
			var circle = FragmentedCircle.CreateDegrees <string> ();
			circle.AddArc ( "r0", arcEnd1, arcEnd2, dir );
			if ( s2.HasValue )
				Assert.AreEqual ( 2, circle.Count );
			else
				Assert.AreEqual ( 1, circle.Count );

			var r = circle [0].Range;
			Assert.AreEqual ( s1, r.Start );
			Assert.AreEqual ( e1, r.End );
			if ( s2.HasValue ) {
				r = circle [1].Range;
				Assert.AreEqual ( s2, r.Start );
				Assert.AreEqual ( e2, r.End );
			}
		}
    }
}
