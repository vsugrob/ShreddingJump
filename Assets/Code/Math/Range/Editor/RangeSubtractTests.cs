using System;
using NUnit.Framework;

namespace Tests {
    public class RangeSubtractTests {
		[Test]
		public void Range_Subtract () {
			AssertSubtractOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 3, 4 ),
				RangeFactory.Create ( 0, 2 ),
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 2, 3 ),
				RangeFactory.Create ( 0, 2 ),
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 1, 3 ),
				RangeFactory.Create ( 0, 1 ),
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 0, 3 ),
				null,
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( -1, 3 ),
				null,
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 1, 2 ),
				RangeFactory.Create ( 0, 1 ),
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 0, 2 ),
				null,
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( -1, 2 ),
				null,
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 0, 3 ),
				RangeFactory.Create ( 1, 2 ),
				RangeFactory.Create ( 0, 1 ),
				RangeFactory.Create ( 2, 3 )
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 0, 1 ),
				RangeFactory.Create ( 1, 2 ),
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( -1, 1 ),
				RangeFactory.Create ( 1, 2 ),
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( -1, 0 ),
				RangeFactory.Create ( 0, 2 ),
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( -2, -1 ),
				RangeFactory.Create ( 0, 2 ),
				null
			);
		}

		[Test]
		public void Range_SubtractPoint () {
			AssertSubtractOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 3, 3 ),
				RangeFactory.Create ( 0, 2 ),
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 2, 2 ),
				RangeFactory.Create ( 0, 2 ),
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 1, 1 ),
				RangeFactory.Create ( 0, 1 ),
				RangeFactory.Create ( 1, 2 )
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( 0, 0 ),
				RangeFactory.Create ( 0, 2 ),
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 0, 2 ),
				RangeFactory.Create ( -1, -1 ),
				RangeFactory.Create ( 0, 2 ),
				null
			);
		}

		[Test]
		public void Range_SubtractFromPoint () {
			AssertSubtractOrdered (
				RangeFactory.Create ( 2, 2 ),
				RangeFactory.Create ( 3, 4 ),
				RangeFactory.Create ( 2, 2 ),
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 2, 2 ),
				RangeFactory.Create ( 3, 3 ),
				RangeFactory.Create ( 2, 2 ),
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 2, 2 ),
				RangeFactory.Create ( 2, 3 ),
				null,
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 2, 2 ),
				RangeFactory.Create ( -1, 3 ),
				null,
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 2, 2 ),
				RangeFactory.Create ( 2, 2 ),
				null,
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 2, 2 ),
				RangeFactory.Create ( 1, 2 ),
				null,
				null
			);
			AssertSubtractOrdered (
				RangeFactory.Create ( 2, 2 ),
				RangeFactory.Create ( 1, 1 ),
				RangeFactory.Create ( 2, 2 ),
				null
			);
		}

		private static void AssertSubtractOrdered <T> ( Range <T> a, Range <T> b, Range <T>? expectedR1, Range <T>? expectedR2 )
			where T : IComparable <T>
		{
			RangeFactory.SubtractOrdered ( a, b, out var actualR1, out var actualR2 );
			Assert.AreEqual ( expectedR1, actualR1 );
			Assert.AreEqual ( expectedR2, actualR2 );
		}
	}
}
