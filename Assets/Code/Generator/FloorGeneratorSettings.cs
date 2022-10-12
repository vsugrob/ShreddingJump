using UnityEngine;

[CreateAssetMenu]
public class FloorGeneratorSettings : ScriptableObject {
	[Header ( "Style" )]
	[SerializeField]
	private GeneratorStyleSettings _style;
	public GeneratorStyleSettings Style {
		get => _tmpStyle ?? _style;
		set => _tmpStyle = value;
	}
	private GeneratorStyleSettings _tmpStyle;
	// TODO: remove. This is temporary, just to check style variance.
	[SerializeField]
	private GeneratorStyleSettings _style1;
	public GeneratorStyleSettings Style1 {
		get => _style1;
		set => _style1 = value;
	}
	// TODO: remove. This is temporary, just to check style variance.
	[SerializeField]
	private GeneratorStyleSettings _style2;
	public GeneratorStyleSettings Style2 {
		get => _style2;
		set => _style2 = value;
	}
	// TODO: remove. This is temporary, just to check style variance.
	[SerializeField]
	private GeneratorStyleSettings _style3;
	public GeneratorStyleSettings Style3 {
		get => _style3;
		set => _style3 = value;
	}
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
