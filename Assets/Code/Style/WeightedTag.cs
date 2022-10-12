using System;

[Serializable]
public struct WeightedTag {
	public string Name;
	public float Weight;

	public WeightedTag ( string name, float weight ) {
		this.Name = name;
		this.Weight = weight;
	}

	public static bool operator == ( WeightedTag a, WeightedTag b ) => a.Name == b.Name && a.Weight == b.Weight;
	public static bool operator != ( WeightedTag a, WeightedTag b ) => a.Name != b.Name || a.Weight != b.Weight;

	public override bool Equals ( object obj ) {
		if ( obj is null )
			return	false;

		var other = ( WeightedTag ) obj;
		return	Name == other.Name && Weight == other.Weight;
	}

	public override int GetHashCode () {
		return	HashHelper.CombineHashes (
			Name != null ? Name.GetHashCode () : 0,
			Weight.GetHashCode ()
		);
	}

	public override string ToString () {
		return	$"({Name}, {Weight:0.##})";
	}
}
