using System.Collections.Generic;
using System.Linq;

namespace System {
	public static class CollectionRandomExt {
		public static void Shuffle <TElement> ( this ICollection <TElement> list ) {
			var itemsLeft = list.ToList ();
			int count = list.Count;
			list.Clear ();

			for ( int i = 0 ; i < count ; i++ ) {
				int index = UnityEngine.Random.Range ( 0, itemsLeft.Count );
				list.Add ( itemsLeft [index] );
				itemsLeft.RemoveAt ( index );
			}
		}

		/// <summary>
		/// <para>
		/// Obtain enumeration of <paramref name="count"/> elements from <paramref name="source"/> collection in random order.
		/// When <paramref name="source"/> collection consists of unique elements, resulting enumeration won't contain duplicates as well.
		/// </para>
		/// <para>
		/// When <paramref name="source"/> collection has less than <paramref name="count"/> elements,
		/// this method yields whole collection.
		/// </para>
		/// <para>
		/// Yields all elements of <paramref name="source"/> collection when <paramref name="count"/> is set to negative value.
		/// </para>
		/// </summary>
		public static IEnumerable <TElement> TakeRandom <TElement> ( this IEnumerable <TElement> source, int count = -1 ) {
			var itemsLeft = source.ToList ();
			if ( itemsLeft.Count < count || count < 0 )
				count = itemsLeft.Count;

			for ( int i = 0 ; i < count ; i++ ) {
				int index = UnityEngine.Random.Range ( 0, itemsLeft.Count );
				yield return	itemsLeft [index];
				itemsLeft.RemoveAt ( index );
			}
		}

		public static TElement TakeRandomSingleOrDefault <TElement> ( this IEnumerable <TElement> source ) {
			if ( source == null )
				throw new ArgumentNullException ( nameof ( source ) );

			if ( !( source is IList<TElement> items ) )
				items = source.ToArray ();

			var count = items.Count;
			if ( count == 0 )
				return	default;

			int index = UnityEngine.Random.Range ( 0, items.Count );
			return	items [index];
		}
	}
}
