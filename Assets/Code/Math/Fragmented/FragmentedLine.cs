﻿using System.Diagnostics;

namespace System.Collections.Generic {
	[DebuggerDisplay ( "Count = {Count}" )]
	public class FragmentedLine <TElement, TLimit> : IReadOnlyList <LineFragment <TElement, TLimit>>
		where TLimit : IComparable <TLimit>
	{
		protected SortedList <TLimit, LineFragment <TElement, TLimit>> fragmentsByStart = new SortedList <TLimit, LineFragment <TElement, TLimit>> ();
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
			Add ( element, RangeFactory.Create ( start, end ) );
		}

		public virtual void Add ( TElement element, Range <TLimit> range ) {
			range.Order ();
			AddOrdered ( element, range );
		}

		public virtual void Add ( LineFragment <TElement, TLimit> fragment ) {
			Add ( fragment.Element, fragment.Range );
		}

		protected void AddOrdered ( TElement element, Range <TLimit> range ) {
			fragmentsByStart.Add ( range.Start, new LineFragment <TElement, TLimit> ( element, range ) );
		}

		public virtual void AddRange ( IEnumerable <LineFragment <TElement, TLimit>> fragments ) {
			foreach ( var fragment in fragments ) {
				Add ( fragment );
			}
		}

		public bool Remove ( LineFragment <TElement, TLimit> fragment ) {
			return	fragmentsByStart.Remove ( fragment.Range.Start );
		}

		public bool Remove ( TLimit start ) {
			return	fragmentsByStart.Remove ( start );
		}

		public void RemoveAt ( int index ) {
			fragmentsByStart.RemoveAt ( index );
		}

		public bool RemoveElement ( TElement element ) {
			var index = IndexOfElement ( element );
			if ( index < 0 )
				return	false;

			RemoveAt ( index );
			return	true;
		}

		public int IndexOfElement ( TElement element ) {
			int i = 0;
			foreach ( var fragment in fragmentsByStart.Values ) {
				if ( Equals ( fragment, element ) )
					return	i;

				i++;
			}

			return	-1;
		}

		public void Clear () {
			fragmentsByStart.Clear ();
		}

		public bool TryFindEmptyRange ( out Range <TLimit> emptyRange ) {
			var prevRangeEnd = MinLimit;
			var fragments = fragmentsByStart.Values;
			for ( int i = 0 ; i < fragments.Count ; i++ ) {
				var range = fragments [i].Range;
				var rangeStart = range.Start;
				if ( rangeStart.CompareTo ( prevRangeEnd ) > 0 ) {
					emptyRange = RangeFactory.Create ( prevRangeEnd, rangeStart );
					return	true;
				}

				prevRangeEnd = range.End;
			}

			if ( prevRangeEnd.CompareTo ( MaxLimit ) < 0 ) {
				emptyRange = RangeFactory.Create ( prevRangeEnd, MaxLimit );
				return	true;
			}

			emptyRange = default;
			return	false;
		}

		public List <Range <TLimit>> GetAllEmptyRanges () {
			var emptyRanges = new List <Range <TLimit>> ();
			if ( fragmentsByStart.Count == 0 ) {
				emptyRanges.Add ( RangeFactory.Create ( MinLimit, MaxLimit ) );
				return	emptyRanges;
			}

			var prevRangeEnd = MinLimit;
			var fragments = fragmentsByStart.Values;
			for ( int i = 0 ; i < fragments.Count ; i++ ) {
				var range = fragments [i].Range;
				var rangeStart = range.Start;
				if ( rangeStart.CompareTo ( prevRangeEnd ) > 0 )
					emptyRanges.Add ( RangeFactory.Create ( prevRangeEnd, rangeStart ) );

				prevRangeEnd = range.End;
			}

			if ( prevRangeEnd.CompareTo ( MaxLimit ) < 0 )
				emptyRanges.Add ( RangeFactory.Create ( prevRangeEnd, MaxLimit ) );

			return	emptyRanges;
		}

		public virtual bool Intersects ( Range <TLimit> range, bool includeTouch = true ) {
			var fragments = fragmentsByStart.Values;
			for ( int i = 0 ; i < fragments.Count ; i++ ) {
				var fragRange = fragments [i].Range;
				if ( fragRange.Intersects ( range, includeTouch ) )
					return	true;
			}

			return	false;
		}

		public virtual int SeekFragmentBoundary ( TLimit start, int dir, out TLimit boundary ) {
			ThrowIfOutOfBounds ( start );
			if ( dir == 0 || Count == 0 ) {
				boundary = default;
				return	-1;
			}

			var index = SeekClosestFragmentIndex ( start, dir );
			if ( index < 0 ) {
				boundary = default;
				return	-1;
			}

			var fragment = this [index];
			boundary = fragment.Range.GetBoundaryByDir ( -dir );
			return	index;
		}

		public virtual int SeekClosestFragmentIndex ( TLimit point, int dir ) {
			ThrowIfOutOfBounds ( point );
			int count = Count;
			if ( count == 0 )
				return	-1;

			dir = Math.Sign ( dir );
			int startIndex, endIndex, increment;
			if ( dir >= 0 ) {
				startIndex = count - 1;
				endIndex = -1;
				increment = -1;
			} else {
				startIndex = 0;
				endIndex = count;
				increment = 1;
			}

			int lastAcceptableIndex = -1;
			for ( int i = startIndex ; i != endIndex ; i += increment ) {
				var range = this [i].Range;
				var sign = range.CompareOrderedTo ( point );
				if ( sign == dir || ( sign == 0 && range.IsPoint ) )
					lastAcceptableIndex = i;
				else
					break;
			}

			return	lastAcceptableIndex;
		}

		private void ThrowIfOutOfBounds ( TLimit point ) {
			if ( MinLimit.CompareTo ( point ) > 0 || MaxLimit.CompareTo ( point ) < 0 )
				throw new ArgumentException ( $"Value must fall in range [{MinLimit};{MaxLimit}]." );
		}

		public IEnumerator <LineFragment <TElement, TLimit>> GetEnumerator () {
			return	fragmentsByStart.Values.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator () {
			return	( ( IEnumerable ) fragmentsByStart.Values ).GetEnumerator ();
		}
	}
}
