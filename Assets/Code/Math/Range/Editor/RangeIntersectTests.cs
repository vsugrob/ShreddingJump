using System;
using NUnit.Framework;

namespace Tests {
    public class RangeIntersectTests {
		[Test]
		public void Range_Intersect () {
			AssertIntersectOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 3, 4 ),
				null
			);
			AssertIntersectOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 2, 4 ),
				RangeFactory.Create ( 2, 2 )
			);
			AssertIntersectOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 1, 4 ),
				RangeFactory.Create ( 1, 2 )
			);
			AssertIntersectOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 0, 4 ),
				RangeFactory.Create ( 0, 2 )
			);
			AssertIntersectOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( -1, 4 ),
				RangeFactory.Create ( 0, 2 )
			);
			AssertIntersectOrdered (
				RangeFactory.Create ( 0, 5 ),
				RangeFactory.Create ( -1, 4 ),
				RangeFactory.Create ( 0, 4 )
			);
			AssertIntersectOrdered (
				RangeFactory.Create ( 4, 5 ),
				RangeFactory.Create ( -1, 4 ),
				RangeFactory.Create ( 4, 4 )
			);
			AssertIntersectOrdered (
				RangeFactory.Create ( 5, 6 ),
				RangeFactory.Create ( -1, 4 ),
				null
			);
		}

		[Test]
		public void Range_IntersectOnePoint () {
			AssertIntersectOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 3, 3 ),
				null
			);
			AssertIntersectOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 2, 2 ),
				RangeFactory.Create ( 2, 2 )
			);
			AssertIntersectOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 1, 1 ),
				RangeFactory.Create ( 1, 1 )
			);
			AssertIntersectOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 0, 0 ),
				RangeFactory.Create ( 0, 0 )
			);
			AssertIntersectOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( -1, -1 ),
				null
			);
			AssertIntersectOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 3, 3 ),
				null
			);
		}

		[Test]
		public void Range_IntersectTwoPoints () {
			AssertIntersectOrdered (
				RangeFactory.Create ( 2, 2 ),
				RangeFactory.Create ( 3, 3 ),
				null
			);
			AssertIntersectOrdered (
				RangeFactory.Create ( 2, 2 ),
				RangeFactory.Create ( 2, 2 ),
				RangeFactory.Create ( 2, 2 )
			);
			AssertIntersectOrdered (
				RangeFactory.Create ( 2, 2 ),
				RangeFactory.Create ( 1, 1 ),
				null
			);
		}

		private static void AssertIntersectOrdered <T> ( Range <T> a, Range <T> b, Range <T>? expectedR )
			where T : IComparable <T>
		{
			RangeFactory.IntersectOrdered ( a, b, out var actualR );
			Assert.AreEqual ( expectedR, actualR );
			RangeFactory.IntersectOrdered ( b, a, out actualR );
			Assert.AreEqual ( expectedR, actualR );
		}
	}
}
