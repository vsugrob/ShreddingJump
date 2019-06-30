using System.Collections.Generic;
using UnityEngine;

public abstract class LevelGenerator : MonoBehaviour {
	public abstract LevelGeneratorSettings BasicSettings { get; }
	[SerializeField]
	private PrefabDatabase _prefabDatabase;
	public PrefabDatabase PrefabDatabase {
		get => _prefabDatabase ?? ( _prefabDatabase = ScriptableObject.CreateInstance <PrefabDatabase> () );
		set => _prefabDatabase = value;
	}

	protected FloorInfo prevFloorInfo;
	protected PlatformCircle floorPlatformCircle, floorObstacleCircle;
	protected float floorHeight, floorY;
	protected Transform floorTf, platformContainerTf;

	public virtual IEnumerable <FloorInfo> Generate ( FloorInfo prevFloorInfo, int nextFloorIndex = 0 ) {
		this.prevFloorInfo = prevFloorInfo;
		floorHeight = Random.Range ( BasicSettings.FloorHeightMin, BasicSettings.FloorHeightMax );
		Debug.Log ( $"floorHeight: {floorHeight}." );
		var prevFloorTf = prevFloorInfo.FloorRoot.transform;
		floorY = prevFloorTf.position.y - floorHeight;
		var floorContainerTf = prevFloorTf.parent;
		var baseAngle = 0f;
		while ( true ) {
			var floorRoot = FloorRoot.Create ( floorContainerTf, nextFloorIndex, floorY );
			floorTf = floorRoot.transform;
			var platformsContainer = PlatformContainer.Create ( floorTf, baseAngle );
			platformContainerTf = platformsContainer.transform;
			ProcessPrevFloorInfo ( baseAngle );
			floorPlatformCircle = new PlatformCircle ();
			floorObstacleCircle = new PlatformCircle ();
			GenerateFloor ();
			var floorCompleteTriggerGo = Instantiate ( PrefabDatabase.FloorCompleteTrigger, floorTf );
			floorCompleteTriggerGo.transform.localPosition = Vector3.zero;
			this.prevFloorInfo = new FloorInfo ( floorRoot, baseAngle, floorPlatformCircle, floorObstacleCircle );
			floorY -= floorHeight;
			baseAngle += RandomHelper.Range ( BasicSettings.BaseAngleOffsetMin, BasicSettings.BaseAngleOffsetMax, BasicSettings.BaseAngleOffsetStep );
			nextFloorIndex++;
			yield return	this.prevFloorInfo;
		}
	}

	protected virtual void ProcessPrevFloorInfo ( float baseAngle ) {
		// Transform prev floor coordinates to match with current floor.
		var invOffset = prevFloorInfo.BaseAngle - baseAngle;
		prevFloorInfo.PlatformCircle.Shift ( invOffset );
		prevFloorInfo.ObstacleCircle.Shift ( invOffset );
	}

	protected abstract void GenerateFloor ();
	protected Column GenerateColumn () {
		var columns = PrefabDatabase.PredefinedColumns;
		if ( columns.Count == 0 ) {
			Debug.LogWarning ( $"No suitable column was found at {floorTf.name}." );
			return	null;
		}

		var prefab = columns [UnityEngine.Random.Range ( 0, columns.Count )];
		var column = Instantiate ( prefab, floorTf );
		var columnTf = column.transform;
		columnTf.localPosition = Vector3.zero;
		var scale = columnTf.localScale;
		scale.y = floorHeight / column.InitialHeight;
		columnTf.localScale = scale;
		return	column;
	}
}
