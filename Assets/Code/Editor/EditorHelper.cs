using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class EditorHelper {
	public static T GetAttribute <T> ( SerializedProperty property, bool inherit = true )
		where T : Attribute
	{
		var obj = property.serializedObject.targetObject;
		var fieldInfo = obj.GetType ().GetField ( property.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );

		if ( fieldInfo == null )
			return	null;

		var attrs = fieldInfo.GetCustomAttributes ( typeof ( T ), inherit ) as T [];

		if ( attrs == null || attrs.Length == 0 )
			return	null;

		return	attrs [0];
	}

	public static int BitMaskField ( Rect pos, int mask, System.Type enumType, GUIContent label ) {
		var itemNames = Enum.GetNames ( enumType );
		var itemValues = Enum.GetValues ( enumType ) as int [];
		int val = mask;
		int maskVal = 0;
		
		for ( int i = 0 ; i < itemValues.Length ; i++ ) {
			if ( itemValues [i] != 0 ) {
				if ( ( val & itemValues [i] ) == itemValues [i] )
					maskVal |= 1 << i;
			} else if ( val == 0 )
                 maskVal |= 1 << i;
		}

		int newMaskVal = EditorGUI.MaskField ( pos, label, maskVal, itemNames );
		int changes = maskVal ^ newMaskVal;
         
		for ( int i = 0 ; i < itemValues.Length ; i++ ) {
			if ( ( changes & ( 1 << i ) ) != 0 ) {				// has this list item changed?
				if ( ( newMaskVal & ( 1 << i ) ) != 0 ) {		// has it been set?
					if ( itemValues [i] == 0 ) {				// special case: if "0" is set, just set the val to 0
						val = 0;
						break;
					} else
						val |= itemValues [i];
				} else {										// it has been reset
					val &= ~itemValues [i];
				}
			}
		}

		return	val;
	}

	public static object GetPropertyLeafObject ( SerializedProperty property ) {
		var pathNodes = property.propertyPath.Split ( '.' );
		object obj = property.serializedObject.targetObject;
		int lastIdx = pathNodes.Length - 1;

		for ( int i = 0 ; i < lastIdx ; i++ ) {
			string pathNode = pathNodes [i];
			var fieldInfo = FindFieldInfoInHierarchy ( obj.GetType (), pathNode );
			obj = fieldInfo.GetValue ( obj );
		}

		return	obj;
	}

	public static FieldInfo FindFieldInfoInHierarchy ( Type type, string fieldName ) {
		FieldInfo fieldInfo = null;

		do {
			fieldInfo = type.GetField (
				fieldName,
				BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public
			);
		} while ( fieldInfo == null && null != ( type = type.BaseType ) );

		return	fieldInfo;
	}

	public static bool SetupTooltipFromAttribute ( SerializedProperty property, GUIContent guiContent ) {
		var ttAttr = GetAttribute <TooltipAttribute> ( property );

		if ( ttAttr == null ) {
			guiContent.tooltip = null;

			return	false;
		} else {
			guiContent.tooltip = ttAttr.tooltip;

			return	true;
		}
	}

	public static float GetLineHeight ( GUIStyle style ) {
		return	style.lineHeight + style.padding.top + style.padding.bottom;
	}

	public static void SetDirty ( IEnumerable <UnityEngine.Object> objects ) {
		foreach ( var obj in objects ) {
			EditorUtility.SetDirty ( obj );
		}
	}
}
