﻿using System;

[Flags]
public enum PlatformKindFlags {
	None			= 0,
	/// <summary>Denotes any horizontal platform, whether it's just a platform or killer obstacle.</summary>
	Platform		= 1 << 0,
	/// <summary>Hole in the ground leading to a next floor.</summary>
	Hole			= 1 << 1,
	/// <summary>Marks platform or wall as an obstacle that kills character upon contact.</summary>
	KillerObstacle	= 1 << 2,
	/// <summary>Denotes any tall platform.</summary>
	Wall			= 1 << 3,
	/// <summary>Marks object as main on the floor. Currently used to distinct main hole from secondary ones.</summary>
	Main			= 1 << 4,
	All				= -1,
}
