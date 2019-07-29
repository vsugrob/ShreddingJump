#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class EditorGUILayoutExt {
	public static int DrawSortingLayersPopup ( string label, int sortingLayerId ) {
		return	DrawSortingLayersPopup ( new GUIContent ( label ), sortingLayerId );
	}

	public static int DrawSortingLayersPopup ( GUIContent label, int sortingLayerId ) {
		var layers = SortingLayer.layers;
		var names = layers.Select ( l => new GUIContent ( l.name ) ).ToArray ();
		if ( !SortingLayer.IsValid ( sortingLayerId ) )
			sortingLayerId = layers [0].id;

		var layerValue = SortingLayer.GetLayerValueFromID ( sortingLayerId );
		var newLayerValue = EditorGUILayout.Popup ( label, layerValue, names );
		return	layers [newLayerValue].id;
	}
}
#endif // UNITY_EDITOR
