using UnityEngine;

[RequireComponent ( typeof ( TrailRendererColorizer ) )]
public class TrailRendererColorizer : Colorizer {
    private TrailRenderer _renderer;
	public TrailRenderer Renderer => _renderer ?? ( _renderer = GetComponent <TrailRenderer> () );

	public override bool SetColor ( Color color, MaterialSubstitutionCache cache ) {
		var renderer = Renderer;
		if ( renderer == null )
			return	false;

		renderer.startColor = color;
		var ec = color;
		ec.a = renderer.endColor.a;
		renderer.endColor = ec;
		return	true;
	}
}
