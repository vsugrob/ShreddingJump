using UnityEngine;

[CreateAssetMenu]
public class LevelGeneratorSettings : ScriptableObject {
	[SerializeField]
	private float _floorHeightMin = 3;
	public float FloorHeightMin => _floorHeightMin;
	[SerializeField]
	private float _floorHeightMax = 4.5f;
	public float FloorHeightMax => _floorHeightMax;
	[SerializeField]
	private float _platformWidthMin = 22.5f;
	public float PlatformWidthMin => _platformWidthMin;
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
	[SerializeField]
	private float _baseAngleOffsetMin = 0;
	public float BaseAngleOffsetMin => _baseAngleOffsetMin;
	[SerializeField]
	private float _baseAngleOffsetMax = 60;
	public float BaseAngleOffsetMax => _baseAngleOffsetMax;
	[SerializeField]
	private float _baseAngleOffsetStep = 22.5f;
	public float BaseAngleOffsetStep => _baseAngleOffsetStep;
}
