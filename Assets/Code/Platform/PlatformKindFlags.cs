using System;

[Flags]
public enum PlatformKindFlags {
	None			= 0,
	Platform		= 1 << 0,
	Hole			= 1 << 1,
	KillerObstacle	= 1 << 2,
	Wall			= 1 << 3,
	Main			= 1 << 4,
	All				= -1,
}
