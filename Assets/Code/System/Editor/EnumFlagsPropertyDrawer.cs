using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using System;

[CustomPropertyDrawer ( typeof ( EnumFlagsAttribute ) )]
public partial class EnumFlagsPropertyDrawer : PropertyDrawer {
	/// <summary>
	/// Num toggle fields displayed in a row before continuing to a next line.
	/// </summary>
	private const int NumElementsPerRow = 2;

	private static string MakePrefKey ( SerializedProperty property ) {
		var targetObject = property.serializedObject.targetObject;

		// I'm pretty sure it can't evaluate to false ever.
		if ( targetObject != null ) {
			string typeName = targetObject.GetType ().Name;

			return	typeName + '.' + property.propertyPath;
		} else
			return	typeof ( EnumFlagsPropertyDrawer ).Name + "::" + property.propertyPath;
	}

	private Type EnumType {
		get {
			var attr = attribute as EnumFlagsAttribute;

			if ( attr != null && attr.DisplayedEnumType != null )
				return	attr.DisplayedEnumType;
			else
				return	fieldInfo.FieldType;
		}
	}

	private bool HideNoneField {
		get {
			var attr = attribute as EnumFlagsAttribute;

			return	attr != null && attr.HideNoneField;
		}
	}

	public override float GetPropertyHeight ( SerializedProperty property, GUIContent label ) {
		float height = GetFoldoutSize ( label ).y;
		bool isExpanded = EditorPrefs.GetBool ( MakePrefKey ( property ), false );

		if ( !isExpanded )
			return	height;

		float rowHeight = EditorHelper.GetLineHeight ( Styles.FlagStyle );
		int numElements;
		var values = Enum.GetValues ( EnumType );

		if ( HideNoneField ) {
			numElements = values
				.OfType <object> ()
				.Where ( value => ( int ) value != 0 )
				.Count ();
		} else
			numElements = values.Length;

		int numRows = GetNumRows ( numElements );
		height += numRows * rowHeight;

		return	height;
	}

