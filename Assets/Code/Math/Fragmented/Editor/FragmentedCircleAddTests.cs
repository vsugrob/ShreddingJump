using System.Collections.Generic;
using NUnit.Framework;

namespace Tests {
    public class FragmentedCircleAddTests {
		[Test]
		public void FragmentedCircle_AddPointInBounds () {
			AssertAddPoint ( 0 );
			AssertAddPoint ( 1 );
			AssertAddPoint ( 1.0001f );
			AssertAddPoint ( 315 );
			AssertAddPoint ( 360, 0 );
		}

		[Test]
		public void FragmentedCircle_AddPointOutOfBoundsNegative () {
			AssertAddPoint ( -1, 359 );
			AssertAddPoint ( -0.01f, 359.99f );
			AssertAddPoint ( -1.0001f, 358.9999f );
			AssertAddPoint ( -315, 45 );
			AssertAddPoint ( -360, 0 );
			AssertAddPoint ( -361, 359 );
			AssertAddPoint ( -450, 270 );
			AssertAddPoint ( -720, 0 );
			AssertAddPoint ( -719.99f, 0.0100097656f );
		}

		[Test]
		public void FragmentedCircle_AddPointOutOfBoundsPositive () {
			AssertAddPoint ( 360.0345f, 0.0344848633f );
			AssertAddPoint ( 362, 2 );
			AssertAddPoint ( 719, 359 );
			AssertAddPoint ( 720, 0 );
			AssertAddPoint ( 3620, 20 );
		}

		[Test]
		public void FragmentedCircle_AddFullCircle () {
			AssertAddFullCircle ( 0, 360 );
			AssertAddFullCircle ( 360, 720 );
			AssertAddFullCircle ( 6840, 7200 );
			AssertAddFullCircle ( 0, 7200 );
			AssertAddFullCircle ( 360, 0 );
			AssertAddFullCircle ( 720, 360 );
			AssertAddFullCircle ( 7200, 6840 );
			AssertAddFullCircle ( 7200, 0 );
			AssertAddFullCircle ( 0, -360 );
			AssertAddFullCircle ( -360, -720 );
			AssertAddFullCircle ( -6840, -7200 );
			AssertAddFullCircle ( -0, -7200 );
			AssertAddFullCircle ( -360, 0 );
			AssertAddFullCircle ( -720, -360 );
			AssertAddFullCircle ( -7200, -6840 );
			AssertAddFullCircle ( -7200, 0 );
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
			AssertAddRangeNoSplit ( 360.0003f, 360.0004f, 0.000305175781f, 0.000396728516f );
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
			AssertAddRangeNoSplit ( -215, -360, 0, 145 );
			AssertAddRangeNoSplit ( -360, -361, 359, 360 );
			AssertAddRangeNoSplit ( -360, -720, 0, 360 );
			AssertAddRangeNoSplit ( -523, -698, 22, 197 );
			AssertAddRangeNoSplit ( -478, -720, 0, 242 );
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

		[Test]
		public void FragmentedCircle_AddPositiveDirRangePositiveWithSplit () {
			AssertAddRangeWithSplit ( 270, 361, 0, 1, 270, 360 );
			AssertAddRangeWithSplit ( 270, 450, 0, 90, 270, 360 );
			AssertAddRangeWithSplit ( 700, 740, 0, 20, 340, 360 );
			AssertAddRangeWithSplit ( 900, 1100, 0, 20, 180, 360 );
		}

		[Test]
		public void FragmentedCircle_AddNegativeDirRangePositiveWithSplit () {
			AssertAddRangeWithSplit ( 361, 359, 0, 1, 359, 360 );
			AssertAddRangeWithSplit ( 400, 359, 0, 40, 359, 360 );
			AssertAddRangeWithSplit ( 800, 500, 0, 80, 140, 360 );
			AssertAddRangeWithSplit ( 1111, 1011, 0, 31, 291, 360 );
		}

		[Test]
		public void FragmentedCircle_AddPositiveDirRangeNegativeWithSplit () {
			AssertAddRangeWithSplit ( -361, -359, 0, 1, 359, 360 );
			AssertAddRangeWithSplit ( -390, -250, 0, 110, 330, 360 );
			AssertAddRangeWithSplit ( -820, -690, 0, 30, 260, 360 );
		}

		[Test]
		public void FragmentedCircle_AddNegativeDirRangeNegativeWithSplit () {
			AssertAddRangeWithSplit ( -359, -361, 0, 1, 359, 360 );
			AssertAddRangeWithSplit ( -685, -770, 0, 35, 310, 360 );
		}

		[Test]
		public void FragmentedCircle_AddRangeWithSplitAroundZero () {
			AssertAddRangeWithSplit ( -1, 1, 0, 1, 359, 360 );
			AssertAddRangeWithSplit ( 1, -1, 0, 1, 359, 360 );
			AssertAddRangeWithSplit ( -30, 30, 0, 30, 330, 360 );
			AssertAddRangeWithSplit ( 30, -30, 0, 30, 330, 360 );
			AssertAddRangeWithSplit ( -179, 180, 0, 180, 181, 360 );
		}

		private void AssertAddPoint ( float point, float? expectedPoint = null ) {
			AssertAddRangeNoSplit ( point, point, expectedPoint );
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

		private void AssertAddFullCircle ( float start, float end ) {
			var circle = FragmentedCircle.CreateDegrees <string> ();
			const string E0 = "r0";
			circle.Add ( E0, start, end );
			Assert.AreEqual ( 1, circle.Count );
			var f0 = circle [0];
			Assert.AreEqual ( E0, f0.Element );
			var r0 = f0.Range;
			Assert.AreEqual ( 0, r0.Start );
			Assert.AreEqual ( 360, r0.End );
		}

		private void AssertAddRangeWithSplit ( float start, float end, float s1, float e1, float s2, float e2 ) {
			var circle = FragmentedCircle.CreateDegrees <string> ();
			const string E0 = "r0";
			circle.Add ( E0, start, end );
			Assert.AreEqual ( 2, circle.Count );
			var f0 = circle [0];
			var f1 = circle [1];
			Assert.AreEqual ( E0, f0.Element );
			Assert.AreEqual ( E0, f1.Element );
			var r0 = f0.Range;
			var r1 = f1.Range;
			Assert.AreEqual ( s1, r0.Start );
			Assert.AreEqual ( e1, r0.End );
			Assert.AreEqual ( s2, r1.Start );
			Assert.AreEqual ( e2, r1.End );
		}
    }
}
