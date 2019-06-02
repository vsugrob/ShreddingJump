using UnityEditor;

public static class PrefabHelper {
	public static SerializedProperty GetPrefabProperty ( SerializedProperty property ) {
		var prefab = PrefabUtility.GetCorrespondingObjectFromSource ( property.serializedObject.targetObject );
		if ( prefab == null )
			return	null;

		var prefabSerializedObject = new SerializedObject ( prefab );
		var prefabProperty = prefabSerializedObject.FindProperty ( property.propertyPath );
		return	prefabProperty;
	}

	public static SerializedProperty GetPrefabPropertyOrSelf ( SerializedProperty property ) {
		return	GetPrefabProperty ( property ) ?? property;
	}
}
