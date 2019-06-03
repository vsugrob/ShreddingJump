namespace System {
	public static class RangeFloatExt {
		public static float Width ( this Range <float> range ) {
			return	range.End - range.Start;
		}

		public static float WidthAbs ( this Range <float> range ) {
			return	Math.Abs ( range.End - range.Start );
		}

		public static Range <float> Add ( this Range <float> range, float offset ) {
			range.Start += offset;
			range.End += offset;
			return	range;
		}

		public static Range <float> Subtract ( this Range <float> range, float offset ) {
			range.Start -= offset;
			range.End -= offset;
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
	}
}
