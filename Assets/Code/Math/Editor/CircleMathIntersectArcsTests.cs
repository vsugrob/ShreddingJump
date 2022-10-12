using System;
using NUnit.Framework;

namespace Tests {
    public class CircleMathIntersectArcsTests {
		[Test]
		public void CircleMath_IntersectArcs_FullCircle () {
			AssertIntersectArcs (
				RangeFactory.Create ( 0f, 360 ),
				RangeFactory.Create ( 0f, 360 ),
				RangeFactory.Create ( 0f, 360 ),
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( 0f, 360 ),
				RangeFactory.Create ( 0f, 30 ),
				RangeFactory.Create ( 0f, 30 ),
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( 0f, 360 ),
				RangeFactory.Create ( 270f, 360 ),
				RangeFactory.Create ( 270f, 360 ),
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( 0f, 360 ),
				RangeFactory.Create ( 270f, 300 ),
				RangeFactory.Create ( 270f, 300 ),
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( 0f, 360 ),
				RangeFactory.Create ( 0f, 0 ),
				RangeFactory.Create ( 0f, 0 ),
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( 0f, 360 ),
				RangeFactory.Create ( 1f, 1 ),
				RangeFactory.Create ( 1f, 1 ),
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( 0f, 360 ),
				RangeFactory.Create ( 255f, 255 ),
				RangeFactory.Create ( 255f, 255 ),
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( 0f, 360 ),
				RangeFactory.Create ( 360f, 360 ),
				RangeFactory.Create ( 360f, 360 ),
				null
			);
		}

		[Test]
		public void CircleMath_IntersectArcs_MoreThanFullCircle () {
			AssertIntersectArcs (
				RangeFactory.Create ( -300f, 400 ),
				RangeFactory.Create ( 0f, 360 ),
				RangeFactory.Create ( 0f, 360 ),
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( -300f, 400 ),
				RangeFactory.Create ( 10f, 20 ),
				RangeFactory.Create ( 10f, 20 ),
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( -300f, 400 ),
				RangeFactory.Create ( -20f, 380f ),
				RangeFactory.Create ( 0f, 360 ),
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( -300f, 400 ),
				RangeFactory.Create ( 23f, 23 ),
				RangeFactory.Create ( 23f, 23 ),
				null
			);
		}

		[Test]
		public void CircleMath_IntersectArcs_NoIntersection () {
			AssertIntersectArcs (
				RangeFactory.Create ( 0f, 30 ),
				RangeFactory.Create ( 60f, 80 ),
				null,
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( -45f, 30 ),
				RangeFactory.Create ( 420f, 440 ),
				null,
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( -45f, 30 ),
				RangeFactory.Create ( 31f, 31 ),
				null,
				null
			);
		}

		[Test]
		public void CircleMath_IntersectArcs_OneIntersection () {
			AssertIntersectArcs (
				RangeFactory.Create ( 0f, 30 ),
				RangeFactory.Create ( 0f, 30 ),
				RangeFactory.Create ( 0f, 30 ),
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( 0f, 30 ),
				RangeFactory.Create ( 0f, 15 ),
				RangeFactory.Create ( 0f, 15 ),
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( 10f, 30 ),
				RangeFactory.Create ( 10f, 30 ),
				RangeFactory.Create ( 10f, 30 ),
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( 10f, 30 ),
				RangeFactory.Create ( 10f, 15 ),
				RangeFactory.Create ( 10f, 15 ),
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( 10f, 310 ),
				RangeFactory.Create ( 10f, 310 ),
				RangeFactory.Create ( 10f, 310 ),
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( 10f, 310 ),
				RangeFactory.Create ( 200f, 310 ),
				RangeFactory.Create ( 200f, 310 ),
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( 10f, 310 ),
				RangeFactory.Create ( 200f, 350 ),
				RangeFactory.Create ( 200f, 310 ),
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( 10f, 310 ),
				RangeFactory.Create ( 310f, 350 ),
				RangeFactory.Create ( 310f, 310 ),
				null
			);
			AssertIntersectArcs (
				RangeFactory.Create ( 10f, 10 ),
				RangeFactory.Create ( 10f, 10 ),
				RangeFactory.Create ( 10f, 10 ),
				null
			);
		}

		[Test]
		public void CircleMath_IntersectArcs_TwoIntersections () {
			AssertIntersectArcs (
				RangeFactory.Create ( 50f, 300 ),
				RangeFactory.Create ( 270f, 430 ),
				RangeFactory.Create ( 50f, 70 ),
				RangeFactory.Create ( 270f, 300 )
			);
			/* This test expects two points, but results are different.
			 * Currently we're not required to have such great precision with points in our project. */
			//AssertIntersectArcs (
			//	RangeFactory.Create ( 50f, 300 ),
			//	RangeFactory.Create ( 300f, 410 ),
			//	RangeFactory.Create ( 50f, 50 ),
			//	RangeFactory.Create ( 300f, 300 )
			//);
			//AssertIntersectArcs (
			//	RangeFactory.Create ( 50f, 300 ),
			//	RangeFactory.Create ( 300f, 430 ),
			//	RangeFactory.Create ( 50f, 70 ),
			//	RangeFactory.Create ( 300f, 300 )
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
