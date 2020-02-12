using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameObject gameManager;

    private void Start()
    {
        this.gameManager = GameObject.FindWithTag("GameController");
    }

    public void PlayGame()
    {
        this.gameManager.GetComponent<GameManager>().StartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void UpdateHighScores()
    {
        TMPro.TextMeshProUGUI highscoresText = GameObject.Find("HighscoresText").GetComponent<TMPro.TextMeshProUGUI>();
        highscoresText.text = "";
        for (int i = 0; i < 10; i++)
        {
            if (PlayerPrefs.HasKey(i + "HScore"))
            {
                highscoresText.text += PlayerPrefs.GetInt(i + "HScore") + "\n";
            }
        }
    }
}
