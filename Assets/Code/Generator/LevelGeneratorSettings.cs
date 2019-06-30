using UnityEngine;

public abstract class LevelGeneratorSettings : ScriptableObject {
	[Header ( "Floor" )]
	[SerializeField]
	private float _floorHeightMin = 3;
	public float FloorHeightMin {
		get => _floorHeightMin;
		set => _floorHeightMin = value;
	}
	[SerializeField]
	private float _floorHeightMax = 4.5f;
	public float FloorHeightMax {
		get => _floorHeightMax;
		set => _floorHeightMax = value;
	}
	[Header ( "Base angle offset" )]
	[SerializeField]
	private float _baseAngleOffsetMin = -22.5f;
	public float BaseAngleOffsetMin {
		get => _baseAngleOffsetMin;
		set => _baseAngleOffsetMin = value;
	}
	[SerializeField]
	private float _baseAngleOffsetMax = 22.5f;
	public float BaseAngleOffsetMax {
		get => _baseAngleOffsetMax;
		set => _baseAngleOffsetMax = value;
	}
	[SerializeField]
	private float _baseAngleOffsetStep = 22.5f;
	public float BaseAngleOffsetStep {
		get => _baseAngleOffsetStep;
		set => _baseAngleOffsetStep = value;
	}
}
