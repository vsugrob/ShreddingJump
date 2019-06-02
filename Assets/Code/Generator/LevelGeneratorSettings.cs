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
	private float _platformAngleWidthMin = 22.5f;
	public float PlatformAngleWidthMin => _platformAngleWidthMin;
	[SerializeField]
	private float _mainHoleAngleWidthMin = 45;
	public float MainHoleAngleWidthMin => _mainHoleAngleWidthMin;
	[SerializeField]
	private float _mainHoleAngleWidthMax = 90;
	public float MainHoleAngleWidthMax => _mainHoleAngleWidthMax;
	[SerializeField]
	private float _secondaryHoleAngleWidthMin = 22.5f;
	public float SecondaryHoleAngleWidthMin => _secondaryHoleAngleWidthMin;
	[SerializeField]
	private float _totalHoleAngleWidthMax = 180;
	public float TotalHoleAngleWidthMax => _totalHoleAngleWidthMax;
	[SerializeField]
	private float _holeAngleWidthStep = 22.5f;
	public float HoleAngleWidthStep => _holeAngleWidthStep;
	[SerializeField]
	private int _holeCountMin = 1;
	public int HoleCountMin => _holeCountMin;
	[SerializeField]
	private int _holeCountMax = 3;
	public int HoleCountMax => _holeCountMax;
}
