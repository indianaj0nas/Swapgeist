using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {

	void Awake () 
	{
		Destroy(gameObject,3f);
	}
}
