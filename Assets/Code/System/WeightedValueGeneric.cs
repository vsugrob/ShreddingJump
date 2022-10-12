using System;

[Serializable]
public struct WeightedValue <TValue> {
	public TValue Value;
	public float Weight;

	public WeightedValue ( TValue value, float weight ) {
		this.Value = value;
		this.Weight = weight;
	}

	public static bool operator == ( WeightedValue <TValue> a, WeightedValue <TValue> b ) => Equals ( a.Value, b.Value ) && a.Weight == b.Weight;
	public static bool operator != ( WeightedValue <TValue> a, WeightedValue <TValue> b ) => !Equals ( a.Value, b.Value ) || a.Weight != b.Weight;

	public override bool Equals ( object obj ) {
		if ( obj is null )
			return	false;

		var other = ( WeightedValue <TValue> ) obj;
		return	Equals ( Value, other.Value ) && Weight == other.Weight;
	}

	public override int GetHashCode () {
		return	HashHelper.CombineHashes (
			Value != null ? Value.GetHashCode () : 0,
			Weight.GetHashCode ()
		);
	}

	public override string ToString () {
		return	$"({Value}, {Weight:0.##})";
	}
}