	public override void OnGUI ( Rect pos, SerializedProperty property, GUIContent label ) {
		EditorHelper.SetupTooltipFromAttribute ( property, label );
		ulong propValue = ( ulong ) property.longValue;
		bool propValueOverridden = property.prefabOverride;
		var enumType = this.EnumType;
		var names = Enum.GetNames ( enumType );
		var values = GetEnumValuesUInt64 ( enumType );
		ulong allValue = 0;
		string noneName = null;
		string allName = null;

		/* Search for field representing zero,
		 * calculate value with all flags turned on. */
		for ( int i = 0 ; i < values.Length ; i++ ) {
			ulong enumFieldValue = values [i];
			allValue |= enumFieldValue;

			if ( enumFieldValue == 0 )
				noneName = names [i];
		}

		// Search for field representing all flags turned on.
		for ( int i = 0 ; i < values.Length ; i++ ) {
			ulong enumFieldValue = values [i];

			if ( enumFieldValue == allValue ) {
				allName = names [i];

				break;
			}
		}

		/* Remove field representing zero from names and values
		 * when it was requested. */
		bool hideNoneField = this.HideNoneField;

		if ( hideNoneField && noneName != null ) {
			var namesList = new List <string> ();
			var valuesList = new List <ulong> ();

			for ( int i = 0 ; i < values.Length ; i++ ) {
				ulong enumFieldValue = values [i];

				if ( enumFieldValue != 0 ) {
					namesList.Add ( names [i] );
					valuesList.Add ( values [i] );
				}
			}

			names = namesList.ToArray ();
			values = valuesList.ToArray ();
		}

		// Compose text representing flags that turned on.
		string valueText = FlagsToString (
			propValue,
			names, values,
			allValue,
			noneName, allName
		);

		// Append textual representation of the value to the property's tooltip.
		if ( !string.IsNullOrEmpty ( label.tooltip ) )
			label.tooltip += "\n\n";

		label.tooltip += valueText;

		var rect = GetFoldoutRect ( pos, label );
		bool isExpanded = EditorPrefs.GetBool ( MakePrefKey ( property ), false );

		// Display foldout bold when prefab value was overridden.
		var foldoutStyle = propValueOverridden ?
			Styles.GetOverriddenStyle ( Styles.FoldoutStyle ) :
			Styles.FoldoutStyle;
		EditorGUI.BeginChangeCheck ();
		isExpanded = EditorGUI.Foldout ( rect, isExpanded, label, toggleOnLabelClick : true, style : foldoutStyle );

		if ( EditorGUI.EndChangeCheck () )
			EditorPrefs.SetBool ( MakePrefKey ( property ), isExpanded );

		/* Place text area containing string representation of turned on flags
		 * at proper indentation from the left edge. */
		float indentLevelWidth = CalcIndentLevelWidth ();
		float outerIndentWidth = EditorGUI.indentLevel * indentLevelWidth;
		rect.x += EditorGUIUtility.labelWidth - outerIndentWidth;
		rect.width = pos.width - rect.x + outerIndentWidth;
		EditorGUI.TextArea ( rect, valueText );

		if ( !isExpanded )
			return;

		// Calculate top left position for the area of fields.
		rect.x = indentLevelWidth;
		rect.y += rect.height;
		// Width and height of every field is fixed.
		rect.width = ( pos.width - indentLevelWidth ) / NumElementsPerRow;
		float toggleHeight = EditorHelper.GetLineHeight ( Styles.FlagStyle );

		ulong newPropValue = propValue;
		ulong prefabPropValue = 0;

		// Try to retrieve value that is stored in prefab for the same property.
		if ( propValueOverridden ) {
			var prefabProperty = PrefabHelper.GetPrefabProperty ( property );

			if ( prefabProperty != null )
				prefabPropValue = ( ulong ) prefabProperty.longValue;
			else {
				/* Sometimes we're not able to locate prefab property.
				 * I haven't investigated deeply to know why. */
				prefabPropValue = propValue;
			}
		}

		/* Render enum fields as labeled checkboxes.
		 * Process changes caused by user actions and
		 * calculate new value for the property. */
		for ( int i = 0 ; i < values.Length ; i++ ) {
			// Does current row filled up?
			if ( ( i % NumElementsPerRow ) == 0 ) {
				// Move to the next row.
				if ( i != 0 ) {
					rect.x = indentLevelWidth;
					rect.y += toggleHeight;
				}
			} else {
				// Move to the next column.
				rect.x += rect.width;
			}

			string enumFieldName = names [i];
			ulong enumFieldValue = values [i];

			// Determine whether next checkbox must be rendered as checked.
			bool toggleChecked;

			if ( enumFieldValue == 0 )
				toggleChecked = propValue == 0;
			else
				toggleChecked = ( enumFieldValue & propValue ) == enumFieldValue;

			// Check whether value was changed as compared with prefab.
			bool fieldValueOverridden;

			if ( propValueOverridden ) {
				bool prefabToggleChecked;

				if ( enumFieldValue == 0 )
					prefabToggleChecked = prefabPropValue == 0;
				else
					prefabToggleChecked = ( enumFieldValue & prefabPropValue ) == enumFieldValue;

				fieldValueOverridden = toggleChecked != prefabToggleChecked;
			} else
				fieldValueOverridden = false;

			// Compose tooltip with flags that turned on and numerical value of the field.
			var fieldLabel = new GUIContent ( enumFieldName );
			bool isFlagField = IsPowerOf2 ( enumFieldValue );

			if ( !isFlagField ) {
				fieldLabel.tooltip = MaskToConstituentFlagsString (
					enumFieldValue,
					names, values,
					allValue,
					noneName, allName
				);
			} else
				fieldLabel.tooltip = enumFieldName;

			fieldLabel.tooltip += " = " + enumFieldValue.ToString ();

			// Setup style for the checkbox.
			var fieldStyle = isFlagField ? Styles.FlagStyle : Styles.MaskStyle;

			if ( fieldValueOverridden )
				fieldStyle = Styles.GetOverriddenStyle ( fieldStyle );

			bool newToggleChecked = EditorGUI.ToggleLeft ( rect, fieldLabel, toggleChecked, fieldStyle );
			bool checkStateChanged = newToggleChecked != toggleChecked;

			// Calculate how this field affects new value of the property.
			if ( checkStateChanged ) {
				if ( enumFieldValue == 0 ) {
					if ( newToggleChecked )
						newPropValue = 0;
				} else {
					if ( newToggleChecked )
						newPropValue |= enumFieldValue;
					else
						newPropValue &= ~enumFieldValue;
				}
			}
		}

		// Register undo and set object dirty.
		if ( newPropValue != propValue ) {
			var targetObjects = property.serializedObject.targetObjects;

#pragma warning disable 0618
			// Undo.RecordObject is not registering changes when type of the field is Int64.
			Undo.RegisterUndo ( targetObjects, property.displayName + " Flags change" );
#pragma warning restore 0618

			property.longValue = ( long ) newPropValue;
			EditorHelper.SetDirty ( targetObjects );
		}
	}

