namespace System.Collections.Generic {
	public static class FragmentedCircle {
		public static FragmentedCircle <TElement> CreateDegrees <TElement> () {
			return	new FragmentedCircle <TElement> ( 360 );
		}

		public static FragmentedCircle <TElement> CreateRadians <TElement> () {
			return	new FragmentedCircle <TElement> ( MathHelper.Pi2 );
		}
	}
}
