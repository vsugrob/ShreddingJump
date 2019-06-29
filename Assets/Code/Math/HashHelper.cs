using System.Collections.Generic;

namespace System {
	public static class HashHelper {
		#region 2 elements
		public static int GetHashCodeValType <TElement> ( TElement x, TElement y )
			where TElement : struct
		{
			int xHash = x.GetHashCode ();
			int yHash = y.GetHashCode ();

			return	CombineHashes ( xHash, yHash );
		}

		public static int GetHashCodeRefType <TElement> ( TElement x, TElement y )
			where TElement : class
		{
			int xHash = object.ReferenceEquals ( x, null ) ? 0 : x.GetHashCode ();
			int yHash = object.ReferenceEquals ( y, null ) ? 0 : y.GetHashCode ();

			return	CombineHashes ( xHash, yHash );
		}

		public static int CombineHashes ( int xHash, int yHash ) {
			return	xHash ^
				( ( yHash << 16 ) | ( int ) ( ( uint ) yHash >> 16 ) );
		}
		#endregion 2 elements

		#region 3 elements
		public static int GetHashCodeValType <TElement> (
			TElement x, TElement y, TElement z
		)
			where TElement : struct
		{
			int xHash = x.GetHashCode ();
			int yHash = y.GetHashCode ();
			int zHash = z.GetHashCode ();

			return	CombineHashes ( xHash, yHash, zHash );
		}

		public static int GetHashCodeRefType <TElement> (
			TElement x, TElement y, TElement z
		)
			where TElement : class
		{
			int xHash = object.ReferenceEquals ( x, null ) ? 0 : x.GetHashCode ();
			int yHash = object.ReferenceEquals ( y, null ) ? 0 : y.GetHashCode ();
			int zHash = object.ReferenceEquals ( z, null ) ? 0 : z.GetHashCode ();

			return	CombineHashes ( xHash, yHash, zHash );
		}

		public static int CombineHashes ( int xHash, int yHash, int zHash ) {
			return	xHash ^
				( ( yHash << 10 ) | ( int ) ( ( uint ) yHash >> 22 ) ) ^
				( ( zHash << 20 ) | ( int ) ( ( uint ) zHash >> 12 ) );
		}
		#endregion 3 elements

		#region Arbitrary number of elements
		public static int GetHashCodeValType <TElement> ( IEnumerable <TElement> values )
			where TElement : struct
		{
			int hash = 0;

			foreach ( var value in values ) {
				hash = ( hash << 1 ) | ( int ) ( ( uint ) hash >> 31 );
				hash ^= value.GetHashCode ();
			}

			return	hash;
		}

		public static int GetHashCodeRefType <TElement> ( IEnumerable <TElement> values )
			where TElement : class
		{
			int hash = 0;

			foreach ( var value in values ) {
				hash = ( hash << 1 ) | ( int ) ( ( uint ) hash >> 31 );
				hash ^= object.ReferenceEquals ( value, null ) ? 0 : value.GetHashCode ();
			}

			return	hash;
		}

		public static int GetHashCodeValType <TElement> ( IList <TElement> values )
			where TElement : struct
		{
			int hash = 0;

			for ( int i = 0 ; i < values.Count ; i++ ) {
				hash = ( hash << 1 ) | ( int ) ( ( uint ) hash >> 31 );
				var value = values [i];
				hash ^= value.GetHashCode ();
			}

			return	hash;
		}

		public static int GetHashCodeRefType <TElement> ( IList <TElement> values )
			where TElement : class
		{
			int hash = 0;

			for ( int i = 0 ; i < values.Count ; i++ ) {
				hash = ( hash << 1 ) | ( int ) ( ( uint ) hash >> 31 );
				var value = values [i];
				hash ^= object.ReferenceEquals ( value, null ) ? 0 : value.GetHashCode ();
			}

			return	hash;
		}

		public static int GetHashCodeCommutativeValType <TElement> ( IList <TElement> values )
			where TElement : struct
		{
			int hash = 0;

			for ( int i = 0 ; i < values.Count ; i++ ) {
				var value = values [i];
				hash ^= value.GetHashCode ();
			}

			return	hash;
		}

		public static int GetHashCodeCommutativeRefType <TElement> ( IList <TElement> values )
			where TElement : class
		{
			int hash = 0;

			for ( int i = 0 ; i < values.Count ; i++ ) {
				var value = values [i];
				hash ^= object.ReferenceEquals ( value, null ) ? 0 : value.GetHashCode ();
			}

			return	hash;
		}
		#endregion Arbitrary number of elements
	}
}
