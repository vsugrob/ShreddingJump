using System.Collections.Generic;
using UnityEngine;

public abstract class ColorizerSingle : Colorizer {
	[SerializeField]
	private ColorRole _role = ColorRole.Unknown;
	public ColorRole Role {
		get => _role;
		set => _role = value;
	}

	public override bool SetColor ( IReadOnlyDictionary <ColorRole, HsvColor> palette, MaterialSubstitutionCache cache ) {
		if ( !palette.TryGetValue ( Role, out var hsvColor ) )
			return	false;

		return	SetColor ( ( Color ) hsvColor, cache );
	}

	public abstract bool SetColor ( Color color, MaterialSubstitutionCache cache );
}
