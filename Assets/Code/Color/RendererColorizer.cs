using UnityEngine;

[RequireComponent ( typeof ( Renderer ) )]
public class RendererColorizer : ColorizerSingle {
	[SerializeField]
	private string _colorPropertyName = "_Color";
	public string ColorPropertyName {
		get => _colorPropertyName;
		set => _colorPropertyName = value;
	}
    private Renderer _renderer;
	public Renderer Renderer => _renderer ?? ( _renderer = GetComponent <Renderer> () );

	public override bool SetColor ( Color color, MaterialSubstitutionCache cache ) {
		var renderer = Renderer;
		if ( renderer == null )
			return	false;

		var material = renderer.sharedMaterial;
		if ( material == null )
			return	false;
		material = cache.GetMaterial ( material );
		material.SetColor ( ColorPropertyName, color );
		renderer.sharedMaterial = material;
		return	true;
	}
}
