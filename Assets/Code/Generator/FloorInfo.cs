public class FloorInfo {
	public FloorRoot FloorRoot { get; set; }
	public PlatformCircle PlatformCircle { get; set; } = new PlatformCircle ();
	public PlatformCircle ObstacleCircle { get; set; } = new PlatformCircle ();

	public FloorInfo ( FloorRoot floorRoot, PlatformCircle platformCircle, PlatformCircle obstacleCircle ) {
		this.FloorRoot = floorRoot;
		this.PlatformCircle = platformCircle;
		this.ObstacleCircle = obstacleCircle;
	}
}
