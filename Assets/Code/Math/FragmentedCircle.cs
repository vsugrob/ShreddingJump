using System.Diagnostics;

namespace System.Collections.Generic {
	[DebuggerDisplay ( "Count = {Count}" )]
	public class FragmentedCircle <TElement, TLimit>
		where TLimit : IComparable <TLimit>
	{
		private List <CircleFragment <TElement, TLimit>> fragments = new List <CircleFragment <TElement, TLimit>> ();
		public int Count => fragments.Count;

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
			for ( int i = 0 ; i < fragments.Count ; i++ ) {
				if ( fragments [i].Range <= margin )
					count++;
			}

			return	count;
		}
	}
}
