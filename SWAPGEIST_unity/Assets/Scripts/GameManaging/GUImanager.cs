using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Rewired;

public class GUImanager : MonoBehaviour
{

    public GameObject titleScreen;
    public GameObject titleDefaultButton;
    public GameObject levelSelectScreen;
    public GameObject levelScreenDefaultButton;
    public GameObject playerSelectScreen;
    public GameObject playerSelectScreenDefaultButton;
    public GameObject creditsScreen;
    public GameObject creditsDefaultButton;
    public GameObject playerObject;

    private Player player;
    public int playerId;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SinglePlayer()
    {
        SceneManager.LoadScene("SinglePlayer", LoadSceneMode.Additive);
    }

    public void MultiPlayer()
    {
        SceneManager.LoadScene("MultiPlayer", LoadSceneMode.Additive);
    }

    public void SetSelectedButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button, null);
    }


}
