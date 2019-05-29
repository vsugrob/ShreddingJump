using System.Diagnostics;

namespace System.Collections.Generic {
	[DebuggerDisplay ( "Count = {Count}" )]
	public class FragmentedCircle <TElement, TLimit>
		where TLimit : IComparable <TLimit>
	{
		private List <CircleFragment <TElement, TLimit>> fragments = new List <CircleFragment <TElement, TLimit>> ();
		public int Count => fragments.Count;
		public TLimit MinLimit { get; private set; }
		public TLimit MaxLimit { get; private set; }

		public FragmentedCircle ( TLimit minLimit, TLimit maxLimit ) {
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
			fragments.Insert ( index, new CircleFragment <TElement, TLimit> ( element, range ) );
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

		public Range <TLimit> FindEmptyRange () {
			var start = MinLimit;
			var end = MinLimit;
			for ( int i = 0 ; i < fragments.Count ; i++ ) {
				var fragment = fragments [i];
				// TODO: implement.
			}
		}
	}
}
