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
	private AnimationCurve _distanceWeightCurve = new AnimationCurve ( new Keyframe ( 0, 1 ), new Keyframe ( 0.9f, 0 ) );
	public AnimationCurve DistanceWeightCurve {
		get => _distanceWeightCurve;
		set => _distanceWeightCurve = value;
	}
}
