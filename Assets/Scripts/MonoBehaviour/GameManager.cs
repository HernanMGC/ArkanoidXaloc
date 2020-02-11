using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class GameManager : MonoBehaviour {
    private int[] bricksPerLevel;
    private int currentLevel = 0;
    private int remainingBricks;
    private bool gameLost = false;
    private bool gameHasStarted = false;
    private bool gamePaused = false;
    private int currentLifes;
    private int currentScore = 0;
    private bool isLevelChanging = false;
    private BallMovement ball;
    private Racket racket;
    private GameObject brickStartingPoint;
    private Canvas canvas;
    private AsyncOperation asyncLoadLevel;

    [Header("Bricks Settings")]
    public Sprite[] breakableBrickPics;
    public Sprite unbreakableBrickPic;

    [Header("Capsules Settings")]
    [Range(0.0f, 1.0f)]
    public float capsuleDropProbability;
    public CapsulePrize[] capsulePrizes;

    [Header("LevelSettings")]
    public int maximumLifes = 3;
    public Brick brickPrefab;
    public ArkanoidLevel[] arkanoidLevels;
    public float vBrickOffset;
    public float hBrickOffset;

    private void Start()
    {
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);
    }

    public void StartGame()
    {
        this.currentScore = 0;
        this.currentLevel = 0;
        SceneManager.UnloadSceneAsync("Menu");
        StartCoroutine(AddGameScene());
    }

    private void Update()
    {
        if (SceneManager.GetSceneByName("GameScene").isLoaded && this.gameHasStarted)
        {
            if (this.remainingBricks <= 0 && !this.gameLost)
            {
                if (this.currentLevel < this.arkanoidLevels.Length - 1)
                {
                    if (!isLevelChanging)
                    {
                        this.currentLevel++;
                        StartCoroutine(InitializeLevelCoroutine(1, this.currentLevel));
                    }
                }
                else if (!this.gamePaused)
                {
                    this.GameWon();
                }
            }
            else if (this.gameLost && !this.gamePaused)
            {
                this.GameLost();
            }

            if (Input.GetAxisRaw("Escape") > 0)
            {
                this.PauseGame();
                this.PauseMenuOpenGUI();
            }
        }
    }

    public void GameLost()
    {
        this.PauseGame();
        this.ShowLostMessageGUI();
    }

    public void GameWon()
    {
        this.PauseGame();
        this.ShowWonMessageGUI();
    }

    public void ShowLostMessageGUI()
    {
        GameObject pauseMenu = this.canvas.gameObject.transform.Find("PauseMenu").gameObject;
        GameObject gameOverMenu = this.canvas.gameObject.transform.Find("PauseMenu").gameObject.transform.Find("GameOverMenu").gameObject;
        GameObject backToMainMenuMenu = this.canvas.gameObject.transform.Find("PauseMenu").gameObject.transform.Find("BackToMainMenuMenu").gameObject;
        pauseMenu.SetActive(true);
        gameOverMenu.SetActive(true);
        backToMainMenuMenu.SetActive(false);

        GameObject lostText = gameOverMenu.transform.Find("LostText").gameObject;
        GameObject wonText = gameOverMenu.transform.Find("WonText").gameObject;

        GameObject yesButton = gameOverMenu.transform.Find("YesButton").gameObject;

        wonText.SetActive(false);
        lostText.SetActive(true);
    }
    public void ShowWonMessageGUI()
    {
        GameObject pauseMenu = this.canvas.gameObject.transform.Find("PauseMenu").gameObject;
        GameObject gameOverMenu = this.canvas.gameObject.transform.Find("PauseMenu").gameObject.transform.Find("GameOverMenu").gameObject;
        GameObject backToMainMenuMenu = this.canvas.gameObject.transform.Find("PauseMenu").gameObject.transform.Find("BackToMainMenuMenu").gameObject;
        pauseMenu.SetActive(true);
        gameOverMenu.SetActive(true);
        backToMainMenuMenu.SetActive(false);

        GameObject lostText = gameOverMenu.transform.Find("LostText").gameObject;
        GameObject wonText = gameOverMenu.transform.Find("WonText").gameObject;

        GameObject yesButton = gameOverMenu.transform.Find("YesButton").gameObject;

        EventSystem.current.SetSelectedGameObject(yesButton);

        lostText.SetActive(false);
        wonText.SetActive(true);
    }

    public void PauseMenuOpenGUI()
    {
        GameObject pauseMenu = this.canvas.gameObject.transform.Find("PauseMenu").gameObject;
        GameObject gameOverMenu = this.canvas.gameObject.transform.Find("PauseMenu").gameObject.transform.Find("GameOverMenu").gameObject;
        GameObject backToMainMenuMenu = this.canvas.gameObject.transform.Find("PauseMenu").gameObject.transform.Find("BackToMainMenuMenu").gameObject;
        pauseMenu.SetActive(true);
        backToMainMenuMenu.SetActive(true);
        gameOverMenu.SetActive(false);

        GameObject yesButton = backToMainMenuMenu.transform.Find("YesButton").gameObject;

        EventSystem.current.SetSelectedGameObject(yesButton);
    }

    public void HidePauseMenuGUI()
    {
        GameObject pauseMenu = this.canvas.gameObject.transform.Find("PauseMenu").gameObject;
        GameObject gameOverMenu = this.canvas.gameObject.transform.Find("PauseMenu").gameObject.transform.Find("GameOverMenu").gameObject;
        GameObject backToMainMenuMenu = this.canvas.gameObject.transform.Find("PauseMenu").gameObject.transform.Find("BackToMainMenuMenu").gameObject;
        pauseMenu.SetActive(false);
        backToMainMenuMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    public void BackToMainMenu()
    {

        SceneManager.UnloadSceneAsync("GameScene");
        StartCoroutine(AddMenuScene());
    }

    public void RestartGame()
    {
        SceneManager.UnloadSceneAsync("GameScene");
        this.currentScore = 0;
        this.currentLevel = 0;
        StartCoroutine(AddGameScene());
    }


    public void LifeLost()
    {
        Debug.Log("LifeLost");
        this.currentLifes--;
        if (currentLifes > 0)
        {
            StartCoroutine(this.ActivateBall());
        }
        else
        {
            Debug.Log("gameLost");
            this.gameLost = true;
        }

        this.LifeLostGUI();
    }

    IEnumerator AddGameScene()
    {
        asyncLoadLevel = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);

        while (!asyncLoadLevel.isDone) { yield return null; }

        this.InitializeVars();
        this.InitializeLevel(currentLevel);
        this.InitializeGUI();

        this.gameHasStarted = true;
    }

    IEnumerator AddMenuScene()
    {
        asyncLoadLevel = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);

        while (!asyncLoadLevel.isDone) { yield return null; }
    }

    IEnumerator ActivateBall()
    {
        //returning 0 will make it wait 1 frame
        yield return 0;

        this.ResetBall();
    }

    IEnumerator ShowLevelMessageGUICoroutine(float seconds)
    {
        this.PauseGame();
        this.ShowLevelMessageGUI();

        yield return new WaitForSeconds(seconds);

        this.HideLevelMessageGUI();
        this.ResumeGame();

    }

    IEnumerator InitializeLevelCoroutine(float seconds, int level)
    {
        this.isLevelChanging = true;
        this.PauseGame();

        yield return new WaitForSeconds(seconds);

        this.InitializeLevel(level);
        this.UpdateLevelGUI();
        this.isLevelChanging = false;
        this.gamePaused = false;
    }

    public void BrickDestroyed(Transform brickTransform, int durability) {
        this.remainingBricks--;
        this.currentScore += 100 * durability;
        this.UpdateScoreGUI();
        if (UnityEngine.Random.Range(0f, 1f) < capsuleDropProbability)
        {
            int weightSum = 0;
            foreach (CapsulePrize capsulePrize in capsulePrizes)
            {
                weightSum += capsulePrize.capsuleWeight;
            }

            int selectedWeight = UnityEngine.Random.Range(1, weightSum);
            int i = 0;
            while (capsulePrizes[i].capsuleWeight < selectedWeight)
            {
                selectedWeight -= capsulePrizes[i].capsuleWeight;
                i++;
            }

            var go = Instantiate(capsulePrizes[i].capsule.gameObject, brickTransform.position, Quaternion.identity) as GameObject;
        }
    }

    private void InitializeLevel(int levelNumber)
    {
        this.ResetGame();

        string levelDefinition = arkanoidLevels[levelNumber].levelDefinition.ToUpper();

        char[] delims = new[] { '\r', '\n' };
        string[] levelDefinitionArray = levelDefinition.Split(delims, System.StringSplitOptions.RemoveEmptyEntries);

        float vBrickPosition = 0.0f;
        float hBrickPosition = 0.0f;

        for (int i = 0; i < levelDefinitionArray.Length; i++)
        {
            vBrickPosition = i * this.vBrickOffset;
            string line = levelDefinitionArray[i].Substring(0, Mathf.Min(levelDefinitionArray[i].Length, 10));

            for (int j = 0; j < line.Length; j++)
            {
                hBrickPosition = j * this.hBrickOffset;
                Vector3 newBrickPosition = new Vector3(brickStartingPoint.transform.position.x + hBrickPosition, brickStartingPoint.transform.position.y - vBrickPosition, 0f);

                this.InstantiateBrick(line[j], newBrickPosition, levelNumber, this.brickStartingPoint);
            }
        }

        this.remainingBricks = this.bricksPerLevel[levelNumber];

        

        StartCoroutine(ShowLevelMessageGUICoroutine(1));
    }

    private void InstantiateBrick(char brickChar, Vector3 position, int levelNumber, GameObject parent) {
        GameObject go;

        switch (brickChar)
        {
            case 'U':
                go = Instantiate(brickPrefab.gameObject, position, Quaternion.identity, parent.transform) as GameObject;
                go.GetComponent<Brick>().hitableReaction = Hitable.HitableReaction.DoNothing;
                go.GetComponent<SpriteRenderer>().sprite = this.unbreakableBrickPic;
                go.SetActive(true);
                break;

            case '1':
                go = Instantiate(brickPrefab.gameObject, position, Quaternion.identity, parent.transform) as GameObject;
                go.GetComponent<Brick>().hitableReaction = Hitable.HitableReaction.DamageSelf;
                go.GetComponent<Brick>().durability = 1;
                go.GetComponent<SpriteRenderer>().sprite = this.breakableBrickPics[Mathf.Min(0, this.breakableBrickPics.Length - 1)];
                go.SetActive(true);
                this.bricksPerLevel[levelNumber]++;
                break;

            case '2':
                go = Instantiate(brickPrefab.gameObject, position, Quaternion.identity, parent.transform) as GameObject;
                go.GetComponent<Brick>().hitableReaction = Hitable.HitableReaction.DamageSelf;
                go.GetComponent<Brick>().durability = 2;
                go.GetComponent<SpriteRenderer>().sprite = this.breakableBrickPics[Mathf.Min(1, this.breakableBrickPics.Length - 1)];
                go.SetActive(true);
                this.bricksPerLevel[levelNumber]++;
                break;

            case '3':
                go = Instantiate(brickPrefab.gameObject, position, Quaternion.identity, parent.transform) as GameObject;
                go.GetComponent<Brick>().hitableReaction = Hitable.HitableReaction.DamageSelf;
                go.GetComponent<Brick>().durability = 3;
                go.GetComponent<SpriteRenderer>().sprite = this.breakableBrickPics[Mathf.Min(2, this.breakableBrickPics.Length - 1)];
                go.SetActive(true);
                this.bricksPerLevel[levelNumber]++;
                break;

            default:
                break;
        }
    }

    private void InitializeVars()
    {
        this.ball = GameObject.FindWithTag("Ball").GetComponent<BallMovement>();
        this.racket = GameObject.FindWithTag("Player").GetComponent<Racket>();
        this.brickStartingPoint = GameObject.Find("BrickStartingPoint");
        this.canvas = GameObject.Find("GameCanvas").GetComponent<Canvas>();
        this.bricksPerLevel = new int[arkanoidLevels.Length];
        this.gameLost = false;
        this.currentLifes = maximumLifes;
    }

    private void InitializeGUI()
    {
        GameObject lifeGroup = this.canvas.gameObject.transform.Find("Lifes").gameObject;
        GameObject life = lifeGroup.transform.Find("Life").gameObject;

        for (int i = 1; i < this.maximumLifes; i++)
        {
            GameObject newLife = Instantiate(life, lifeGroup.transform) as GameObject;
            newLife.transform.localPosition += new Vector3((newLife.GetComponent<RectTransform>().rect.width + 10) * i, 0, 0);
        }

        this.UpdateLevelGUI();
        this.UpdateScoreGUI();
    }

    private void UpdateLevelGUI()
    {
        GameObject levelGroup = this.canvas.gameObject.transform.Find("Level").gameObject;
        GameObject level = levelGroup.transform.Find("LevelValue").gameObject;
        level.GetComponent<TMPro.TextMeshProUGUI>().text = (this.currentLevel + 1).ToString();
    }

    private void UpdateScoreGUI()
    {
        GameObject scoreGroup = this.canvas.gameObject.transform.Find("Score").gameObject;
        GameObject score = scoreGroup.transform.Find("ScoreValue").gameObject;
        string format = "0000000000";
        score.GetComponent<TMPro.TextMeshProUGUI>().text = (this.currentScore).ToString(format);
    }

    private void LifeLostGUI()
    {
        GameObject lifeGroup = this.canvas.gameObject.transform.Find("Lifes").gameObject;

        if (this.currentLifes >= 0) {
            Destroy(lifeGroup.transform.GetChild(this.currentLifes).gameObject);
        }
    }

    private void ShowLevelMessageGUI()
    {
        GameObject messageGroup = this.canvas.gameObject.transform.Find("Message").gameObject;
        GameObject message = messageGroup.transform.Find("MessageValue").gameObject;

        message.GetComponent<TMPro.TextMeshProUGUI>().text = "Welcome to\nLevel " + (this.currentLevel + 1);
        messageGroup.SetActive(true);
    }

    private void HideLevelMessageGUI()
    {
        GameObject messageGroup = this.canvas.gameObject.transform.Find("Message").gameObject;
        GameObject message = messageGroup.transform.Find("MessageValue").gameObject;

        messageGroup.SetActive(false);
    }

    private void CleanBricks()
    {
        Brick[] bricks = GameObject.FindObjectsOfType(typeof(Brick)) as Brick[];

        foreach (Brick brick in bricks)
        {
            Destroy(brick.gameObject);
        }
    }
    private void CleanCapsules()
    {
        Capsule[] capsules = GameObject.FindObjectsOfType(typeof(Capsule)) as Capsule[];

        foreach (Capsule capsule in capsules)
        {
            Destroy(capsule.gameObject);
        }
    }

    private void ResetBall() {
        if (ball != null)
        {
            this.ball.GetComponent<BallMovement>().ResetBall();
        }
        if (racket != null)
        {
            this.racket.GetComponent<CharacterMovement>().AttachBall(this.ball.gameObject);
        }
    }
    private void ResetRacket()
    {
        this.racket.GetComponent<Racket>().ResetRacket();
    }

    private void ResetGame() {
        this.CleanBricks();
        this.CleanCapsules();
        this.ResetRacket();
        this.ResetBall();
    }

    public void ResumeGame() {
        this.racket.GetComponent<CharacterMovement>().SetMove(true);
        if (this.ball.gameObject.transform.parent == null)
        {
            this.ball.GetComponent<BallMovement>().SetMove(true);
        }
        Capsule[] allCapsules = UnityEngine.Object.FindObjectsOfType<Capsule>();
        foreach (Capsule capsule in allCapsules)
        {
            capsule.SetMove(true);
        }
        this.gamePaused = false;

        this.HidePauseMenuGUI();
    }

    private void PauseGame()
    {
        this.gamePaused = true;
        this.racket.GetComponent<CharacterMovement>().SetMove(false);
        this.ball.GetComponent<BallMovement>().SetMove(false);
        Capsule[] allCapsules = UnityEngine.Object.FindObjectsOfType<Capsule>();
        foreach (Capsule capsule in allCapsules)
        {
            capsule.SetMove(false);
        }
    }
}
