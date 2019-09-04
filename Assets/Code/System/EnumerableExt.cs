using System.Collections.Generic;
using UnityEngine;

namespace System.Linq {
	public static class EnumerableExt {
		public static void Consume <T> ( this IEnumerable <T> source ) {
			var en = source.GetEnumerator ();
			while ( en.MoveNext () );
		}

		public static IEnumerable <T> TakeFraction <T> ( this IEnumerable <T> source, float fraction, RoundingMode roundingMode = RoundingMode.Ceil ) {
			fraction = Mathf.Clamp01 ( fraction );
			var elements = source.ToArray ();
			var count = ( int ) MathHelper.Round ( elements.Length * fraction, roundingMode );
			for ( int i = 0 ; i < count ; i++ ) {
				yield return	elements [i];
			}
		}
	}
}
