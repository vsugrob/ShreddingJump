using System.Collections.Generic;

namespace System {
	/// <summary>
	/// Represents range between [<see cref="Start"/>; <see cref="End"/>], includes both ends.
	/// Point represented by range with coincident ends.
	/// Cannot represent empty range. For this purpose consider using Nullable&lt;Range&lt;T&gt;&gt; or another data structure.
	/// </summary>
	/// <typeparam name="T">Type of range ends. Must implement <see cref="IComparable{T}"/>.</typeparam>
	[Serializable]
	public struct Range <T>
		where T : IComparable <T>
	{
		/// <summary>
		/// Starting value of the range.
		/// Can be greater or equal to <see cref="End"/>.</summary>
		public T Start { get; set; }
		/// <summary>
		/// Ending value of the range (inclusive).
		/// Not mandated to be greater than <see cref="Start"/>.
		/// </summary>
		public T End { get; set; }
		/// <summary>Range considered invalid when at least one of its ends set to null.</summary>
		public bool IsValid => !ReferenceEquals ( Start, null ) && !ReferenceEquals ( End, null );
		/// <summary>Returns true when <see cref="End"/> is less than <see cref="Start"/>.</summary>
		public bool IsReversed => IsValid && Start.CompareTo ( End ) > 0;
		/// <summary>Returns true when <see cref="Start"/> is less than or equal to <see cref="End"/>.</summary>
		public bool IsOrdered => IsValid && Start.CompareTo ( End ) <= 0;
		/// <summary>Returns true when <see cref="End"/> is equal to <see cref="Start"/>.</summary>
		public bool IsPoint => IsValid && Start.CompareTo ( End ) == 0;
		public Range <T> Reversed {
			get {
				var r = this;
				r.Reverse ();
				return	r;
			}
		}
		public Range <T> Ordered {
			get {
				var r = this;
				r.Order ();
				return	r;
			}
		}

		public Range ( T point ) {
			this.Start = point;
			this.End = point;
		}

		public Range ( T start, T end ) {
			this.Start = start;
			this.End = end;
		}

		public void Reverse () {
			var tmp = Start;
			Start = End;
			End = tmp;
		}

		public void Order () {
			if ( IsReversed )
				Reverse ();
		}

		public bool Contains ( T value ) {
			if ( !IsValid )
				return	false;

			if ( IsReversed )
				return	End.CompareTo ( value ) <= 0 && Start.CompareTo ( value ) >= 0;
			else
				return	 Start.CompareTo ( value ) <= 0 && End.CompareTo ( value ) >= 0;
		}

		public bool Contains ( Range <T> otherRange ) {
			return	IsValid && otherRange.IsValid &&
				Contains ( otherRange.Start ) &&
				Contains ( otherRange.End );
		}

		private bool ContainsAnyEnd ( Range <T> otherRange ) {
			return	Contains ( otherRange.Start ) ||
					Contains ( otherRange.End );
		}

		public bool Intersects ( Range <T> otherRange ) {
			return	IsValid && otherRange.IsValid &&
				( ContainsAnyEnd ( otherRange ) ||
					otherRange.ContainsAnyEnd ( this ) );
		}

		public bool Equivalent ( Range <T> otherRange ) {
			return	IsValid && otherRange.IsValid &&
				Start.CompareTo ( otherRange.Start ) == 0 &&
				End.CompareTo ( otherRange.End ) == 0;
		}

		public bool IntersectsWithAny ( IEnumerable <Range <T>> ranges ) {
			foreach ( var otherRange in ranges ) {
				if ( Intersects ( otherRange ) )
					return	true;
			}

			return	false;
		}

		#region Operators
		public static bool operator < ( Range <T> range, T point ) {
			return	range.Start.CompareTo ( point ) < 0 &&
					range.End.CompareTo ( point ) < 0;
		}

		public static bool operator > ( Range <T> range, T point ) {
			return	range.Start.CompareTo ( point ) > 0 &&
					range.End.CompareTo ( point ) > 0;
		}

		public static bool operator < ( T point, Range <T> range ) {
			return	point.CompareTo ( range.Start ) < 0 &&
					point.CompareTo ( range.End ) < 0;
		}

		public static bool operator > ( T point, Range <T> range ) {
			return	point.CompareTo ( range.Start ) > 0 &&
					point.CompareTo ( range.End ) > 0;
		}

		public static bool operator <= ( Range <T> range, T point ) {
			return	range.Start.CompareTo ( point ) <= 0 &&
					range.End.CompareTo ( point ) <= 0;
		}

		public static bool operator >= ( Range <T> range, T point ) {
			return	range.Start.CompareTo ( point ) >= 0 &&
					range.End.CompareTo ( point ) >= 0;
		}

		public static bool operator <= ( T point, Range <T> range ) {
			return	point.CompareTo ( range.Start ) <= 0 &&
					point.CompareTo ( range.End ) <= 0;
		}

		public static bool operator >= ( T point, Range <T> range ) {
			return	point.CompareTo ( range.Start ) >= 0 &&
					point.CompareTo ( range.End ) >= 0;
		}
		#endregion Operators

		public int CompareOrderedTo ( T point ) {
			var eSign = End.CompareTo ( point );
			if ( eSign < 0 )
				return	-1;
			else if ( eSign == 0 )
				return	IsPoint ? 0 : -1;

			var sSign = Start.CompareTo ( point );
			return	sSign >= 0 ? 1 : 0;
		}

		public override bool Equals ( object obj ) {
			if ( ReferenceEquals ( this, obj ) )
				return	true;

			if ( !( obj is Range <T> ) )
				return	false;

			var other = ( Range <T> ) obj;
			return	this.IsValid && other.IsValid &&
					this.Start.Equals ( other.Start ) &&
					this.End.Equals ( other.End );
		}

		public override int GetHashCode () {
			int xHash, yHash;
			if ( typeof ( T ).IsValueType ) {
				xHash = Start.GetHashCode ();
				yHash = End.GetHashCode ();
			} else {
				xHash = ReferenceEquals ( Start, null ) ? 0 : Start.GetHashCode ();
				yHash = ReferenceEquals ( End, null ) ? 0 : End.GetHashCode ();
			}

			return	HashHelper.CombineHashes ( xHash, yHash );
		}

		public override string ToString () {
			return	$"[{Start};{End}]";
		}
	}
}
