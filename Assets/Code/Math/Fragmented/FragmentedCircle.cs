namespace System.Collections.Generic {
	public static class FragmentedCircle {
		public static FragmentedCircle <TElement> CreateDegrees <TElement> () {
			return	new FragmentedCircle <TElement> ( 360 );
		}

		public static FragmentedCircle <TElement> CreateDegrees <TElement> ( IEnumerable <LineFragment <TElement, float>> fragments ) {
			var circle = new FragmentedCircle <TElement> ( 360 );
			circle.AddRange ( fragments );
			return	circle;
		}

		public static FragmentedCircle <TElement> CreateRadians <TElement> () {
			return	new FragmentedCircle <TElement> ( MathHelper.Pi2 );
		}

		public static FragmentedCircle <TElement> CreateRadians <TElement> ( IEnumerable <LineFragment <TElement, float>> fragments ) {
			var circle = new FragmentedCircle <TElement> ( MathHelper.Pi2 );
			circle.AddRange ( fragments );
			return	circle;
		}
	}
}
