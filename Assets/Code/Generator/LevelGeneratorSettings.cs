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
	[Header ( "Horizontal obstacles" )]
	[SerializeField]
	private float _horzObstacleWidthMin = 22.5f;
	public float HorzObstacleWidthMin => _horzObstacleWidthMin;
	[SerializeField]
	private float _horzObstacleWidthMax = 45;
	public float HorzObstacleWidthMax => _horzObstacleWidthMax;
	[SerializeField]
	private float _horzObstacleWidthStep = 22.5f;
	public float HorzObstacleWidthStep => _horzObstacleWidthStep;
	[SerializeField]
	private float _totalHorzObstacleWidthMax = 135;
	public float TotalHorzObstacleWidthMax => _totalHorzObstacleWidthMax;
	[SerializeField]
	private float _minSpaceBetweenHorzObstacles = 22.5f;
	public float MinSpaceBetweenHorzObstacles => _minSpaceBetweenHorzObstacles;
	[SerializeField]
	private int _horzObstacleCountMin = 0;
	public int HorzObstacleCountMin => _horzObstacleCountMin;
	[SerializeField]
	private int _horzObstacleCountMax = 4;
	public int HorzObstacleCountMax => _horzObstacleCountMax;
	[SerializeField]
	private bool _allowObstaclesUnderHoles = false;
	public bool AllowObstaclesUnderHoles => _allowObstaclesUnderHoles;
	[Header ( "Moving obstacles" )]
	[SerializeField]
	private float _obstacleOverPlatformMovingChance = 0.3f;
	public float ObstacleOverPlatformMovingChance => _obstacleOverPlatformMovingChance;
	[SerializeField]
	private float _movingObstacleAngularSpeedMin = 45;
	public float MovingObstacleAngularSpeedMin => _movingObstacleAngularSpeedMin;
	[SerializeField]
	private float _movingObstacleAngularSpeedMax = 135;
	public float MovingObstacleAngularSpeedMax => _movingObstacleAngularSpeedMax;
}
