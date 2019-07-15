using System.Collections.Generic;
using UnityEngine;

public abstract class Colorizer : MonoBehaviour {
	private static List <Colorizer> colorizers = new List <Colorizer> ();
	public abstract bool SetColor ( IReadOnlyDictionary <ColorRole, HsvColor> palette, MaterialSubstitutionCache cache );
	public static void SetColors ( Transform rootTransform, IReadOnlyDictionary <ColorRole, HsvColor> palette, MaterialSubstitutionCache cache ) {
		rootTransform.GetComponentsInChildren ( includeInactive : true, colorizers );
		for ( int i = 0 ; i < colorizers.Count ; i++ ) {
			colorizers [i].SetColor ( palette, cache );
		}

		colorizers.Clear ();
	}

	public static void SetColors ( Transform rootTransform, IReadOnlyDictionary <ColorRole, HsvColor> palette, Transform materialCacheContainerParentTf ) {
		var cache = MaterialSubstitutionCache.GetInstance ( materialCacheContainerParentTf );
		SetColors ( rootTransform, palette, cache );
	}
}
