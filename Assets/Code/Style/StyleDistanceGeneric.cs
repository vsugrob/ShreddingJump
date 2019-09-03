using UnityEngine;

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

