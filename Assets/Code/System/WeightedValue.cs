public static class WeightedValue {
	public static WeightedValue <TValue> Create <TValue> ( TValue value, float weight ) {
		return	new WeightedValue <TValue> ( value, weight );
	}
}
