using UnityEngine;

public static class StyleDistance {
	public static StyleDistance <TComponent> Create <TComponent> ( TComponent component, float distance )
		where TComponent : Component
	{
		return	new StyleDistance <TComponent> ( component, distance );
	}
}