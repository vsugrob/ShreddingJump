namespace System {
	/// <summary>
	/// Represents range between [<see cref="Range{T}.Start"/>; <see cref="Range{T}.End"/>], includes both ends.
	/// Point represented by range with coincident ends.
	/// Cannot represent empty range. For this purpose consider using Nullable&lt;Range&lt;T&gt;&gt; or another data structure.
	/// </summary>
	[Serializable]
	public class RangeSingle : Range <float> {
		public RangeSingle (): base () {}
		public RangeSingle ( float point ): base ( point ) {}
		public RangeSingle ( float start, float end ): base ( start, end ) {}
	}
}
