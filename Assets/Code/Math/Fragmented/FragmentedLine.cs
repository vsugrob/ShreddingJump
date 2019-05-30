﻿using System.Diagnostics;

namespace System.Collections.Generic {
	[DebuggerDisplay ( "Count = {Count}" )]
	public class FragmentedLine <TElement, TLimit> : IReadOnlyList <LineFragment <TElement, TLimit>>
		where TLimit : IComparable <TLimit>
	{
		private List <LineFragment <TElement, TLimit>> fragments = new List <LineFragment <TElement, TLimit>> ();
		public int Count => fragments.Count;
		public TLimit MinLimit { get; private set; }
		public TLimit MaxLimit { get; private set; }
		public LineFragment <TElement, TLimit> this [int index] => fragments [index];

		public FragmentedLine ( TLimit minLimit, TLimit maxLimit ) {
			if ( ReferenceEquals ( minLimit, null ) )
				throw new ArgumentNullException ( nameof ( minLimit ) );

			if ( ReferenceEquals ( maxLimit, null ) )
				throw new ArgumentNullException ( nameof ( maxLimit ) );

			if ( minLimit.CompareTo ( maxLimit ) >= 0 )
				throw new ArgumentException ( "maxLimit limit must be greater than minLimit.", nameof ( maxLimit ) );

			this.MinLimit = minLimit;
			this.MaxLimit = maxLimit;
		}

		public void Add ( TElement element, TLimit start, TLimit end ) {
			Add ( element, Range.Create ( start, end ) );
		}

		public void Add ( TElement element, Range <TLimit> range ) {
			range.Order ();
			int index = CountFragmentsLessThanOrEqual ( range.Start );
			fragments.Insert ( index, new LineFragment <TElement, TLimit> ( element, range ) );
		}

		private int CountFragmentsLessThanOrEqual ( TLimit margin ) {
			int count = 0;
			// TODO: optimize for orderd list of fragments. It MUST be better than O(n).
			for ( int i = 0 ; i < fragments.Count ; i++ ) {
				if ( fragments [i].Range <= margin )
					count++;
			}

			return	count;
		}

		public bool TryFindEmptyRange ( out Range <TLimit> emptyRange ) {
			if ( fragments.Count == 0 ) {
				emptyRange = Range.Create ( MinLimit, MaxLimit );
				return	true;
			}

			var prevRangeEnd = MinLimit;
			for ( int i = 0 ; i < fragments.Count ; i++ ) {
				var range = fragments [i].Range;
				var rangeStart = range.Start;
				if ( rangeStart.CompareTo ( prevRangeEnd ) > 0 ) {
					emptyRange = Range.Create ( prevRangeEnd, rangeStart );
					return	true;
				}

				prevRangeEnd = range.End;
			}

			var lastRange = fragments [fragments.Count - 1].Range;
			var lastRangeEnd = lastRange.End;
			if ( lastRangeEnd.CompareTo ( MaxLimit ) < 0 ) {
				emptyRange = Range.Create ( lastRangeEnd, MaxLimit );
				return	true;
			}

			emptyRange = default;
			return	false;
		}

		public IEnumerator <LineFragment <TElement, TLimit>> GetEnumerator () {
			return	fragments.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator () {
			return	( ( IEnumerable ) fragments ).GetEnumerator ();
		}
	}
}
