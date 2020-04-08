using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour {

public int highScore;
public int currentScore;
public TextMeshProUGUI currentScoreText;
public TextMeshProUGUI theHighScoreText;

private GameObject[] getCount;
public int currentAmountOfEnemies;
public int enemyCap;

private DataController dataController;
private PlayerProgress playerProgress;
private GameObject player;

	void Awake () 
	{
		player = GameObject.Find("PLAYER");
		dataController = FindObjectOfType<DataController>();
		currentScore = 0;
		//highScore = PlayerPrefs.GetInt("highScore");
	}
	
	void Update () 
	{
		showCurrentScore();
		CheckEnemyAm();
		GameOver();
		//updateHighScore();
	}

	void showCurrentScore()
	{
		currentScoreText.text = currentScore.ToString();
	}

	void updateHighScore()
	{
		if (currentScore > highScore)
		highScore = currentScore;

		if(highScore == currentScore)
		currentScore = 0;
	}

	public void CheckEnemyAm()
	{
		getCount = GameObject.FindGameObjectsWithTag ("enemy");
		currentAmountOfEnemies = getCount.Length;
	}

	void GameOver()
	{
		if(player.gameObject == null)
		return;
		
		if (player.gameObject != null)
		{
			dataController.SubmitNewPlayerScore (currentScore);
			theHighScoreText.text = "High Score: " + dataController.GetHighestPlayerScore().ToString();
		}

		/*PlayerPrefs.SetInt("highScore", highScore);
		PlayerPrefs.Save();*/
	}
}
