public static class MathHelper {
	public static bool StepTowards ( ref float value, float target, float step ) {
		var diff = target - value;
		bool stepIsExcessive = diff > 0 ? diff <= step : diff >= step;
		if ( stepIsExcessive )
			value = target;
		else
			value += step;

		return	stepIsExcessive;
	}
}
