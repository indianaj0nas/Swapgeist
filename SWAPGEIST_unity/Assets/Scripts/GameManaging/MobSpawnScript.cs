using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawnScript : MonoBehaviour {

public GameObject enemy1;
public GameObject enemy2;
public GameObject enemy3;
public int spawnAmount;
public GameObject ghost;
public float spawnTime = 5f;

private Vector3 spawnArea;
private float newSpawnTime;
private int pickRandom;

void Awake()
{
	//enemyCap = 10;
	newSpawnTime = 5f;
	ghost = GameObject.Find("Ghost");
	//InvokeRepeating("SpawnEnemy", spawnRate, spawnRate);
}

void Update()
{
	SpawnTimer();
}

public void SpawnEnemy()
{
	Vector3 spawnArea = new Vector3(transform.position.x + Random.Range(0, 10), transform.position.y, transform.position.z + Random.Range(0,10));
	//for (int i = 0; i < spawnAmount; i++)
        //{
			Instantiate(enemy1, spawnArea, Quaternion.identity);
			//Instantiate(enemy2, spawnArea, Quaternion.identity);
			//Instantiate(enemy3, spawnArea, Quaternion.identity);

			pickRandom = Random.Range(0, 100);

			if (pickRandom > 15)
			{
				Instantiate(enemy2, spawnArea, Quaternion.identity);
			}
			else {Instantiate(enemy3, spawnArea, Quaternion.identity);}
		//}

}

void SpawnTimer()
 {
	 	 GameObject gameManager = GameObject.Find("GameManager");
Score scoreScript = gameManager.GetComponent<Score>();

     spawnTime -= Time.deltaTime;
     if (spawnTime <= 0)
     {
		 //SpawnEnemy();
         //Instantiate(enemy1, spawnArea, Quaternion.identity);
         spawnTime = newSpawnTime;
	 }

	 if (scoreScript.currentAmountOfEnemies >= scoreScript.enemyCap)
	 {
		 spawnAmount = 0;
		 //newSpawnTime = 0;
	 }

if (scoreScript.currentScore <= 200 && scoreScript.currentAmountOfEnemies <= scoreScript.enemyCap)
{
	spawnAmount = 1;
	newSpawnTime = 20f;
	scoreScript.enemyCap = 10;
	SpawnEnemy();
}

else if (scoreScript.currentScore <= 800 && scoreScript.currentAmountOfEnemies <= scoreScript.enemyCap)
{
	spawnAmount = 5;
	newSpawnTime = 15f;
	scoreScript.enemyCap = 30;
	SpawnEnemy();
}

else if (scoreScript.currentScore <= 1500 && scoreScript.currentAmountOfEnemies <= scoreScript.enemyCap)
{
	spawnAmount = 10;
	newSpawnTime = 15f;
	scoreScript.enemyCap = 45;
	SpawnEnemy();
}

else if (scoreScript.currentScore <= 2500 && scoreScript.currentAmountOfEnemies <= scoreScript.enemyCap)
{
	spawnAmount = 15;
	newSpawnTime = 15f;
	scoreScript.enemyCap = 50;
	SpawnEnemy();
}

}
}