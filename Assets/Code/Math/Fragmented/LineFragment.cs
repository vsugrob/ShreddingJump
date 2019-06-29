namespace System.Collections.Generic {
	public static class LineFragment {
		public static LineFragment <TElement, TLimit> Create <TElement, TLimit> ( TElement element, Range <TLimit> range )
			where TLimit : IComparable <TLimit>
		{
			return	new LineFragment <TElement, TLimit> ( element, range );
		}
	}
}
