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

	public static MaterialSubstitutionCache GetInstance ( Transform containerParentTf ) {
		var cache = containerParentTf.GetComponentInChildren <MaterialSubstitutionCache> ();
		if ( cache != null ) return	cache;
		var container = new GameObject ( nameof ( MaterialSubstitutionCache ) );
		container.transform.SetParent ( containerParentTf, worldPositionStays : true );
		cache = container.AddComponent <MaterialSubstitutionCache> ();
		return	cache;
	}
}
