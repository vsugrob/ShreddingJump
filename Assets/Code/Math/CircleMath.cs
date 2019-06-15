﻿namespace System {
	public static class CircleMath {
		public static void CoerceArc (
			Range <float> arc, float pi2,
			out Range <float> resultArc1, out Range <float>? resultArc2
		) {
			arc.Order ();
			CoerceArcOrdered ( arc, pi2, out resultArc1, out resultArc2 );
		}

		public static void CoerceArcOrdered (
			Range <float> arc, float pi2,
			out Range <float> resultArc1, out Range <float>? resultArc2
		) {
			resultArc2 = null;
			arc = CoerceArcToClosestBase ( arc, pi2, out var splitIsPossible );
			if ( !splitIsPossible ) {
				resultArc1 = arc;
				return;
			}

			if ( arc.End <= pi2 ) {
				resultArc1 = Range.Create ( arc.Start, arc.End );
			} else {
				resultArc1 = Range.Create ( 0, arc.End - pi2 );
				resultArc2 = Range.Create ( arc.Start, pi2 );
			}
		}

		private static Range <float> CoerceArcToClosestBase ( Range <float> arc, float pi2, out bool splitIsPossible ) {
			var start = arc.Start;
			var end = arc.End;
			var diff = end - start;
			if ( diff >= pi2 ) {
				splitIsPossible = false;
				return	Range.Create ( 0, pi2 );
			} else if ( diff == 0 ) {
				splitIsPossible = false;
				return	new Range <float> ( CoerceAngle ( start, pi2 ) );
			}

			splitIsPossible = true;
			var baseAngle = FindClosestCircleBase ( start, pi2 );
			return	arc.Shift ( -baseAngle );
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

		public static void CoerceArc (
			Range <float> arcEnds, int dir, float pi2,
			out Range <float> resultArc1, out Range <float>? resultArc2
		) {
			var diff = arcEnds.Width ();
			if ( diff != 0 ) {
				if ( dir == 0 )
					throw new ArgumentException ( "Non-point arc cannot be represented with zero winding.", nameof ( dir ) );

				var absDiff = Math.Abs ( diff );
				if ( absDiff < pi2 ) {
					dir = Math.Sign ( dir );
					if ( Math.Sign ( diff ) != dir ) {
						var outerArc = pi2 - absDiff;
						arcEnds.End = arcEnds.Start + outerArc * dir;
					}
				}
			}

			CoerceArc ( arcEnds, pi2, out resultArc1, out resultArc2 );
		}
	}
}
