using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour {

	//public RoundData[] allRoundData;

	private PlayerProgress playerProgress;
	private float slowTimeTimer;

	void Start () 
	{
		DontDestroyOnLoad(gameObject);
		LoadPlayerProgress();

		SceneManager.LoadScene("MainMenu");
	}

	void Update()
	{
		CheckTimeScale();
	}

	/*public RoundData GetCurrentRoundData()
	{
		return allRoundData[0];
	}*/

	public void SubmitNewPlayerScore(int newScore)
	{
		if (newScore > playerProgress.highestScore)
		{
			playerProgress.highestScore = newScore;
			SavePlayerProgress();
		}
	}

	public int GetHighestPlayerScore()
	{
		return playerProgress.highestScore;
	}

	private void LoadPlayerProgress()
	{
		playerProgress = new PlayerProgress();

		if(PlayerPrefs.HasKey("highestScore"))
		{
			playerProgress.highestScore = PlayerPrefs.GetInt("highestScore");
		}
	}

	private void SavePlayerProgress()
	{
		PlayerPrefs.SetInt("highestScore", playerProgress.highestScore);
	}

	void CheckTimeScale()
	{
		if (Time.timeScale < 1)
		{
			slowTimeTimer += Time.fixedUnscaledDeltaTime;

			if (slowTimeTimer >= 2f)
			{
				Time.timeScale = 1f;
			}
		}

		if(Time.timeScale == 1)
		{
			slowTimeTimer = 0;
		}
	}
}
