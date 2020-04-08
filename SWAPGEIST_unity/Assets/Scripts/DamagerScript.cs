using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerScript : MonoBehaviour {

public bool swish;
public bool destroyOnCollission;
public bool deactivateAfterTime;
public bool destroyAfterTime;
public bool projectile;
public float lifeTime;
public float projectileSpeed;

public bool createParticle;
public GameObject creationParticle;
public bool destroyParticle;
public GameObject destructionParticle;

private float destroyTimer = 0;

void Awake()
{
	Invoke("Particles", 0.1f);
}

void OnEnable()
{
	if(destroyAfterTime == true)
	Destroy(gameObject, lifeTime);
	if(deactivateAfterTime == true)
	Invoke("Deactivate", lifeTime);
}
void Update()
{
	MoveForward();
	SwishIt();

	destroyTimer += Time.deltaTime;
}

void MoveForward()
{
	if(projectile == true)
	transform.position += transform.forward * Time.deltaTime * projectileSpeed;
}

void SwishIt()
{
	if(swish)
	transform.Rotate(new Vector3(0,0,Random.Range(-3, 3)) * 8f, Space.World);
}

void Deactivate()
{
	gameObject.SetActive(false);
}

void DestroyBullet()
{
	projectileSpeed = 0f;
	Destroy(gameObject, 0.1f);
}

void Particles()
{
	if(createParticle == true)
	Instantiate(creationParticle, gameObject.transform.position, Quaternion.identity);
	if(destroyParticle == true)
	Instantiate(destructionParticle, gameObject.transform.position, Quaternion.identity);
}

void OnDestroy()
{
	Particles();
}

void OnTriggerEnter(Collider col)
{
	if(destroyOnCollission && destroyTimer >= 0.7f)
	{
		Invoke("DestroyBullet", 0.1f);
	}
	
}

}
