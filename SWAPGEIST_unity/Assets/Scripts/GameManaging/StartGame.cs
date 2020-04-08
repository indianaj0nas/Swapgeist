using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class StartGame : MonoBehaviour
{
	public int playerId = 0;
    private Player player;

	void Awake()
	{
		player = ReInput.players.GetPlayer(playerId);

	}

    void Update()
    {
        StartTheGame();
        QuitGame();
    }

    void StartTheGame()
    {
        if (player.GetButtonUp("Possess"))
        {
            SceneManager.LoadScene("GameRoom");
            SceneManager.UnloadSceneAsync("MainMenu");
        }
    }

    void QuitGame()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
