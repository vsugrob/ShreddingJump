using UnityEngine;

[RequireComponent ( typeof ( SpriteRenderer ) )]
public class SpriteRendererColorizer : SimpleColorizer {
    private SpriteRenderer _renderer;
	public SpriteRenderer Renderer => _renderer ?? ( _renderer = GetComponent <SpriteRenderer> () );

	public override bool SetColor ( Color color, MaterialSubstitutionCache cache ) {
		var renderer = Renderer;
		if ( renderer == null )
			return	false;

		renderer.color = color;
		return	true;
	}
}
