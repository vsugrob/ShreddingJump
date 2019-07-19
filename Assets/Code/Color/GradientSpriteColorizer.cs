using System.Collections.Generic;
using UnityEngine;

[RequireComponent ( typeof ( SpriteRenderer ) )]
public class GradientSpriteColorizer : GradientColorizer {
	[SerializeField]
	private int _length = 10;
	public int Length {
		get => _length;
		set => _length = value;
	}
	[SerializeField]
	private float _pixelsPerUnit = 10;
	public float PixelsPerUnit {
		get => _pixelsPerUnit;
		set => _pixelsPerUnit = value;
	}
	private SpriteRenderer _spriteRenderer;
	private SpriteRenderer SpriteRenderer => _spriteRenderer ?? ( _spriteRenderer = GetComponent <SpriteRenderer> () );

	public override bool SetColor ( IReadOnlyDictionary <ColorRole, HsvColor> palette, MaterialSubstitutionCache cache ) {
		if ( !base.SetColor ( palette, cache ) || SpriteRenderer == null )
			return	false;
		else if ( Length < 1 ) {
			Debug.LogError ( $"{nameof ( Length )} must be positive. Actual value: {Length}." );
			return	false;
		} else if ( PixelsPerUnit < 0 ) {
			Debug.LogError ( $"{nameof ( PixelsPerUnit )} must be non-negative. Actual value: {PixelsPerUnit}." );
			return	false;
		}

		var tex = new Texture2D ( 1, Length );
		var grad = Gradient;
		float t = 0;
		float dt = 1f / ( Length - 1 );
		for ( int i = 0 ; i < Length ; i++, t += dt ) {
			tex.SetPixel ( 0, i, grad.Evaluate ( t ) );
		}

		tex.Apply ();
		tex.wrapMode = TextureWrapMode.Clamp;
		var sprite = Sprite.Create ( tex, new Rect ( 0, 0, tex.width, tex.height ), new Vector2 ( 0.5f, 0.5f ), PixelsPerUnit );
		SpriteRenderer.sprite = sprite;
		return	true;
	}
}