public class FloorInfo {
	public FloorRoot FloorRoot { get; set; }
	public float BaseAngle { get; set; }
	public PlatformCircle PlatformCircle { get; set; } = new PlatformCircle ();
	public PlatformCircle ObstacleCircle { get; set; } = new PlatformCircle ();

	public FloorInfo ( FloorRoot floorRoot, float baseAngle, PlatformCircle platformCircle, PlatformCircle obstacleCircle ) {
		this.FloorRoot = floorRoot;
		this.BaseAngle = baseAngle;
		this.PlatformCircle = platformCircle;
		this.ObstacleCircle = obstacleCircle;
	}
}
