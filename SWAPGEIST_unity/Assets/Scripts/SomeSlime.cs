using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeSlime : MonoBehaviour {

public float lifeTime;


void Update()
{
	Destroy(gameObject, lifeTime);
	Wiggle();
	//Shrink();
	Grow();
}

void Wiggle()
{
	transform.position = new Vector3(transform.position.x + Random.RandomRange(-0.1f, 0.1f), transform.position.y, transform.position.z + Random.RandomRange(-0.1f, 0.1f));
}

void Shrink()
{
	//transform.localScale -= new Vector3(1f, 1f, 1f) * Time.deltaTime;
	gameObject.transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, 0, 0), Time.deltaTime*0.05f);
}

void Grow()
{
	//transform.localScale -= new Vector3(1f, 1f, 1f) * Time.deltaTime;
	gameObject.transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(7, 7, 7), Time.deltaTime* 10f);
}

}
