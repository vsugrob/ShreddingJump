using System.Collections.Generic;
using System.Linq;

public static class EnumerablePlatformExt {
	public static IEnumerable <Platform> RequireFlags ( this IEnumerable <Platform> platforms, PlatformKindFlags flags ) {
		return	platforms.Where ( p => ( p.Kind & flags ) == flags );
	}

	public static IEnumerable <Platform> MatchFlags ( this IEnumerable <Platform> platforms, PlatformKindFlags flags ) {
		return	platforms.Where ( p => p.Kind == flags );
	}

	public static IEnumerable <Platform> WidthBetween ( this IEnumerable <Platform> platforms, float minAngleWidth, float maxAngleWidth ) {
		return	platforms.Where ( p => CheckValueFits ( p.AngleWidth, minAngleWidth, maxAngleWidth ) );
	}

	public static IEnumerable <Platform> HeightBetween ( this IEnumerable <Platform> platforms, float minHeight, float maxHeight ) {
		return	platforms.Where ( p => CheckValueFits ( p.Height, minHeight, maxHeight ) );
	}

	private static bool CheckValueFits ( float value, float min, float max ) {
		return	min <= value && value <= max;
	}
}
