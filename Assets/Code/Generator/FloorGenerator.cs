using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class FloorGenerator : MonoBehaviour {
	public abstract FloorGeneratorSettings BasicSettings { get; }
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

	public virtual IEnumerable <FloorInfo> Generate ( IEnumerable <FloorInfo> prevGeneratorOutput ) {
		if ( prevGeneratorOutput is null )
			throw new ArgumentNullException ( nameof ( prevGeneratorOutput ) );

		FloorInfo lastFloorInfo = null;
		foreach ( var floorInfo in prevGeneratorOutput ) {
			yield return	floorInfo;
			lastFloorInfo = floorInfo;
		}

		if ( lastFloorInfo is null )
			throw new InvalidOperationException ( "Previous level generator returned empty sequence of floors." );

		foreach ( var floorInfo in Generate ( lastFloorInfo ) ) {
			yield return	floorInfo;
		}
	}

	public virtual IEnumerable <FloorInfo> Generate ( FloorInfo prevFloorInfo, int nextFloorIndex = 0 ) {
		this.prevFloorInfo = prevFloorInfo;
		floorHeight = UnityEngine.Random.Range ( BasicSettings.FloorHeightMin, BasicSettings.FloorHeightMax );
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

		var prefab = columns.TakeRandomByStyleProximity ( BasicSettings.Style );
		var column = Instantiate ( prefab, floorTf );
		var columnTf = column.transform;
		columnTf.localPosition = Vector3.zero;
		var scale = columnTf.localScale;
		scale.y = floorHeight / column.InitialHeight;
		columnTf.localScale = scale;
		return	column;
	}

	[ContextMenu ( "Evaluate chances" )]
	private void EvaluateChances () {
		EvaluateChances (
			"Platforms 45°",
			PrefabDatabase
				.PredefinedPlatforms
				.MatchFlags ( PlatformKindFlags.Platform )
				.WidthBetween ( 45, 45 )
		);
		EvaluateChances (
			"Obstacles 45°",
			PrefabDatabase
				.PredefinedPlatforms
				.MatchFlags ( PlatformKindFlags.KillerObstacle | PlatformKindFlags.Platform )
				.WidthBetween ( 45, 45 )
		);
		EvaluateChances (
			"Walls 22.5°",
			PrefabDatabase
				.PredefinedPlatforms
				.MatchFlags ( PlatformKindFlags.KillerObstacle | PlatformKindFlags.Wall )
				.WidthBetween ( 22.5f, 22.5f )
		);
		EvaluateChances (
			"Columns",
			PrefabDatabase.PredefinedColumns
		);
	}

	private void EvaluateChances <TComponent> ( string header, IEnumerable <TComponent> elements, int numTakes = 50 )
		where TComponent : Component
	{
		var elementsArray = elements.ToArray ();
		var style = BasicSettings.Style;
		var styleDistances = new Dictionary <TComponent, float> ();
		foreach ( var sd in elementsArray.CalculateStyleDistance ( style, StyleComponent.ManhattanAverageDistance ) ) {
			styleDistances [sd.Component] = sd.Distance;
		}

		var counters = new Dictionary <TComponent, int> ();
		var totalNumTakes = elementsArray.Length * numTakes;
		for ( int i = 0 ; i < totalNumTakes ; i++ ) {
			var component = elementsArray.TakeRandomByStyleProximity ( style );
			counters.TryGetValue ( component, out var count );
			counters [component] = count + 1;
		}

		var elementRarities = styleDistances
			.OrderBy ( kv => kv.Value )
			.Select ( kv => {
				counters.TryGetValue ( kv.Key, out var counter );
				return	new { Element = kv.Key, Distance = kv.Value, Percent = ( float ) counter / totalNumTakes };
			} );
		var sb = new StringBuilder ();
		sb.AppendLine ( $"{header}, elements: {elementsArray.Length}" );
		foreach ( var er in elementRarities ) {
			sb.AppendLine ( $"  {er.Percent * 100:##}% {er.Element.name}, Distance: {er.Distance}" );
		}

		Debug.Log ( sb );
	}
}
