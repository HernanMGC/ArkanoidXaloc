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
}
