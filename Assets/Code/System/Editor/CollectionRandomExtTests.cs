using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests {
    public class CollectionRandomExtTests {
		[Test]
		public void WeightedRandomElementPicking_UniqueWeights () {
			TestWeightedValuesDistribution ( new WeightedValue <string> [] {
				WeightedValue.Create ( "a", 1 ),
			} );
			TestWeightedValuesDistribution ( new WeightedValue <string> [] {
				WeightedValue.Create ( "a", 1 ),
				WeightedValue.Create ( "b", 4 ),
			} );
			TestWeightedValuesDistribution ( new WeightedValue <string> [] {
				WeightedValue.Create ( "a", 1 ),
				WeightedValue.Create ( "b", 4 ),
				WeightedValue.Create ( "c", 3 ),
				WeightedValue.Create ( "d", 2 ),
			} );
		}

		private void TestWeightedValuesDistribution <TValue> ( WeightedValue <TValue> [] weightedValues, int pickCount = 1000 ) {
			var counters = new Dictionary <TValue, int> ();
			for ( int i = 0 ; i < pickCount ; i++ ) {
				var value = weightedValues.TakeRandomSingleOrDefault ();
				counters.TryGetValue ( value, out var counter );
				counters [value] = counter + 1;
			}

			var expectedOrder = weightedValues.OrderBy ( wv => wv.Weight ).Select ( wv => wv.Value );
			var actualOrder = counters.OrderBy ( kv => kv.Value ).Select ( kv => kv.Key );
			CollectionAssert.AreEqual ( expectedOrder, actualOrder );
		}
	}
}
