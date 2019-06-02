using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class EnumFlagsPropertyDrawer {
	private static class Styles {
		private static Dictionary <GUIStyle, GUIStyle> overriddenStyles =
			new Dictionary <GUIStyle, GUIStyle> ();

		public static GUIStyle GetOverriddenStyle ( GUIStyle normalStyle ) {
			GUIStyle overriddenStyle;

			if ( !overriddenStyles.TryGetValue ( normalStyle, out overriddenStyle ) ) {
				overriddenStyle = new GUIStyle ( normalStyle );
				overriddenStyle.fontStyle |= FontStyle.Bold;
				overriddenStyles [normalStyle] = overriddenStyle;
			}

			return	overriddenStyle;
		}

		private static GUIStyle _foldoutStyle;
		public static GUIStyle FoldoutStyle {
			get {
				if ( _foldoutStyle == null ) {
					_foldoutStyle = new GUIStyle ( EditorStyles.foldout );
					_foldoutStyle.clipping = TextClipping.Clip;
					_foldoutStyle.padding.right = 0;
					_foldoutStyle.margin.right = 0;
				}

				return	_foldoutStyle;
			}
		}

		private static GUIStyle _flagStyle;
		public static GUIStyle FlagStyle {
			get {
				if ( _flagStyle == null ) {
					_flagStyle = new GUIStyle ( EditorStyles.label );
				}

				return	_flagStyle;
			}
		}

		private static GUIStyle _maskStyle;
		public static GUIStyle MaskStyle {
			get {
				if ( _maskStyle == null ) {
					_maskStyle = new GUIStyle ( EditorStyles.label );
					_maskStyle.normal.textColor = new Color ( 0.05f, 0.25f, 0.05f );
				}

				return	_maskStyle;
			}
		}
	}
}