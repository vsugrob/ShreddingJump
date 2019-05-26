using UnityEngine;

[CreateAssetMenu]
public class LevelGeneratorSettings : ScriptableObject {
	[SerializeField]
	private float _minFloorHeight = 3;
	public float MinFloorHeight => _minFloorHeight;
	[SerializeField]
	private float _maxFloorHeight = 4.5f;
	public float MaxFloorHeight => _maxFloorHeight;
}
