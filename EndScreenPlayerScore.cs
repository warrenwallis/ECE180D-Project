using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

/*
Replace TextMeshPro Text UI with regular Text component:
Go to EndScreen scene and find Canvas > MainMenu > YourStats > Text (TMP)
Change the text to size 40 and center it left/right and up/down
 */

public class EndScreenPlayerScore : MonoBehaviour
{
    public Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "PlayerName \n" + "Score: " + ScoreDisplay.score.ToString() + "\n" + ScoreDisplay.percent.ToString() + "% Accuracy";
        Debug.Log("Setting scoreText.");
        //scoreText.text = "Your Score:";
    }
}
