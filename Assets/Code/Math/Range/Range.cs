namespace System {
	public static class Range {
		public static Range <T> Create <T> ( T start, T end )
			where T : IComparable <T>
		{
			return	new Range <T> ( start, end );
		}

		public static Range <T> Ordered <T> ( T start, T end )
			where T : IComparable <T>
		{
			var r = new Range <T> ( start, end );
			r.Order ();
			return	r;
		}

		public static Range <T> Point <T> ( T point )
			where T : IComparable <T>
		{
			return	new Range <T> ( point );
		}
	}
}
