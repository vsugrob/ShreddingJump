using System;
using NUnit.Framework;

namespace Tests {
    public class RangeComparisonTests {
		[Test]
		public void Range_NonPoint_CompareOrderedTo () {
			AssertSubtractOrdered ( 1, 3, 4, -1 );
			AssertSubtractOrdered ( 1, 3, 3, -1 );
			AssertSubtractOrdered ( 1, 3, 2, 0 );
			AssertSubtractOrdered ( 1, 3, 1, 1 );
			AssertSubtractOrdered ( 1, 3, 0, 1 );
		}

		[Test]
		public void Range_Point_CompareOrderedTo () {
			AssertSubtractOrdered ( 1, 1, 2, -1 );
			AssertSubtractOrdered ( 1, 1, 1, 0 );
			AssertSubtractOrdered ( 1, 1, 0, 1 );
		}

		private static void AssertSubtractOrdered <T> ( T s, T e, T p, int expectedResult )
			where T : IComparable <T>
		{
			var r = RangeFactory.Create ( s, e );
			Assert.IsTrue ( r.IsOrdered );
			Assert.AreEqual ( expectedResult, r.CompareOrderedTo ( p ) );
		}
	}
}
