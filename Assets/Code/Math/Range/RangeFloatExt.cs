﻿namespace System {
	public static class RangeFloatExt {
		public static float Width ( this Range <float> range ) {
			return	range.End - range.Start;
		}

		public static float WidthAbs ( this Range <float> range ) {
			return	Math.Abs ( range.End - range.Start );
		}

		public static float Middle ( this Range <float> range ) {
			return	( range.Start + range.End ) / 2;
		}

		public static Range <float> Shift ( this Range <float> range, float offset ) {
			range.Start += offset;
			range.End += offset;
			return	range;
		}

		public static Range <float> Multiply ( this Range <float> range, float multiplier ) {
			range.Start *= multiplier;
			range.End *= multiplier;
			return	range;
		}

		public static Range <float> Divide ( this Range <float> range, float dividend ) {
			range.Start /= dividend;
			range.End /= dividend;
			return	range;
		}

		public static Range <float> Grow ( this Range <float> range, float delta ) {
			if ( delta < 0 )
				return	range.Shrink ( -delta );

			if ( range.IsOrdered ) {
				range.Start -= delta;
				range.End += delta;
			} else {
				range.Start += delta;
				range.End -= delta;
			}

			return	range;
		}

		public static Range <float> Shrink ( this Range <float> range, float delta ) {
			if ( delta < 0 )
				return	range.Grow ( -delta );

			var width = range.Width ();
			var widthAbs = Math.Abs ( width );
			if ( widthAbs <= delta * 2 )
				delta = widthAbs / 2;

			if ( width >= 0 ) {
				range.Start += delta;
				range.End -= delta;
			} else {
				range.Start -= delta;
				range.End += delta;
			}

			return	range;
		}
	}
}
