using System.Collections.Generic;
using UnityEngine;

public class StyleComponent : MonoBehaviour {
	[SerializeField]
	private List <WeightedTag> _tags = new List <WeightedTag> ();
	public List <WeightedTag> Tags => _tags;
}
