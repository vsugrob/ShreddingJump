/* Originally written by nickgravelyn.
 * https://github.com/nickgravelyn */
using UnityEngine;
using UnityEditor;

[CustomEditor ( typeof ( SortingLayerExposed ) )]
public class SortingLayerExposedEditor : Editor {
	public override void OnInspectorGUI () {
		// Get the renderer from the target object
		var renderer = ( target as SortingLayerExposed ).gameObject.GetComponent <Renderer> ();

		// If there is no renderer, we can't do anything
		if ( !renderer )
			return;

		// Expose the sorting layer ID.
		int newSortingLayerId = EditorGUILayoutExt.DrawSortingLayersPopup ( "Sorting Layer", renderer.sortingLayerID );
		if ( newSortingLayerId != renderer.sortingLayerID ) {
			Undo.RecordObject ( renderer, "Edit Sorting Layer." );
			renderer.sortingLayerID = newSortingLayerId;
			EditorUtility.SetDirty ( renderer );
		}

		// Expose the manual sorting order
		int newSortingLayerOrder = EditorGUILayout.IntField ( "Sorting Layer Order", renderer.sortingOrder );
		if ( newSortingLayerOrder != renderer.sortingOrder ) {
			Undo.RecordObject ( renderer, "Edit Sorting Order" );
			renderer.sortingOrder = newSortingLayerOrder;
			EditorUtility.SetDirty ( renderer );
		}
	}
}
