using UnityEngine;

[RequireComponent ( typeof ( TrailRendererColorizer ) )]
public class TrailRendererColorizer : Colorizer {
    private TrailRenderer _renderer;
	public TrailRenderer Renderer => _renderer ?? ( _renderer = GetComponent <TrailRenderer> () );
	[SerializeField]
	private bool _setStartColor = true;
	public bool SetStartColor {
		get => _setStartColor;
		set => _setStartColor = value;
	}
	[SerializeField]
	private bool _setEndColor = true;
	public bool SetEndColor {
		get => _setEndColor;
		set => _setEndColor = value;
	}

	public override bool SetColor ( Color color, MaterialSubstitutionCache cache ) {
		var renderer = Renderer;
		if ( renderer == null )
			return	false;

		if ( SetStartColor )
			renderer.startColor = MixWithExistingAlpha ( color, renderer.startColor );

		if ( SetEndColor )
			renderer.endColor = MixWithExistingAlpha ( color, renderer.endColor );

		return	true;
	}

	private static Color MixWithExistingAlpha ( Color newColor, Color existingColor ) {
		newColor.a = existingColor.a;
		return	newColor;
	}
}
