using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayHighScoreOnMenu : MonoBehaviour {

public TextMeshProUGUI highScoreText;

private DataController dataController;

	void Awake () 
	{
		dataController = FindObjectOfType<DataController>();
		PrintScore();
	}

	void PrintScore() //"print" ahaha, sounds so professional
	{
		highScoreText.text = "High Score: " + dataController.GetHighestPlayerScore().ToString();
	}

}
