using System.Diagnostics;

namespace System.Collections.Generic {
	[DebuggerDisplay ( "Count = {Count}" )]
	public class FragmentedLine <TElement, TLimit> : IReadOnlyList <LineFragment <TElement, TLimit>>
		where TLimit : IComparable <TLimit>
	{
		private SortedList <TLimit, LineFragment <TElement, TLimit>> fragmentsByStart = new SortedList <TLimit, LineFragment <TElement, TLimit>> ();
		public int Count => fragmentsByStart.Count;
		public TLimit MinLimit { get; private set; }
		public TLimit MaxLimit { get; private set; }
		public LineFragment <TElement, TLimit> this [int index] => fragmentsByStart.Values [index];

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

		public virtual void Add ( TElement element, TLimit start, TLimit end ) {
			Add ( element, Range.Create ( start, end ) );
		}

		public virtual void Add ( TElement element, Range <TLimit> range ) {
			range.Order ();
			AddOrdered ( element, range );
		}

		protected void AddOrdered ( TElement element, Range <TLimit> range ) {
			fragmentsByStart.Add ( range.Start, new LineFragment <TElement, TLimit> ( element, range ) );
		}

		public bool TryFindEmptyRange ( out Range <TLimit> emptyRange ) {
			if ( fragmentsByStart.Count == 0 ) {
				emptyRange = Range.Create ( MinLimit, MaxLimit );
				return	true;
			}

			var prevRangeEnd = MinLimit;
			var fragments = fragmentsByStart.Values;
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
			return	fragmentsByStart.Values.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator () {
			return	( ( IEnumerable ) fragmentsByStart.Values ).GetEnumerator ();
		}
	}
}
