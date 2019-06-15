namespace System.Collections.Generic {
	public class FragmentedCircle <TElement> : FragmentedLine <TElement, float> {
		public FragmentedCircle ( float pi2 ):
			base ( 0, pi2 )
		{}

		public override void Add ( TElement element, Range <float> arc ) {
			CircleMath.CoerceArc ( arc, MaxLimit, out var range1, out var range2 );
			AddOrdered ( element, range1 );
			if ( range2.HasValue )
				AddOrdered ( element, range2.Value );
		}

		public void Add ( TElement element, float arcEnd1, float arcEnd2, int dir ) {
			Add ( element, Range.Create ( arcEnd1, arcEnd2 ), dir );
		}

		public void Add ( TElement element, Range <float> arcEnds, int dir ) {
			CircleMath.CoerceArc ( arcEnds, dir, MaxLimit, out var range1, out var range2 );
			AddOrdered ( element, range1 );
			if ( range2.HasValue )
				AddOrdered ( element, range2.Value );
		}

		public void Shift ( float offset ) {
			var fragments = new LineFragment <TElement, float> [fragmentsByStart.Count];
			fragmentsByStart.Values.CopyTo ( fragments, 0 );
			Clear ();
			for ( int i = 0 ; i < fragments.Length ; i++ ) {
				var frag = fragments [i];
				Add ( frag.Element, frag.Range.Shift ( offset ) );
			}
		}

		public override int SeekFragmentBoundary ( float start, int dir, out float boundary ) {
			if ( start != MaxLimit )
				start = CircleMath.CoerceAngle ( start, MaxLimit );

			return	base.SeekFragmentBoundary ( start, dir, out boundary );
		}

		public override int SeekClosestFragmentIndex ( float point, int dir ) {
			if ( point != MaxLimit )
				point = CircleMath.CoerceAngle ( point, MaxLimit );

			int index = base.SeekClosestFragmentIndex ( point, dir );
			if ( index >= 0 )
				return	index;

			if ( dir > 0 )
				return	base.SeekClosestFragmentIndex ( MinLimit, dir );
			else if ( dir < 0 )
				return	base.SeekClosestFragmentIndex ( MaxLimit, dir );
			else
				return	-1;
		}
	}
}
