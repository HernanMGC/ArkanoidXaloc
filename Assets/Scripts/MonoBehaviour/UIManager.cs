using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    private Canvas canvas;
    private GameObject pauseMenu;
    private GameObject gameOverMenu;
    private GameObject backToMainMenuMenu;
    private GameObject lostText;
    private GameObject wonText;
    private GameObject yesButton;
    private GameObject lifeGroup;
    private GameObject life;
    private GameObject levelGroup;
    private GameObject level;
    private GameObject scoreGroup;
    private GameObject score;
    private GameObject messageGroup;
    private GameObject message;

    public void InitializeVars()
    {
        this.canvas = GameObject.Find("GameCanvas").GetComponent<Canvas>();
        this.pauseMenu = this.canvas.gameObject.transform.Find("PauseMenu").gameObject;
        this.gameOverMenu = pauseMenu.gameObject.transform.Find("GameOverMenu").gameObject;
        this.backToMainMenuMenu = this.canvas.gameObject.transform.Find("PauseMenu").gameObject.transform.Find("BackToMainMenuMenu").gameObject;
        this.lostText = gameOverMenu.transform.Find("LostText").gameObject;
        this.wonText = gameOverMenu.transform.Find("WonText").gameObject;
        this.yesButton = gameOverMenu.transform.Find("YesButton").gameObject;
        this.lifeGroup = this.canvas.gameObject.transform.Find("Lifes").gameObject;
        this.life = lifeGroup.transform.Find("Life").gameObject;
        this.levelGroup = this.canvas.gameObject.transform.Find("Level").gameObject;
        this.level = levelGroup.transform.Find("LevelValue").gameObject;
        this.scoreGroup = this.canvas.gameObject.transform.Find("Score").gameObject;
        this.score = scoreGroup.transform.Find("ScoreValue").gameObject;
        this.messageGroup = this.canvas.gameObject.transform.Find("Message").gameObject;
        this.message = messageGroup.transform.Find("MessageValue").gameObject;
    }

    public void ShowLostMessageGUI()
    {
        pauseMenu.SetActive(true);
        gameOverMenu.SetActive(true);
        backToMainMenuMenu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(yesButton);

        wonText.SetActive(false);
        lostText.SetActive(true);
    }
    public void ShowWonMessageGUI()
    {
        pauseMenu.SetActive(true);
        gameOverMenu.SetActive(true);
        backToMainMenuMenu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(yesButton);

        lostText.SetActive(false);
        wonText.SetActive(true);
    }

    public void PauseMenuOpenGUI()
    {
        pauseMenu.SetActive(true);
        backToMainMenuMenu.SetActive(true);
        gameOverMenu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(yesButton);
    }

    public void HidePauseMenuGUI()
    {
        pauseMenu.SetActive(false);
        backToMainMenuMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    public void InitializeGUI(int maximumLifes, int currentLevel, int currentScore)
    {
        for (int i = 1; i < maximumLifes; i++)
        {
            GameObject newLife = Instantiate(life, lifeGroup.transform) as GameObject;
            newLife.transform.localPosition += new Vector3((newLife.GetComponent<RectTransform>().rect.width + 10) * i, 0, 0);
        }

        this.UpdateLevelGUI(currentLevel);
        this.UpdateScoreGUI(currentScore);
    }

    public void UpdateLevelGUI(int currentLevel)
    {
        level.GetComponent<TMPro.TextMeshProUGUI>().text = (currentLevel + 1).ToString();
    }

    public void UpdateScoreGUI(int currentScore)
    {
        string format = "0000000000";
        score.GetComponent<TMPro.TextMeshProUGUI>().text = (currentScore).ToString(format);
    }

    public void LifeLostGUI(int currentLifes)
    {

        if (currentLifes >= 0)
        {
            Destroy(lifeGroup.transform.GetChild(currentLifes).gameObject);
        }
    }

    public void ShowLevelMessageGUI(int currentLevel)
    {

        message.GetComponent<TMPro.TextMeshProUGUI>().text = "Welcome to\nLevel " + (currentLevel + 1);
        messageGroup.SetActive(true);
    }

    public void HideLevelMessageGUI()
    {
        messageGroup.SetActive(false);
    }
}
