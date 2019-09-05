using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests {
    public class CollectionRandomExtTests {
		[Test]
		public void WeightedRandomElementPicking_Empty () {
			Assert.IsNull ( new WeightedValue <string> [0].TakeRandomSingleOrDefault () );
		}

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

		[Test]
		public void WeightedRandomElementPicking_DuplicateWeights () {
			TestWeightedValuesDistribution ( new WeightedValue <string> [] {
				WeightedValue.Create ( "a", 1 ),
				WeightedValue.Create ( "b", 1 ),
			} );
			TestWeightedValuesDistribution ( new WeightedValue <string> [] {
				WeightedValue.Create ( "a", 1 ),
				WeightedValue.Create ( "b", 1 ),
				WeightedValue.Create ( "c", 2 ),
			} );
			TestWeightedValuesDistribution ( new WeightedValue <string> [] {
				WeightedValue.Create ( "x", 6 ),
				WeightedValue.Create ( "a", 1 ),
				WeightedValue.Create ( "b", 1 ),
				WeightedValue.Create ( "c", 2 ),
			} );
			TestWeightedValuesDistribution ( new WeightedValue <string> [] {
				WeightedValue.Create ( "x", 6 ),
				WeightedValue.Create ( "a", 1 ),
				WeightedValue.Create ( "b", 1 ),
				WeightedValue.Create ( "c", 2 ),
				WeightedValue.Create ( "d", 2 ),
			} );
			TestWeightedValuesDistribution ( new WeightedValue <string> [] {
				WeightedValue.Create ( "x", 6 ),
				WeightedValue.Create ( "a", 1 ),
				WeightedValue.Create ( "b", 1 ),
				WeightedValue.Create ( "y", 3 ),
				WeightedValue.Create ( "c", 2 ),
				WeightedValue.Create ( "d", 2 ),
				WeightedValue.Create ( "e", 2 ),
			} );
		}

		private void TestWeightedValuesDistribution <TValue> ( WeightedValue <TValue> [] weightedValues, int pickCount = 1000 ) {
			Assert.AreEqual ( weightedValues.Length, weightedValues.Select ( wv => wv.Value ).Distinct ().Count () );
			var counters = new Dictionary <TValue, int> ();
			for ( int i = 0 ; i < pickCount ; i++ ) {
				var value = weightedValues.TakeRandomSingleOrDefault ();
				counters.TryGetValue ( value, out var counter );
				counters [value] = counter + 1;
			}

			var actualOrder = counters.OrderBy ( kv => kv.Value ).Select ( kv => kv.Key ).ToArray ();
			Assert.AreEqual ( weightedValues.Length, actualOrder.Length );
			var expectedOrder = weightedValues.GroupBy ( wv => wv.Weight ).OrderBy ( g => g.Key );
			int actualIndex = 0;
			foreach ( var group in expectedOrder ) {
				var groupElements = group.Select ( wv => wv.Value ).ToArray ();
				for ( int i = 0 ; i < groupElements.Length ; i++ ) {
					var actualValue = actualOrder [actualIndex++];
					Assert.IsTrue ( groupElements.Contains ( actualValue ) );
				}
			}
		}
	}
}
