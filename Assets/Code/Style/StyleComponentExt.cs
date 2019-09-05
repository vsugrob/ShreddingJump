using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public static class StyleComponentExt {
	public static IEnumerable <StyleDistance <TComponent>> CalculateStyleDistance <TComponent> (
		this IEnumerable <TComponent> components, GeneratorStyleSettings reference,
		Func <GeneratorStyleSettings, Component, float> distanceFunc = null
	)
		where TComponent : Component
	{
		if ( distanceFunc == null ) distanceFunc = StyleComponent.Distance;
		return	components.Select ( c => StyleDistance.Create ( c, distanceFunc ( reference, c ) ) );
	}

	public static TComponent FindByStyle <TComponent> (
		this IEnumerable <TComponent> components, GeneratorStyleSettings reference,
		Func <GeneratorStyleSettings, Component, float> distanceFunc = null
	)
		where TComponent : Component
	{
		var all = components
			.CalculateStyleDistance ( reference, distanceFunc )
			.OrderBy ( sd => sd.Distance )
			.ToArray ();
		if ( all.Length == 0 )
			return	null;

		var last = all [all.Length - 1];
		var threshold = UnityRandom.value * Mathf.Clamp01 ( reference.DistanceCutoff ) * last.Distance;
		var equals = all
			.Where ( sd => sd.Distance <= threshold )
			.ToArray ();
		if ( equals.Length == 0 )
			return	GetClosest ( all );

		return	equals.TakeRandomSingleOrDefault ().Component;
	}

	private static TComponent GetClosest <TComponent> ( StyleDistance <TComponent> [] styleDistances )
		where TComponent : Component
	{
		if ( styleDistances.Length == 0 )
			return	null;

		var minDistance = styleDistances.Min ( sd => sd.Distance );
		var equals = styleDistances
			.Where ( sd => sd.Distance == minDistance )
			.ToArray ();
		return	equals.TakeRandomSingleOrDefault ().Component;
	}
}
