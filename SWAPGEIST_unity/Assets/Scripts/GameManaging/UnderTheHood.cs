using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UnderTheHood : MonoBehaviour
{

    public int highScore;
    public int thisScore;
    public TextMeshProUGUI thisScoreText;
    //public TextMeshProUGUI thisHighScore;
    public GameObject gameOverScreen;
    public bool gameOver;

    private GameObject player;
    private float playAgainTimer;

    void Awake()
    {
        player = GameObject.Find("PLAYER");
        gameOver = false;
    }

    void Update()
    {
        HandleThisScore();
        NewHighScore();
       // ShowGameOverScreen();
    }

    void ShowGameOverScreen()
    {
        if (player == null)
        {
            gameOver = true;
        }

        if (!gameOver)
        {
            playAgainTimer = 0;
        }

        if (gameOver)
        {
            gameOverScreen.SetActive(true);
            if (Input.GetButtonUp("Pause"))
            {
                SceneManager.LoadScene("MainMenu");
            }

            playAgainTimer += Time.deltaTime;

            if (Input.GetButtonUp("Possess") && playAgainTimer > 2.5f)
            {
                SceneManager.LoadScene("GameRoom");
            }
        }
    }

    void NewHighScore()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        Score scoreScript = gameManager.GetComponent<Score>();

        if (thisScore > highScore)
        {
            highScore = thisScore;
            //thisHighScore.text = "High Score: " + highScore.ToString();
        }

    }

    void HandleThisScore()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        Score scoreScript = gameManager.GetComponent<Score>();

        thisScore = scoreScript.currentScore;
        thisScoreText.text = "Current Score: " + thisScore.ToString();

    }

}
