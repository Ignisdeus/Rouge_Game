using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public Text scoreDisplay; 
    private void Start() {

        if (GameMaster.score > 0) {
            scoreDisplay.text = GameMaster.score.ToString(); 

        }
    }

    public void LoadMainMenu() {

        GameMaster.score = 0;
        GameMaster.level = 1; 
        GameMaster.playerhealth = 3;
        SceneManager.LoadScene("MainMenu");

    }

    public void LoadGame() {

        GameMaster.score = 0;
        GameMaster.playerhealth = 3;
        SceneManager.LoadScene("LevelOne");

    }
}
