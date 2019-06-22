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

		public static void IntersectOrdered <T> ( Range <T> a, Range <T> b, out Range <T>? r )
			where T : IComparable <T>
		{
			var asbsSign = a.Start.CompareTo ( b.Start );
			if ( asbsSign <= 0 )
				IntersectOrderedAStartLTOrEqBStart ( a, b, asbsSign, out r );
			else
				IntersectOrderedAStartLTOrEqBStart ( b, a, -asbsSign, out r );
		}

		private static void IntersectOrderedAStartLTOrEqBStart <T> ( Range <T> a, Range <T> b, int asbsSign, out Range <T>? r )
			where T : IComparable <T>
		{
			if ( asbsSign == 0 ) {								// a.s == b.s
				var aebeSign = a.End.CompareTo ( b.End );
				if ( aebeSign <= 0 )							// a.e <= b.e
					r = a;										// { a.s==b.s, a.e, b.e }
				else											// a.e > b.e
					r = Create ( a.Start, b.End );				// { a.s==b.s, b.e, a.e }
			} else if ( asbsSign < 0 ) {						// a.s < b.s
				var aebsSign = a.End.CompareTo ( b.Start );
				if ( aebsSign == 0 )							// a.e == b.s
					r = Create ( a.End, a.End );				// { a.s, a.e==b.s, b.e }
				else if ( aebsSign < 0 )						// a.e < b.s
					r = null;									// { a.s, a.e, b.s, b.e }
				else {											// a.e > b.s
					var aebeSign = a.End.CompareTo ( b.End );
					if ( aebeSign <= 0 )						// a.e <= b.e
						r = Create ( b.Start, a.End );			// { a.s, b.s, a.e, b.e }
					else										// a.e > b.e
						r = b;									// { a.s, b.s, b.e, a.e }
				}
			} else												// a.s > b.s
				throw new InvalidOperationException ( "This method expects that a.Start <= b.Start." );
		}

		public static void SubtractOrdered <T> ( Range <T> a, Range <T> b, out Range <T>? r1, out Range <T>? r2 )
			where T : IComparable <T>
		{
			r2 = null;
			var asbsSign = a.Start.CompareTo ( b.Start );
			if ( asbsSign == 0 ) {								// a.s == b.s
				var aebeSign = a.End.CompareTo ( b.End );
				if ( aebeSign <= 0 )							// a.e <= b.e
					r1 = null;									// { a.s==b.s, a.e, b.e }
				else											// a.e > b.e
					r1 = Create ( b.End, a.End );				// { a.s==b.s, b.e, a.e }
			} else if ( asbsSign < 0 ) {						// a.s < b.s
				var aebsSign = a.End.CompareTo ( b.Start );
				if ( aebsSign <= 0 )							// a.e <= b.s
					r1 = a;										// { a.s, a.e, b.s, b.e }
				else {											// a.e > b.s
					var aebeSign = a.End.CompareTo ( b.End );
					if ( aebeSign <= 0 )						// a.e <= b.e
						r1 = Create ( a.Start, b.Start );		// { a.s, b.s, a.e, b.e }
					else {										// a.e > b.e
						r1 = Create ( a.Start, b.Start );		// { a.s, b.s, b.e, a.e }
						r2 = Create ( b.End, a.End );
					}
				}
			} else {											// a.s > b.s
				var asbeSign = a.Start.CompareTo ( b.End );
				if ( asbeSign == 0 ) {							// a.s == b.e
					if ( a.IsPoint )							// a.s == a.e
						r1 = null;								// { b.s, b.e==a.s==a.e }
					else
						r1 = a;									// { b.s, b.e, a.s, a.e }
				} else if ( asbeSign > 0 )						// a.s > b.e
					r1 = a;										// { b.s, b.e, a.s, a.e }
				else {											// a.s < b.e
					var aebeSign = a.End.CompareTo ( b.End );
					if ( aebeSign <= 0 )						// a.e <= b.e
						r1 = null;								// { b.s, a.s, a.e, b.e }
					else										// a.e > b.e
						r1 = Create ( b.End, a.End );			// { b.s, a.s, b.e, a.e }
				}
			}
		}
	}
}