	// These are here to reduce amout of memory allocations.
	private static Dictionary <string, ulong> nameValues = new Dictionary <string, ulong> ();
	private static List <string> enabledFlagNames = new List <string> ();

	private static string FlagsToString (
		ulong propValue,
		string [] names, ulong [] values,
		ulong allValue,
		string noneName, string allName,
		ulong upperInclusiveLimit = ulong.MaxValue
	) {
		string valueText;

		if ( propValue == 0 )
			valueText = noneName != null ? noneName : "None";
		else if ( propValue == allValue )
			valueText = allName != null ? allName : "All";
		else {
			/* TODO: make cache for names and values of each inspected enum.
			 * It'll make everything lot faster. */
			nameValues.Clear ();

			for ( int i = 0 ; i < values.Length ; i++ ) {
				nameValues [names [i]] = values [i];
			}

			enabledFlagNames.Clear ();
			var topDownKvs = nameValues
				.Where ( kv => kv.Value <= upperInclusiveLimit )
				.OrderByDescending ( kv => kv.Value );
			ulong flags = propValue;

			foreach ( var kv in topDownKvs ) {
				ulong flag = kv.Value;

				if ( flag != 0 && ( flags & flag ) == flag ) {
					flags &= ~flag;
					enabledFlagNames.Insert ( 0, kv.Key );
				}
			}

			valueText = string.Join ( " | ", enabledFlagNames );
		}

		return	valueText;
	}

	private static string MaskToConstituentFlagsString (
		ulong propValue,
		string [] names, ulong [] values,
		ulong allValue,
		string noneName, string allName
	) {
		return	FlagsToString (
			propValue,
			names, values,
			allValue,
			noneName, allName,
			propValue - 1
		);
	}

	private static ulong [] GetEnumValuesUInt64 ( Type enumType ) {
		return	Enum.GetValues ( enumType )
			.OfType <object> ()
			.Select ( value => ChangeTypeToUInt64 ( value ) )
			.ToArray ();
	}

	private static ulong ChangeTypeToUInt64 ( object value ) {
		var type = value.GetType ().GetEnumUnderlyingType ();
		unchecked {
				 if ( type == typeof ( sbyte ) ) return	( ulong ) ( sbyte ) value;
			else if ( type == typeof ( short ) ) return	( ulong ) ( short ) value;
			else if ( type == typeof ( int   ) ) return	( ulong ) ( int   ) value;
			else if ( type == typeof ( long  ) ) return	( ulong ) ( long  ) value;
			else throw new InvalidCastException ( $"Cannot change type from {type} to {nameof ( UInt64 )}." );
		}
	}

	private static Vector2 GetFoldoutSize ( GUIContent label ) {
		float headerWidth = EditorGUIUtility.labelWidth;
		float headerHeight = Styles.FoldoutStyle.CalcHeight ( label, EditorGUIUtility.labelWidth );

		return	new Vector2 ( headerWidth, headerHeight );
	}

	private static Rect GetFoldoutRect ( Rect pos, GUIContent label ) {
		var size = GetFoldoutSize ( label );
		size.x = pos.width;

		return	new Rect ( pos.position, size );
	}

	private static int GetNumRows ( int numElements ) {
		return	Mathf.CeilToInt ( ( float ) numElements / NumElementsPerRow );
	}

	/// <summary>
	/// Calculate width corresponding to one level of indentation caused
	/// by nesting fields.
	/// This is a very rough empirical estimation, it probably should
	/// be rewritten.
	/// </summary>
	/// <returns>Indent with in pixels.</returns>
	private static float CalcIndentLevelWidth () {
		var style = EditorStyles.label;
		var padding = EditorStyles.label.padding;
		float twoCharWidth = style.CalcSize ( new GUIContent ( "AZ" ) ).x;
		float paddingWidth = padding.left + padding.right;

		return	twoCharWidth - paddingWidth;
	}

	private static bool IsPowerOf2 ( ulong value ) {
		return	( value & ( value - 1 ) ) == 0;
	}
}
