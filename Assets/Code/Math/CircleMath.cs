namespace System {
	public static class CircleMath {
		public static void CoerceRange (
			Range <float> range, float pi2,
			out Range <float> range1, out Range <float>? range2
		) {
			range2 = null;
			var start = range.Start;
			var end = range.End;
			var diff = end - start;
			if ( Math.Abs ( diff ) >= pi2 ) {
				range1 = Range.Create ( 0, pi2 );
				return;
			} else if ( diff == 0 ) {
				range1 = new Range <float> ( CoerceAngle ( start, pi2 ) );
				return;
			}

			MathHelper.SortMinMax ( ref start, ref end );
			var baseAngle = FindClosestCircleBase ( start, pi2 );
			start -= baseAngle;
			end -= baseAngle;
			if ( end <= pi2 ) {
				range1 = Range.Create ( start, end );
			} else {
				range1 = Range.Create ( 0, end - pi2 );
				range2 = Range.Create ( start, pi2 );
			}
		}

		public static float CoerceAngle ( float angle, float pi2 ) {
			angle = angle % pi2;
			if ( angle < 0 )
				angle += pi2;

			return	angle;
		}

		public static float FindClosestCircleBase ( float angle, float pi2 ) {
			var divInt = ( int ) ( angle / pi2 );
			var b = divInt * pi2;
			if ( b > angle ) {
				// It's possible when angle is negative. Calculate base value, that is less than angle.
				b -= pi2;
			}

			return	b;
		}
	}
}
