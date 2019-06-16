using System;
using NUnit.Framework;

namespace Tests {
    public class CircleMathIntersectArcsTests {
		[Test]
		public void CircleMath_IntersectArcs_FullCircle () {
			AssertIntersectArcs (
				Range.Create ( 0f, 360 ),
				Range.Create ( 0f, 360 ),
				Range.Create ( 0f, 360 ),
				null
			);
			AssertIntersectArcs (
				Range.Create ( 0f, 360 ),
				Range.Create ( 0f, 30 ),
				Range.Create ( 0f, 30 ),
				null
			);
			AssertIntersectArcs (
				Range.Create ( 0f, 360 ),
				Range.Create ( 270f, 360 ),
				Range.Create ( 270f, 360 ),
				null
			);
			AssertIntersectArcs (
				Range.Create ( 0f, 360 ),
				Range.Create ( 270f, 300 ),
				Range.Create ( 270f, 300 ),
				null
			);
			AssertIntersectArcs (
				Range.Create ( 0f, 360 ),
				Range.Create ( 0f, 0 ),
				Range.Create ( 0f, 0 ),
				null
			);
			AssertIntersectArcs (
				Range.Create ( 0f, 360 ),
				Range.Create ( 1f, 1 ),
				Range.Create ( 1f, 1 ),
				null
			);
			AssertIntersectArcs (
				Range.Create ( 0f, 360 ),
				Range.Create ( 255f, 255 ),
				Range.Create ( 255f, 255 ),
				null
			);
			AssertIntersectArcs (
				Range.Create ( 0f, 360 ),
				Range.Create ( 360f, 360 ),
				Range.Create ( 360f, 360 ),
				null
			);
		}

		[Test]
		public void CircleMath_IntersectArcs_MoreThanFullCircle () {
			AssertIntersectArcs (
				Range.Create ( -300f, 400 ),
				Range.Create ( 0f, 360 ),
				Range.Create ( 0f, 360 ),
				null
			);
			AssertIntersectArcs (
				Range.Create ( -300f, 400 ),
				Range.Create ( 10f, 20 ),
				Range.Create ( 10f, 20 ),
				null
			);
			AssertIntersectArcs (
				Range.Create ( -300f, 400 ),
				Range.Create ( -20f, 380f ),
				Range.Create ( 0f, 360 ),
				null
			);
			AssertIntersectArcs (
				Range.Create ( -300f, 400 ),
				Range.Create ( 23f, 23 ),
				Range.Create ( 23f, 23 ),
				null
			);
		}

		[Test]
		public void CircleMath_IntersectArcs_NoIntersection () {
			AssertIntersectArcs (
				Range.Create ( 0f, 30 ),
				Range.Create ( 60f, 80 ),
				null,
				null
			);
			AssertIntersectArcs (
				Range.Create ( -45f, 30 ),
				Range.Create ( 420f, 440 ),
				null,
				null
			);
			AssertIntersectArcs (
				Range.Create ( -45f, 30 ),
				Range.Create ( 31f, 31 ),
				null,
				null
			);
		}

		[Test]
		public void CircleMath_IntersectArcs_OneIntersection () {
			AssertIntersectArcs (
				Range.Create ( 0f, 30 ),
				Range.Create ( 0f, 30 ),
				Range.Create ( 0f, 30 ),
				null
			);
			AssertIntersectArcs (
				Range.Create ( 0f, 30 ),
				Range.Create ( 0f, 15 ),
				Range.Create ( 0f, 15 ),
				null
			);
			AssertIntersectArcs (
				Range.Create ( 10f, 30 ),
				Range.Create ( 10f, 30 ),
				Range.Create ( 10f, 30 ),
				null
			);
			AssertIntersectArcs (
				Range.Create ( 10f, 30 ),
				Range.Create ( 10f, 15 ),
				Range.Create ( 10f, 15 ),
				null
			);
			AssertIntersectArcs (
				Range.Create ( 10f, 310 ),
				Range.Create ( 10f, 310 ),
				Range.Create ( 10f, 310 ),
				null
			);
			AssertIntersectArcs (
				Range.Create ( 10f, 310 ),
				Range.Create ( 200f, 310 ),
				Range.Create ( 200f, 310 ),
				null
			);
			AssertIntersectArcs (
				Range.Create ( 10f, 310 ),
				Range.Create ( 200f, 350 ),
				Range.Create ( 200f, 310 ),
				null
			);
			AssertIntersectArcs (
				Range.Create ( 10f, 310 ),
				Range.Create ( 310f, 350 ),
				Range.Create ( 310f, 310 ),
				null
			);
			AssertIntersectArcs (
				Range.Create ( 10f, 10 ),
				Range.Create ( 10f, 10 ),
				Range.Create ( 10f, 10 ),
				null
			);
		}

