using System;

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
	/// <summary>
	/// Character won't be able to pass through this object, either because it's too tall to jump over,
	/// or it triggers some action that kills character.
	/// </summary>
	Unpassable		= 1 << 5,
	/// <summary>Bumping into this platform finishes level.</summary>
	FinishLine		= 1 << 6,
	All				= -1,
}
