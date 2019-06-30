using System.Collections.Generic;

namespace System.Linq {
	public static class EnumerableExt {
		public static void Consume <T> ( this IEnumerable <T> source ) {
			var en = source.GetEnumerator ();
			while ( en.MoveNext () );
		}
	}
}
