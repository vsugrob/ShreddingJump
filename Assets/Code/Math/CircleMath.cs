namespace System {
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
			arc = CoerceArcToClosestBase ( arc, pi2, out var splitIsPossible, out _ );
			if ( !splitIsPossible ) {
				resultArc1 = arc;
				return;
			}

			if ( arc.End <= pi2 ) {
				resultArc1 = RangeFactory.Create ( arc.Start, arc.End );
			} else {
				resultArc1 = RangeFactory.Create ( 0, arc.End - pi2 );
				resultArc2 = RangeFactory.Create ( arc.Start, pi2 );
			}
		}

		private static Range <float> CoerceArcToClosestBase ( Range <float> arc, float pi2, out bool splitIsPossible, out float baseAngle ) {
			var diff = arc.Width ();
			if ( diff >= pi2 ) {
				splitIsPossible = false;
				baseAngle = 0;
				return	RangeFactory.Create ( 0, pi2 );
			} else if ( diff == 0 ) {
				splitIsPossible = false;
				baseAngle = 0;
				return	new Range <float> ( CoerceAngle ( arc.Start, pi2 ) );
			}

			splitIsPossible = true;
			return	ShiftToClosestCircleBase ( arc, pi2, out baseAngle );
		}

		public static Range <float> ShiftToClosestCircleBase ( Range <float> arc, float pi2 ) {
			return	ShiftToClosestCircleBase ( arc, pi2, out _ );
		}

		public static Range <float> ShiftToClosestCircleBase ( Range <float> arc, float pi2, out float baseAngle ) {
			baseAngle = FindClosestCircleBase ( arc.Start, pi2 );
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
			arcEnds = ArcEndsToArc ( arcEnds, dir, pi2 );
			CoerceArc ( arcEnds, pi2, out resultArc1, out resultArc2 );
		}

		public static Range <float> ArcEndsToArc ( Range <float> arcEnds, int dir, float pi2 ) {
			var diff = arcEnds.Width ();
			if ( diff == 0 )
				return	arcEnds;
			else if ( dir == 0 )
				throw new ArgumentException ( "Non-point arc cannot be represented with zero winding.", nameof ( dir ) );

			var absDiff = Math.Abs ( diff );
			if ( absDiff >= pi2 )
				return	RangeFactory.Create ( 0, pi2 );

			dir = Math.Sign ( dir );
			if ( Math.Sign ( diff ) != dir ) {
				var outerArc = pi2 - absDiff;
				arcEnds.End = arcEnds.Start + outerArc * dir;
			}

			return	arcEnds;
		}

		public static void IntersectArcs (
			float pi2,
			Range <float> arcEnds1, int dir1,
			Range <float> arcEnds2, int dir2,
			out Range <float>? resultArc1, out Range <float>? resultArc2
		) {
			var arc1 = ArcEndsToArc ( arcEnds1, dir1, pi2 );
			var arc2 = ArcEndsToArc ( arcEnds2, dir2, pi2 );
			IntersectArcs (
				pi2,
				arc1, arc2,
				out resultArc1, out resultArc2
			);
		}

		public static void IntersectArcs (
			float pi2,
			Range <float> arc1, Range <float> arc2,
			out Range <float>? resultArc1, out Range <float>? resultArc2
		) {
			var diff1 = arc1.Width ();
			var diff2 = arc2.Width ();
			// Change to positive winding.
			if ( diff1 < 0 ) arc1.Reverse ();
			if ( diff2 < 0 ) arc2.Reverse ();
			var diff1Abs = Math.Abs ( diff1 );
			if ( diff1Abs >= pi2 ) {
				resultArc1 = CoerceArcToClosestBase ( arc2, pi2, out _, out _ );
				resultArc2 = null;
				return;
			}

			var diff2Abs = Math.Abs ( diff2 );
			if ( diff2Abs >= pi2 ) {
				resultArc1 = CoerceArcToClosestBase ( arc1, pi2, out _, out _ );
				resultArc2 = null;
				return;
			}
			// Shift and modulate to common base.
			var offset = -arc1.Start;
			arc1 = arc1.Shift ( offset );
			arc2 = arc2.Shift ( offset );
			CoerceArcOrdered ( arc2, pi2, out var arc21, out var arc22 );
			// Intersect.
			IntersectArcsNormalizedOrdered ( pi2, arc1, arc21, out resultArc1 );
			if ( arc22.HasValue ) {
				IntersectArcsNormalizedOrdered ( pi2, arc1, arc22.Value, out resultArc2 );
				if ( resultArc2.HasValue && !resultArc1.HasValue ) {
					resultArc1 = resultArc2;
					resultArc2 = null;
				}
			} else
				resultArc2 = null;
			// Remove redundant point.
			if ( resultArc1.HasValue && resultArc2.HasValue ) {
				if ( resultArc1.Value.IsPoint ) {
					if ( resultArc2.Value.Contains ( resultArc1.Value.Start ) ) {
						resultArc1 = resultArc2;
						resultArc2 = null;
					}
				} else if ( resultArc2.Value.IsPoint && resultArc1.Value.Contains ( resultArc2.Value.Start ) )
					resultArc2 = null;
			}
			// Shift back.
			if ( resultArc1.HasValue ) {
				resultArc1 = resultArc1.Value.Shift ( -offset );
				if ( resultArc2.HasValue )
					resultArc2 = resultArc2.Value.Shift ( -offset );
			}
			// Coerce to closest base.
			if ( resultArc1.HasValue ) {
				resultArc1 = CoerceArcToClosestBase ( resultArc1.Value, pi2, out _, out _ );
				if ( resultArc2.HasValue ) {
					resultArc2 = CoerceArcToClosestBase ( resultArc2.Value, pi2, out _, out _ );
					// Order can be different after rebase.
					if ( resultArc1.Value.Start > resultArc2.Value.Start )
						MathHelper.Swap ( ref resultArc1, ref resultArc2 );
				}
			}
		}

		private static void IntersectArcsNormalizedOrdered ( float pi2, Range <float> a, Range <float> b, out Range <float>? r ) {
			RangeFactory.IntersectOrdered ( a, b, out r );
			if ( r.HasValue )
				return;

			if ( ( ( a.Start == 0 || a.End == 0 ) && ( b.Start == pi2 || b.End == pi2 ) ) ||
				 ( ( b.Start == 0 || b.End == 0 ) && ( a.Start == pi2 || a.End == pi2 ) )
			) {
				r = RangeFactory.Point ( 0f );
			}
		}
	}
}
