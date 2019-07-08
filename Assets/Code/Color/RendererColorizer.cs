using System.Collections.Generic;
using UnityEngine;

[RequireComponent ( typeof ( Renderer ) )]
public class RendererColorizer : MonoBehaviour {
	[SerializeField]
	private ColorRole _role = ColorRole.Unknown;
	public ColorRole Role {
		get => _role;
		set => _role = value;
	}
    private Renderer _renderer;
	public Renderer Renderer => _renderer ?? ( _renderer = GetComponent <Renderer> () );
	private static List <RendererColorizer> colorizers = new List <RendererColorizer> ();

	public bool SetColor ( IReadOnlyDictionary <ColorRole, HsvColor> palette, MaterialSubstitutionCache cache ) {
		if ( !palette.TryGetValue ( Role, out var hsvColor ) )
			return	false;

		return	SetColor ( ( Color ) hsvColor, cache );
	}

	public bool SetColor ( Color color, MaterialSubstitutionCache cache ) {
		var renderer = Renderer;
		if ( renderer == null )
			return	false;

		var material = renderer.sharedMaterial;
		if ( material == null )
			return	false;
		material = cache.GetMaterial ( material );
		material.color = color;
		return	true;
	}

	public static void SetColors ( Transform rootTransform, IReadOnlyDictionary <ColorRole, HsvColor> palette, MaterialSubstitutionCache cache ) {
		rootTransform.GetComponentsInChildren ( includeInactive : true, colorizers );
		for ( int i = 0 ; i < colorizers.Count ; i++ ) {
			colorizers [i].SetColor ( palette, cache );
		}

		colorizers.Clear ();
	}

	public static void SetColors ( Transform rootTransform, IReadOnlyDictionary <ColorRole, HsvColor> palette, GameObject materialCacheContainer ) {
		var cache = MaterialSubstitutionCache.GetInstance ( materialCacheContainer );
		SetColors ( rootTransform, palette, cache );
	}
}
