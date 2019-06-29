using UnityEngine;

[CreateAssetMenu]
public class LevelGeneratorSettings : ScriptableObject {
	[Header ( "Floor" )]
	[SerializeField]
	private float _floorHeightMin = 3;
	public float FloorHeightMin => _floorHeightMin;
	[SerializeField]
	private float _floorHeightMax = 4.5f;
	public float FloorHeightMax => _floorHeightMax;
	[Header ( "Platforms" )]
	[SerializeField]
	private float _platformWidthMin = 22.5f;
	public float PlatformWidthMin => _platformWidthMin;
	[Header ( "Holes" )]
	[SerializeField]
	private float _mainHoleWidthMin = 45;
	public float MainHoleWidthMin => _mainHoleWidthMin;
	[SerializeField]
	private float _mainHoleWidthMax = 90;
	public float MainHoleWidthMax => _mainHoleWidthMax;
	[SerializeField]
	private float _secondaryHoleWidthMin = 22.5f;
	public float SecondaryHoleWidthMin => _secondaryHoleWidthMin;
	[SerializeField]
	private float _spaceBetweenHolesMin = 22.5f;
	public float SpaceBetweenHolesMin => _spaceBetweenHolesMin;
	[SerializeField]
	private float _totalHoleWidthMax = 180;
	public float TotalHoleWidthMax => _totalHoleWidthMax;
	[SerializeField]
	private float _holeWidthStep = 22.5f;
	public float HoleWidthStep => _holeWidthStep;
	[SerializeField]
	private int _holeCountMin = 1;
	public int HoleCountMin => _holeCountMin;
	[SerializeField]
	private int _holeCountMax = 3;
	public int HoleCountMax => _holeCountMax;
	[Header ( "Base angle offset" )]
	[SerializeField]
	private float _baseAngleOffsetMin = -22.5f;
	public float BaseAngleOffsetMin => _baseAngleOffsetMin;
	[SerializeField]
	private float _baseAngleOffsetMax = 22.5f;
	public float BaseAngleOffsetMax => _baseAngleOffsetMax;
	[SerializeField]
	private float _baseAngleOffsetStep = 22.5f;
	public float BaseAngleOffsetStep => _baseAngleOffsetStep;
	[Header ( "Obstacles(horizontal and walls)" )]
	[SerializeField]
	private float _obstacleWidthMin = 22.5f;
	public float ObstacleWidthMin => _obstacleWidthMin;
	[SerializeField]
	private float _obstacleWidthStep = 22.5f;
	public float ObstacleWidthStep => _obstacleWidthStep;
	[SerializeField]
	private float _horzObstacleWidthMax = 45;
	public float HorzObstacleWidthMax => _horzObstacleWidthMax;
	[SerializeField]
	private float _wallWidthMax = 22.5f;
	public float WallWidthMax => _wallWidthMax;
	[SerializeField]
	private float _totalObstacleWidthMax = 135;
	public float TotalObstacleWidthMax => _totalObstacleWidthMax;
	[SerializeField]
	private float _minSpaceBetweenObstacles = 22.5f;
	public float MinSpaceBetweenObstacles => _minSpaceBetweenObstacles;
	[SerializeField]
	private int _obstacleCountMin = 0;
	public int ObstacleCountMin => _obstacleCountMin;
	[SerializeField]
	private int _obstacleCountMax = 4;
	public int ObstacleCountMax => _obstacleCountMax;
	[SerializeField]
	private float _wallObstacleChance = 0.3f;
	public float WallObstacleChance => _wallObstacleChance;
	[SerializeField]
	private int _wallCountMax = 3;
	public int WallCountMax => _wallCountMax;
	[SerializeField]
	private float _unpassableWallObstacleChance = 0.2f;
	public float UnpassableWallObstacleChance => _unpassableWallObstacleChance;
	[SerializeField]
	private int _unpassableWallCountMax = 1;
	public int UnpassableWallCountMax => _unpassableWallCountMax;
	[SerializeField]
	private float _obstacleOverHoleChance = 0.3f;
	public float ObstacleOverHoleChance => _obstacleOverHoleChance;
	[SerializeField]
	private float _obstacleOverHoleMinResidualWidth = 22.5f;
	public float ObstacleOverHoleMinResidualWidth => _obstacleOverHoleMinResidualWidth;
	[Header ( "Safe Zone" )]
	[SerializeField]
	private float _safeZoneShrinkMin = 0;
	public float SafeZoneShrinkMin => _safeZoneShrinkMin;
	[SerializeField]
	private float _safeZoneShrinkMax = 67.5f;
	public float SafeZoneShrinkMax => _safeZoneShrinkMax;
	[SerializeField]
	private float _safeZoneShrinkStep = 22.5f;
	public float SafeZoneShrinkStep => _safeZoneShrinkStep;
	[SerializeField]
	private float _safeZoneMinWidth = 22.5f;
	public float SafeZoneMinWidth => _safeZoneMinWidth;
	[Header ( "Moving obstacles" )]
	[SerializeField]
	private float _horzObstacleOverPlatformMovingChance = 0.5f;
	public float HorzObstacleOverPlatformMovingChance => _horzObstacleOverPlatformMovingChance;
	[SerializeField]
	private float _wallOverPlatformMovingChance = 0.05f;
	public float WallOverPlatformMovingChance => _wallOverPlatformMovingChance;
	[SerializeField]
	private float _unpassableWallOverPlatformMovingChance = 0;
	public float UnpassableWallOverPlatformMovingChance => _unpassableWallOverPlatformMovingChance;
	[SerializeField]
	private float _movingObstacleAngularSpeedMin = 45;
	public float MovingObstacleAngularSpeedMin => _movingObstacleAngularSpeedMin;
	[SerializeField]
	private float _movingObstacleAngularSpeedMax = 135;
	public float MovingObstacleAngularSpeedMax => _movingObstacleAngularSpeedMax;
	[SerializeField]
	private AnimationCurve _movingObstacleMotionCurve = new AnimationCurve ( new Keyframe ( 0, 0 ), new Keyframe ( 1, 1 ) );
	public AnimationCurve MovingObstacleMotionCurve {
		get => _movingObstacleMotionCurve;
		set => _movingObstacleMotionCurve = value;
	}
	[SerializeField]
	private float _movingObstacleMinOscillationTime = 0.5f;
	public float MovingObstacleMinOscillationTime {
		get => _movingObstacleMinOscillationTime;
		set => _movingObstacleMinOscillationTime = value;
	}
}
