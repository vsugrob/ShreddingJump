namespace System.Collections.Generic {
	public class FragmentedCircle <TElement> : FragmentedLine <TElement, float> {
		public FragmentedCircle ( float pi2 ):
			base ( 0, pi2 )
		{}

		public override void Add ( TElement element, Range <float> range ) {
			CoerceRange ( range, out var range1, out var range2 );
			AddOrdered ( element, range1 );
			if ( range2.HasValue )
				AddOrdered ( element, range2.Value );
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
				range1 = new Range <float> ( CoerceAngle ( start ) );
				return;
			}

			MathHelper.SortMinMax ( ref start, ref end );
			var baseAngle = FindClosestCircleBase ( start );
			start -= baseAngle;
			end -= baseAngle;
			if ( end <= MaxLimit ) {
				range1 = Range.Create ( start, end );
			} else {
				range1 = Range.Create ( 0, end - MaxLimit );
				range2 = Range.Create ( start, MaxLimit );
			}
		}

		private float CoerceAngle ( float angle ) {
			angle = angle % MaxLimit;
			if ( angle < 0 )
				angle += MaxLimit;

			return	angle;
		}

		private float FindClosestCircleBase ( float angle ) {
			var divInt = ( int ) ( angle / MaxLimit );
			var b = divInt * MaxLimit;
			if ( b > angle ) {
				// It's possible when angle is negative. Calculate base value, that is less than angle.
				b -= MaxLimit;
			}

			return	b;
		}
	}
}
