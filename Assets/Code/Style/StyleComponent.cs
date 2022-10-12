using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class StyleComponent : MonoBehaviour {
	[SerializeField]
	private List <WeightedTag> _tags = new List <WeightedTag> ();
	public List <WeightedTag> Tags => _tags;
	// TODO: extract to some kind of HierarchyHelper.
	public static StyleComponent GetOrCreate ( GameObject gameObject ) {
		var component = gameObject.GetComponent <StyleComponent> ();
		if ( component == null )
			component = gameObject.AddComponent <StyleComponent> ();

		return	component;
	}

	public bool TryGetTagWeight ( string name, out float weight ) {
		for ( int i = 0 ; i < Tags.Count ; i++ ) {
			var tag = Tags [i];
			if ( tag.Name == name ) {
				weight = tag.Weight;
				return	true;
			}
		}

		weight = 0;
		return	false;
	}

	public static float DistanceSq ( GeneratorStyleSettings reference, StyleComponent subject ) {
		var refTags = reference.Tags;
		var sqSum = 0f;
		for ( int i = 0 ; i < refTags.Count ; i++ ) {
			var t1 = refTags [i];
			subject.TryGetTagWeight ( t1.Name, out var w2 );
			var dw = t1.Weight - w2;
			sqSum += dw * dw;
		}

		return	sqSum;
	}

	public static float Distance ( GeneratorStyleSettings reference, StyleComponent subject ) {
		return	Mathf.Sqrt ( DistanceSq ( reference, subject ) );
	}

	public static float Distance ( GeneratorStyleSettings reference, Component component ) {
		var subject = GetOrCreate ( component.gameObject );
		return	Distance ( reference, subject );
	}

	public static float ManhattanAverageDistance ( GeneratorStyleSettings reference, StyleComponent subject ) {
		var refTags = reference.Tags;
		var sum = 0f;
		for ( int i = 0 ; i < refTags.Count ; i++ ) {
			var t1 = refTags [i];
			subject.TryGetTagWeight ( t1.Name, out var w2 );
			var dw = t1.Weight - w2;
			sum += Mathf.Abs ( dw );
		}

		return	sum / refTags.Count;
	}

	public static float ManhattanAverageDistance ( GeneratorStyleSettings reference, Component component ) {
		var subject = GetOrCreate ( component.gameObject );
		return	ManhattanAverageDistance ( reference, subject );
	}
}
