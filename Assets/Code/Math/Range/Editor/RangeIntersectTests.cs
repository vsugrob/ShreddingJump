using System;
using NUnit.Framework;

namespace Tests {
    public class RangeIntersectTests {
		[Test]
		public void Range_Intersect () {
			AssertIntersectOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 3, 4 ),
				null
			);
			AssertIntersectOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 2, 4 ),
				Range.Create ( 2, 2 )
			);
			AssertIntersectOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 1, 4 ),
				Range.Create ( 1, 2 )
			);
			AssertIntersectOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 0, 4 ),
				Range.Create ( 0, 2 )
			);
			AssertIntersectOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( -1, 4 ),
				Range.Create ( 0, 2 )
			);
			AssertIntersectOrdered (
				Range.Create ( 0, 5 ),
				Range.Create ( -1, 4 ),
				Range.Create ( 0, 4 )
			);
			AssertIntersectOrdered (
				Range.Create ( 4, 5 ),
				Range.Create ( -1, 4 ),
				Range.Create ( 4, 4 )
			);
			AssertIntersectOrdered (
				Range.Create ( 5, 6 ),
				Range.Create ( -1, 4 ),
				null
			);
		}

		[Test]
		public void Range_IntersectOnePoint () {
			AssertIntersectOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 3, 3 ),
				null
			);
			AssertIntersectOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 2, 2 ),
				Range.Create ( 2, 2 )
			);
			AssertIntersectOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 1, 1 ),
				Range.Create ( 1, 1 )
			);
			AssertIntersectOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 0, 0 ),
				Range.Create ( 0, 0 )
			);
			AssertIntersectOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( -1, -1 ),
				null
			);
			AssertIntersectOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 3, 3 ),
				null
			);
		}

		[Test]
		public void Range_IntersectTwoPoints () {
			AssertIntersectOrdered (
				Range.Create ( 2, 2 ),
				Range.Create ( 3, 3 ),
				null
			);
			AssertIntersectOrdered (
				Range.Create ( 2, 2 ),
				Range.Create ( 2, 2 ),
				Range.Create ( 2, 2 )
			);
			AssertIntersectOrdered (
				Range.Create ( 2, 2 ),
				Range.Create ( 1, 1 ),
				null
			);
		}

		private static void AssertIntersectOrdered <T> ( Range <T> a, Range <T> b, Range <T>? expectedR )
			where T : IComparable <T>
		{
			Range.IntersectOrdered ( a, b, out var actualR );
			Assert.AreEqual ( expectedR, actualR );
			Range.IntersectOrdered ( b, a, out actualR );
			Assert.AreEqual ( expectedR, actualR );
		}
	}
}
