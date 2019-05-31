using System.Collections.Generic;
using NUnit.Framework;


namespace Tests {
    public class FragmentedCircleTests {
		[Test]
		public void FragmentedCircle_AddPositiveNonTrespassingRange () {
			AssertAddPositiveNonOverlappingRange ( 0, 45 );
			AssertAddPositiveNonOverlappingRange ( 5, 6 );
			AssertAddPositiveNonOverlappingRange ( 0, 360 );
			AssertAddPositiveNonOverlappingRange ( 270, 360 );
			AssertAddPositiveNonOverlappingRange ( 359, 360 );
		}

		private void AssertAddPositiveNonOverlappingRange ( float start, float end ) {
			var circle = FragmentedCircle.CreateDegrees <string> ();
			const string E0 = "r0";
			circle.Add ( E0, start, end );
			Assert.AreEqual ( 1, circle.Count );
			var f0 = circle [0];
			Assert.AreEqual ( E0, f0.Element );
			var r0 = f0.Range;
			Assert.AreEqual ( start, r0.Start );
			Assert.AreEqual ( end, r0.End );
		}
    }
}