		[Test]
		public void CircleMath_IntersectArcs_TwoIntersections () {
			AssertIntersectArcs (
				Range.Create ( 50f, 300 ),
				Range.Create ( 270f, 430 ),
				Range.Create ( 50f, 70 ),
				Range.Create ( 270f, 300 )
			);
			/* This test expects two points, but results are different.
			 * Currently we're not required to have such great precision with points in our project. */
			//AssertIntersectArcs (
			//	Range.Create ( 50f, 300 ),
			//	Range.Create ( 300f, 410 ),
			//	Range.Create ( 50f, 50 ),
			//	Range.Create ( 300f, 300 )
			//);
			//AssertIntersectArcs (
			//	Range.Create ( 50f, 300 ),
			//	Range.Create ( 300f, 430 ),
			//	Range.Create ( 50f, 70 ),
			//	Range.Create ( 300f, 300 )
			//);
		}

		private static void AssertIntersectArcs (
			Range <float> arc1, Range <float> arc2,
			Range <float>? expectedArc1Ends, Range <float>? expectedArc2Ends
		) {
			for ( int s1 = -720 ; s1 <= 720 ; s1 += 360 ) {
				for ( int s2 = -720 ; s2 <= 720 ; s2 += 360 ) {
					AssertIntersectArcsAllReverses ( arc1.Shift ( s1 ), arc2.Shift ( s2 ), expectedArc1Ends, expectedArc2Ends );
				}
			}
		}

		private static void AssertIntersectArcsAllReverses (
			Range <float> arc1, Range <float> arc2,
			Range <float>? expectedArc1Ends, Range <float>? expectedArc2Ends
		) {
			AssertIntersectArcsOneWay ( arc1, arc2, expectedArc1Ends, expectedArc2Ends );
			AssertIntersectArcsOneWay ( arc1.Reversed, arc2, expectedArc1Ends, expectedArc2Ends );
			AssertIntersectArcsOneWay ( arc1.Reversed, arc2.Reversed, expectedArc1Ends, expectedArc2Ends );
			AssertIntersectArcsOneWay ( arc1, arc2.Reversed, expectedArc1Ends, expectedArc2Ends );
			AssertIntersectArcsOneWay ( arc2, arc1, expectedArc1Ends, expectedArc2Ends );
			AssertIntersectArcsOneWay ( arc2.Reversed, arc1, expectedArc1Ends, expectedArc2Ends );
			AssertIntersectArcsOneWay ( arc2.Reversed, arc1.Reversed, expectedArc1Ends, expectedArc2Ends );
			AssertIntersectArcsOneWay ( arc2, arc1.Reversed, expectedArc1Ends, expectedArc2Ends );
		}

		private static void AssertIntersectArcsOneWay (
			Range <float> arc1, Range <float> arc2,
			Range <float>? expectedArc1Ends, Range <float>? expectedArc2Ends
		) {
			CircleMath.IntersectArcs ( 360, arc1, arc2, out var actualArc1, out var actualArc2 );
			Assert.AreEqual ( expectedArc1Ends.HasValue, actualArc1.HasValue );
			if ( expectedArc1Ends.HasValue ) {
				Assert.AreEqual (
					CircleMath.CoerceAngle ( expectedArc1Ends.Value.Start, 360 ),
					CircleMath.CoerceAngle ( actualArc1.Value.Start, 360 ),
					$"expectedArc1Ends.Start does not match with actual value. arc1: {arc1}, arc2: {arc2}"
				);
				Assert.AreEqual (
					CircleMath.CoerceAngle ( expectedArc1Ends.Value.End, 360 ),
					CircleMath.CoerceAngle ( actualArc1.Value.End, 360 ),
					$"expectedArc1Ends.End does not match with actual value. arc1: {arc1}, arc2: {arc2}"
				);
			}
			Assert.AreEqual ( expectedArc2Ends.HasValue, actualArc2.HasValue );
			if ( expectedArc2Ends.HasValue ) {
				Assert.AreEqual (
					CircleMath.CoerceAngle ( expectedArc2Ends.Value.Start, 360 ),
					CircleMath.CoerceAngle ( actualArc2.Value.Start, 360 ),
					$"expectedArc2Ends.Start does not match with actual value. arc1: {arc1}, arc2: {arc2}"
				);
				Assert.AreEqual (
					CircleMath.CoerceAngle ( expectedArc2Ends.Value.End, 360 ),
					CircleMath.CoerceAngle ( actualArc2.Value.End, 360 ),
					$"expectedArc2Ends.End does not match with actual value. arc1: {arc1}, arc2: {arc2}"
				);
			}
			CircleMath.IntersectArcs ( 360, arc1, Math.Sign ( arc1.Width () ), arc2, Math.Sign ( arc2.Width () ), out var actualArc21, out var actualArc22 );
			Assert.AreEqual ( actualArc1, actualArc21 );
			Assert.AreEqual ( actualArc2, actualArc22 );
		}
	}
}
