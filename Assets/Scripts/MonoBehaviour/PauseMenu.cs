using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameObject gameManager;

    private void Start()
    {
        this.gameManager = GameObject.FindWithTag("GameController");
    }

    public void BackToMainMenu()
    {
        this.gameManager.GetComponent<GameManager>().BackToMainMenu();
    }

    public void RestartGame()
    {
        this.gameManager.GetComponent<GameManager>().RestartGame();
    }

    public void ResumeGame()
    {
        this.gameManager.GetComponent<GameManager>().ResumeGame();
    }
}
