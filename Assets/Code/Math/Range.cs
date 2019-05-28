﻿using System.Collections.Generic;

namespace System {
	/// <summary>
	/// Represents range between [<see cref="Start"/>; <see cref="End"/>], includes both ends.
	/// Point represented by range with coincident ends.
	/// Cannot represent empty range. For this purpose consider using Nullable&lt;Range&lt;T&gt;&gt; or another data structure.
	/// </summary>
	/// <typeparam name="T">Type of range ends. Must implement <see cref="IComparable{T}"/>.</typeparam>
	[Serializable]
	public class Range <T>
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
		/// <summary>Returns true when <see cref="End"/> is equal to <see cref="Start"/>.</summary>
		public bool IsPoint => IsValid && Start.CompareTo ( End ) == 0;

		public Range () {
			this.Start = default;
			this.End = default;
		}

		public Range ( T soleElement ) {
			this.Start = soleElement;
			this.End = soleElement;
		}

		public Range ( T start, T end ) {
			this.Start = start;
			this.End = end;
		}

		public void Reverse () {
			var tmp = Start;
			Start = End;
			End = Start;
		}

		public void MakeOrdered () {
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

		public override bool Equals ( object obj ) {
			if ( ReferenceEquals ( this, obj ) )
				return	true;

			var other = obj as Range <T>;
			if ( ReferenceEquals ( other, null ) )
				return	false;

			if ( !this.IsValid || !other.IsValid )
				return	false;

			return	this.Start.Equals ( other.Start ) &&
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
