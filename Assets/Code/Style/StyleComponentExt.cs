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
