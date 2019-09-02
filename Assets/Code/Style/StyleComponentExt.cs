using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StyleComponentExt {
	public static IEnumerable <StyleDistance <TComponent>> CalculateStyleDistance <TComponent> ( this IEnumerable <TComponent> components, GeneratorStyleSettings reference )
		where TComponent : Component
	{
		return	components.Select ( c => StyleDistance.Create ( c, StyleComponent.Distance ( reference, c ) ) );
	}
}

public struct StyleDistance <TComponent>
	where TComponent : Component
{
	public TComponent Component;
	public float Distance;

	public StyleDistance ( TComponent component, float distance ) {
		this.Component = component;
		this.Distance = distance;
	}

	public override string ToString () {
		return	$"({Component}, Distance: {Distance})";
	}
}

public static class StyleDistance {
	public static StyleDistance <TComponent> Create <TComponent> ( TComponent component, float distance )
		where TComponent : Component
	{
		return	new StyleDistance <TComponent> ( component, distance );
	}
}
