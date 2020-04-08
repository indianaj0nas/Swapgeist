using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour {
	
	void Awake() 
	{
		float randomScale = Random.Range(.5f, 1.5f);
		transform.localScale = new Vector3(randomScale, 0.01f, randomScale);
	}
}
