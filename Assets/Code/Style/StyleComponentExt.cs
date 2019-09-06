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

	public static TComponent TakeRandomByStyleProximity <TComponent> ( this IEnumerable <TComponent> components, GeneratorStyleSettings reference )
		where TComponent : Component
	{
		var all = components
			.CalculateStyleDistance ( reference, StyleComponent.ManhattanAverageDistance )
			.ToArray ();
		if ( all.Length == 0 )
			return	null;

		var weightedComponents = all
			.Select ( sd => WeightedValue.Create ( sd.Component, reference.DistanceWeightCurve.Evaluate ( sd.Distance ) ) );
		var bestMatch = weightedComponents.TakeRandomSingleOrDefaultByWeight ();
		if ( !( bestMatch is null ) )
			return	bestMatch;

		var minDistance = all.Min ( sd => sd.Distance );
		return	all.Where ( e => e.Distance <= minDistance ).TakeRandomSingleOrDefault ().Component;
	}
}
