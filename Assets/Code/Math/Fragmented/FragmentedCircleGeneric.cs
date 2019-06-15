namespace System.Collections.Generic {
	public class FragmentedCircle <TElement> : FragmentedLine <TElement, float> {
		public FragmentedCircle ( float pi2 ):
			base ( 0, pi2 )
		{}

		public override void Add ( TElement element, Range <float> range ) {
			CircleMath.CoerceRange ( range, MaxLimit, out var range1, out var range2 );
			AddOrdered ( element, range1 );
			if ( range2.HasValue )
				AddOrdered ( element, range2.Value );
		}

		public void AddArc ( TElement element, float arcEnd1, float arcEnd2, int dir ) {
			AddArc ( element, Range.Create ( arcEnd1, arcEnd2 ), dir );
		}

		public void AddArc ( TElement element, Range <float> arcEnds, int dir ) {
			var diff = arcEnds.Width ();
			if ( diff != 0 ) {
				if ( dir == 0 )
					throw new ArgumentException ( "Non-point arc cannot be represented with zero winding.", nameof ( dir ) );

				var absDiff = Math.Abs ( diff );
				if ( absDiff < MaxLimit ) {
					dir = Math.Sign ( dir );
					if ( Math.Sign ( diff ) != dir ) {
						var outerArc = MaxLimit - absDiff;
						arcEnds.End = arcEnds.Start + outerArc * dir;
					}
				}
			}

			Add ( element, arcEnds );
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
