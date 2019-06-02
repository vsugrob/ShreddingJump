using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class EnumFlagsAttribute : PropertyAttribute {
	public readonly Type DisplayedEnumType;
	public bool HideNoneField { get; set; }

	public EnumFlagsAttribute () {
		this.DisplayedEnumType = null;
	}

	public EnumFlagsAttribute ( Type displayedEnumType ) {
		this.DisplayedEnumType = displayedEnumType;
	}
 }