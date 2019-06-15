using System;
using NUnit.Framework;

namespace Tests {
    public class RangeSubtractTests {
		[Test]
		public void Range_Subtract () {
			AssertSubtractOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 3, 4 ),
				Range.Create ( 0, 2 ),
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 2, 3 ),
				Range.Create ( 0, 2 ),
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 1, 3 ),
				Range.Create ( 0, 1 ),
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 0, 3 ),
				null,
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( -1, 3 ),
				null,
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 1, 2 ),
				Range.Create ( 0, 1 ),
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 0, 2 ),
				null,
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( -1, 2 ),
				null,
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 0, 3 ),
				Range.Create ( 1, 2 ),
				Range.Create ( 0, 1 ),
				Range.Create ( 2, 3 )
			);
			AssertSubtractOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 0, 1 ),
				Range.Create ( 1, 2 ),
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( -1, 1 ),
				Range.Create ( 1, 2 ),
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( -1, 0 ),
				Range.Create ( 0, 2 ),
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( -2, -1 ),
				Range.Create ( 0, 2 ),
				null
			);
		}

		[Test]
		public void Range_SubtractPoint () {
			AssertSubtractOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 3, 3 ),
				Range.Create ( 0, 2 ),
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 2, 2 ),
				Range.Create ( 0, 2 ),
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 1, 1 ),
				Range.Create ( 0, 1 ),
				Range.Create ( 1, 2 )
			);
			AssertSubtractOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( 0, 0 ),
				Range.Create ( 0, 2 ),
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 0, 2 ),
				Range.Create ( -1, -1 ),
				Range.Create ( 0, 2 ),
				null
			);
		}

		[Test]
		public void Range_SubtractFromPoint () {
			AssertSubtractOrdered (
				Range.Create ( 2, 2 ),
				Range.Create ( 3, 4 ),
				Range.Create ( 2, 2 ),
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 2, 2 ),
				Range.Create ( 3, 3 ),
				Range.Create ( 2, 2 ),
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 2, 2 ),
				Range.Create ( 2, 3 ),
				null,
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 2, 2 ),
				Range.Create ( -1, 3 ),
				null,
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 2, 2 ),
				Range.Create ( 2, 2 ),
				null,
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 2, 2 ),
				Range.Create ( 1, 2 ),
				null,
				null
			);
			AssertSubtractOrdered (
				Range.Create ( 2, 2 ),
				Range.Create ( 1, 1 ),
				Range.Create ( 2, 2 ),
				null
			);
		}

		private static void AssertSubtractOrdered <T> ( Range <T> a, Range <T> b, Range <T>? expectedR1, Range <T>? expectedR2 )
			where T : IComparable <T>
		{
			Range.SubtractOrdered ( a, b, out var actualR1, out var actualR2 );
			Assert.AreEqual ( expectedR1, actualR1 );
			Assert.AreEqual ( expectedR2, actualR2 );
		}
	}
}
