using UnityEngine;

[RequireComponent ( typeof ( Camera ) )]
public class CameraBackgroundColorizer : Colorizer {
    private Camera _camera;
	public Camera Camera => _camera ?? ( _camera = GetComponent <Camera> () );

	public override bool SetColor ( Color color, MaterialSubstitutionCache cache ) {
		var camera = Camera;
		if ( camera == null )
			return	false;

		camera.backgroundColor = color;
		camera.clearFlags = CameraClearFlags.SolidColor;
		return	true;
	}
}
