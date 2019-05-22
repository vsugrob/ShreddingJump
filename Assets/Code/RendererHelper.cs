using System.Collections.Generic;
using UnityEngine;

public static class RendererHelper {
	public static bool TryGetBBCenter ( IEnumerable <Renderer> renderers, out Vector3 center ) {
		center = Vector3.zero;
		int count = 0;
		foreach ( var renderer in renderers ) {
			center += renderer.bounds.center;
			count++;
		}

		if ( count == 0 )
			return	false;

		center /= count;
		return	true;
	}
}
