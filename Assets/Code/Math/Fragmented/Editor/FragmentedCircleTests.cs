using System.Collections.Generic;
using NUnit.Framework;

namespace Tests {
    public class FragmentedCircleTests {
		[Test]
		public void FragmentedCircle_AddPointInBounds () {
			AssertAddRangeNoSplit ( 0, 0 );
			AssertAddRangeNoSplit ( 1, 1 );
			AssertAddRangeNoSplit ( 1.0001f, 1.0001f );
			AssertAddRangeNoSplit ( 315, 315 );
			AssertAddRangeNoSplit ( 360, 360 );
		}

		[Test]
		public void FragmentedCircle_AddPointOutOfBoundsNegative () {
			AssertAddRangeNoSplit ( -1, -1, 359 );
			AssertAddRangeNoSplit ( -0.01f, -0.01f, 359.99f );
			AssertAddRangeNoSplit ( -1.0001f, -1.0001f, 358.9999f );
			AssertAddRangeNoSplit ( -315, -315, 45 );
			AssertAddRangeNoSplit ( -360, -360, 0 );
			AssertAddRangeNoSplit ( -361, -361, 359 );
			AssertAddRangeNoSplit ( -450, -450, 270 );
			AssertAddRangeNoSplit ( -720, -720, 0 );
			AssertAddRangeNoSplit ( -719.99f, -719.99f, 0.0100097656f );
		}

		[Test]
		public void FragmentedCircle_AddPointOutOfBoundsPositive () {
			AssertAddRangeNoSplit ( 360.0345f, 360.0345f, 0.0344848633f );
			AssertAddRangeNoSplit ( 362, 362, 2 );
			AssertAddRangeNoSplit ( 719, 719, 359 );
			AssertAddRangeNoSplit ( 3620, 3620, 20 );
		}

		[Test]
		public void FragmentedCircle_AddPositiveDirRangeInBoundsNoSplit () {
			AssertAddRangeNoSplit ( 0, 0.01f );
			AssertAddRangeNoSplit ( 0, 45 );
			AssertAddRangeNoSplit ( 5, 6 );
			AssertAddRangeNoSplit ( 79.99f, 80.01f );
			AssertAddRangeNoSplit ( 0, 360 );
			AssertAddRangeNoSplit ( 270, 360 );
			AssertAddRangeNoSplit ( 359, 360 );
		}

		[Test]
		public void FragmentedCircle_AddNegativeDirRangeInBoundsNoSplit () {
			AssertAddRangeNoSplit ( 0.01f, 0 );
			AssertAddRangeNoSplit ( 12, 1 );
			AssertAddRangeNoSplit ( 76, 75 );
			AssertAddRangeNoSplit ( 95.01345f, 77.345f );
			AssertAddRangeNoSplit ( 360, 0 );
			AssertAddRangeNoSplit ( 359, 1 );
			AssertAddRangeNoSplit ( 360, 359.99f );
		}

		[Test]
		public void FragmentedCircle_AddPositiveDirRangeOutOfBoundsPositiveNoSplit () {
			AssertAddRangeNoSplit ( 360, 361, 0, 1 );
			AssertAddRangeNoSplit ( 360.0003f, 360.0004f, 000305175781f, 0.000396728516f );
			AssertAddRangeNoSplit ( 360, 720, 0, 360 );
			AssertAddRangeNoSplit ( 415, 699, 55, 339 );
			AssertAddRangeNoSplit ( 720, 1080, 0, 360 );
		}

		[Test]
		public void FragmentedCircle_AddNegativeDirRangeOutOfBoundsPositiveNoSplit () {
			AssertAddRangeNoSplit ( 365, 360, 0, 5 );
			AssertAddRangeNoSplit ( 555, 444, 84, 195 );
			AssertAddRangeNoSplit ( 720, 360, 0, 360 );
			AssertAddRangeNoSplit ( 1070, 720, 0, 350 );
			AssertAddRangeNoSplit ( 1080, 720, 0, 360 );
			AssertAddRangeNoSplit ( 1080, 900, 180, 360 );
			AssertAddRangeNoSplit ( 1011, 911, 191, 291 );
		}

		[Test]
		public void FragmentedCircle_AddNegativeDirRangeOutOfBoundsNegativeNoSplit () {
			AssertAddRangeNoSplit ( 0, -1, 359, 360 );
			AssertAddRangeNoSplit ( 0, -0.0001f, 359.9999f, 360 );
			AssertAddRangeNoSplit ( -0.0001f, -0.0101f, 359.9899f, 359.9999f );
			AssertAddRangeNoSplit ( -1, -185, 175, 359 );
			AssertAddRangeNoSplit ( 0, -360, 0, 360 );
			AssertAddRangeNoSplit ( -89, -275, 85, 271 );
			AssertAddRangeNoSplit ( -215, -360, 145, 360 );
			AssertAddRangeNoSplit ( -360, -361, 359, 360 );
			AssertAddRangeNoSplit ( -360, -720, 0, 360 );
			AssertAddRangeNoSplit ( -523, -698, 22, 197 );
			AssertAddRangeNoSplit ( -478, -720, 242, 360 );
			AssertAddRangeNoSplit ( -720, -1080, 0, 360 );
		}

		[Test]
		public void FragmentedCircle_AddPositiveDirRangeOutOfBoundsNegativeNoSplit () {
			AssertAddRangeNoSplit ( -1, 0, 359, 360 );
			AssertAddRangeNoSplit ( -345, -32, 15, 328 );
			AssertAddRangeNoSplit ( -360, 0, 0, 360 );
			AssertAddRangeNoSplit ( -576, -360, 144, 360 );
			AssertAddRangeNoSplit ( -720, -360, 0, 360 );
			AssertAddRangeNoSplit ( -720, -431, 0, 289 );
			AssertAddRangeNoSplit ( -1000, -800, 80, 280 );
			AssertAddRangeNoSplit ( -1080, -720, 0, 360 );
		}

		private void AssertAddRangeNoSplit ( float start, float end, float? expectedStart = null, float? expectedEnd = null ) {
			var circle = FragmentedCircle.CreateDegrees <string> ();
			const string E0 = "r0";
			circle.Add ( E0, start, end );
			Assert.AreEqual ( 1, circle.Count );
			var f0 = circle [0];
			Assert.AreEqual ( E0, f0.Element );
			var r0 = f0.Range;
			if ( !expectedStart.HasValue ) {
				Assert.IsTrue ( 0 <= start && start <= 360 ); 
				Assert.IsTrue ( 0 <= end && end <= 360 ); 
				MathHelper.SortMinMax ( ref start, ref end );
				expectedStart = start;
				expectedEnd = end;
			} else if ( !expectedEnd.HasValue )
				expectedEnd = expectedStart;

			Assert.AreEqual ( expectedStart.Value, r0.Start );
			Assert.AreEqual ( expectedEnd.Value, r0.End );
		}
    }
}
