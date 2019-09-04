using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GeneratorStyleSettings : ScriptableObject {
	[SerializeField]
	private List <WeightedTag> _tags = new List <WeightedTag> ();
	public List <WeightedTag> Tags {
		get => _tags;
		set => _tags = value;
	}
	[SerializeField]
	private float _distanceCutoff = 0.6f;
	public float DistanceCutoff {
		get => _distanceCutoff;
		set => _distanceCutoff = value;
	}
}
