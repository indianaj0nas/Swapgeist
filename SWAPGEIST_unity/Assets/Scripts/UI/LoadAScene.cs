using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAScene : MonoBehaviour
{

    public string loadThis;
    public string playerScene;
    public Scene destinationScene;
    public List<GameObject> objectsToMove;
    private bool levelSelected;

    void Awake()
    {
        levelSelected = false;
    }

    void Update()
    {
        StartLoadingScreen();
    }

    public void LoadThisScene()
    {
        SceneManager.LoadScene(loadThis, LoadSceneMode.Additive);
        //SceneManager.UnloadSceneAsync("MainMenu");
        //Invoke("UnloadMainMenu", 3f);
        //StartLoadingScreen();
        levelSelected = true;

    }

    public void UnLoadMainMenu()
    {
        SceneManager.UnloadSceneAsync("MainMenu");
    }

    public void StartLoadingScreen()
    {
        if (levelSelected)
        {
            Scene scene = SceneManager.GetActiveScene();
            Scene playerScene;
            //Scene destinationScene;
            //destinationScene = SceneManager.GetActiveScene();
            //Debug.Log("1 player");
            //if (scene.name != "MainMenu")
            //{
            //if (SceneManager.GetActiveScene().name == "SinglePlayer")
            if (SceneManager.GetSceneByName("SinglePlayer").isLoaded)
            {
                Debug.Log("1 player");
                playerScene = SceneManager.GetSceneByName("SinglePlayer");
                SceneManager.MergeScenes(playerScene, SceneManager.GetSceneByName(loadThis));
            }

            if (SceneManager.GetSceneByName("MultiPlayer").isLoaded)
            {
                Debug.Log("2 players");
                playerScene = SceneManager.GetSceneByName("MultiPlayer");
                SceneManager.MergeScenes(playerScene, SceneManager.GetSceneByName(loadThis));
            }
            /* foreach (GameObject i in objectsToMove)
             {
                 SceneManager.MoveGameObjectToScene(i, SceneManager.GetSceneByName(loadThis));
             }*/
            //}
			//Invoke("UnloadMainMenu", 3f);
			SceneManager.UnloadSceneAsync("MainMenu");
        }

    }
}
