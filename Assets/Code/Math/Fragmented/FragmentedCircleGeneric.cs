namespace System.Collections.Generic {
	public class FragmentedCircle <TElement> : FragmentedLine <TElement, float> {
		public FragmentedCircle ( float minLimit, float maxLimit ):
			base ( minLimit, maxLimit )
		{}

		public override void Add ( TElement element, Range <float> range ) {
			CoerceRange ( ref range );
			base.Add ( element, range );
		}

		private void CoerceRange ( ref Range <float> range ) {
			// TODO: preserve inner and outer arcs!
			range.Start = CoerceAngle ( range.Start );
			range.End = CoerceAngle ( range.End );
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
