using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public static int score;
    public static int level = 1, playerhealth = 3;
    
    GameObject scoreObject;
    Text scoreDisplay; 

    public void Start() {
        scoreObject = GameObject.FindGameObjectWithTag("Score");
        scoreDisplay = scoreObject.GetComponent<Text>(); 
    }
    private void Update() {
        scoreDisplay.text = score.ToString(); 
    }
}
