using System.Collections.Generic;
using UnityEngine;

public class MaterialSubstitutionCache : MonoBehaviour {
	private Dictionary <Material, Material> replacementsByOrig = new Dictionary <Material, Material> ();

	public Material GetMaterial ( Material originalSharedMaterial ) {
		if ( replacementsByOrig.TryGetValue ( originalSharedMaterial, out var replacement ) )
			return	replacement;

		replacement = Instantiate ( originalSharedMaterial );
		replacementsByOrig.Add ( originalSharedMaterial, replacement );
		return	replacement;
	}

	public static MaterialSubstitutionCache GetInstance ( GameObject container ) {
		var cache = container.GetComponent <MaterialSubstitutionCache> ();
		if ( cache != null ) return	cache;
		cache = container.AddComponent <MaterialSubstitutionCache> ();
		return	cache;
	}
}
