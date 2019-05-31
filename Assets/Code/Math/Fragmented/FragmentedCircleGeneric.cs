namespace System.Collections.Generic {
	public class FragmentedCircle <TElement> : FragmentedLine <TElement, float> {
		public FragmentedCircle ( float pi2 ):
			base ( 0, pi2 )
		{}

		public override void Add ( TElement element, Range <float> range ) {
			CoerceRange ( range, out var range1, out var range2 );
			base.Add ( element, range1 );
			if ( range2.HasValue )
				base.Add ( element, range2.Value );
		}

		private void CoerceRange ( Range <float> range, out Range <float> range1, out Range <float>? range2 ) {
			range2 = null;
			var start = range.Start;
			var end = range.End;
			var diff = end - start;
			if ( Math.Abs ( diff ) >= MaxLimit ) {
				range1 = Range.Create ( 0, MaxLimit );
				return;
			} else if ( diff == 0 ) {
				range1 = new Range <float> ( start );
				return;
			}

			var dirPositive = diff > 0;
			start = CoerceAngle ( start );
			end = CoerceAngle ( end );
			var diff2 = end - start;
			// diff2 cannot be 0, it's been already checked that original diff less than full round.
			var dirPositive2 = diff2 > 0;
			if ( dirPositive != dirPositive2 ) {
				if ( dirPositive ) {
					range1 = Range.Create ( 0, end );
					range2 = Range.Create ( start, MaxLimit );
					return;
				} else {
					range1 = Range.Create ( 0, start );
					range2 = Range.Create ( end, MaxLimit );
					return;
				}
			}

			range1 = Range.Ordered ( start, end );
		}

		private float CoerceAngle ( float angle ) {
			if ( angle == MaxLimit )
				return	angle;

			angle = angle % MaxLimit;
			if ( angle < 0 )
				angle += MaxLimit;

			return	angle;
		}
	}
}
