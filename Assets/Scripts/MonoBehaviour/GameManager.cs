using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private List<GameObject> bricks;

    [Header("Scene Settings")]
    public String gameSceneName;
    public String menuSceneName;
    public UIManager uiManager;

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
        if (SceneManager.GetSceneByName(gameSceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(gameSceneName);
        }

        if (!SceneManager.GetSceneByName(menuSceneName).isLoaded)
        {
            SceneManager.LoadSceneAsync(menuSceneName, LoadSceneMode.Additive);
        }
    }

    public void StartGame()
    {
        this.currentScore = 0;
        this.currentLevel = 0;
        SceneManager.UnloadSceneAsync(menuSceneName);
        StartCoroutine(AddGameScene());
    }

    private void Update()
    {
        if (SceneManager.GetSceneByName(gameSceneName).isLoaded && this.gameHasStarted)
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
        this.StoreScore();
        this.PauseGame();
        this.ShowLostMessageGUI();
    }

    public void GameWon()
    {
        this.StoreScore();
        this.PauseGame();
        this.ShowWonMessageGUI();
    }

    private void StoreScore()
    {
        int newScore;
        int oldScore;
        newScore = this.currentScore;
        for (int i = 0; i < 10; i++)
        {
            if (PlayerPrefs.HasKey(i + "HScore"))
            {
                if (PlayerPrefs.GetInt(i + "HScore") < newScore)
                {
                    oldScore = PlayerPrefs.GetInt(i + "HScore");
                    PlayerPrefs.SetInt(i + "HScore", newScore);
                    newScore = oldScore;
                }
            }
            else
            {
                PlayerPrefs.SetInt(i + "HScore", newScore);
                newScore = 0;
            }
        }
    }

    public void ShowLostMessageGUI()
    {
        uiManager.ShowLostMessageGUI();
    }
    public void ShowWonMessageGUI()
    {
        uiManager.ShowWonMessageGUI();
    }

    public void PauseMenuOpenGUI()
    {
        uiManager.PauseMenuOpenGUI();
    }

    public void HidePauseMenuGUI()
    {
        uiManager.HidePauseMenuGUI();
    }

    public void BackToMainMenu()
    {

        SceneManager.UnloadSceneAsync(gameSceneName);
        StartCoroutine(AddMenuScene());
    }

    public void RestartGame()
    {
        SceneManager.UnloadSceneAsync(gameSceneName);
        this.currentScore = 0;
        this.currentLevel = 0;
        StartCoroutine(AddGameScene());
    }


    public void LifeLost()
    {
        this.currentLifes--;
        if (currentLifes > 0)
        {
            StartCoroutine("ActivateBall");
        }
        else
        {
            this.gameLost = true;
        }

        this.LifeLostGUI();
    }

    IEnumerator AddGameScene()
    {
        asyncLoadLevel = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);

        while (!asyncLoadLevel.isDone) { yield return null; }

        this.InitializeVars();
        this.InitializeLevel(currentLevel);
        this.InitializeGUI();

        this.gameHasStarted = true;
    }

    IEnumerator AddMenuScene()
    {
        asyncLoadLevel = SceneManager.LoadSceneAsync(menuSceneName, LoadSceneMode.Additive);

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

        this.bricks = new List<GameObject>();

        switch (brickChar)
        {
            case 'U':
                go = Instantiate(brickPrefab.gameObject, position, Quaternion.identity, parent.transform) as GameObject;
                go.GetComponent<Brick>().hitableReaction = Hitable.HitableReaction.DoNothing;
                go.GetComponent<SpriteRenderer>().sprite = this.unbreakableBrickPic;
                go.SetActive(true);
                this.bricks.Add(go);
                break;

            case '1':
                go = Instantiate(brickPrefab.gameObject, position, Quaternion.identity, parent.transform) as GameObject;
                go.GetComponent<Brick>().hitableReaction = Hitable.HitableReaction.DamageSelf;
                go.GetComponent<Brick>().durability = 1;
                go.GetComponent<SpriteRenderer>().sprite = this.breakableBrickPics[Mathf.Min(0, this.breakableBrickPics.Length - 1)];
                go.SetActive(true);
                this.bricksPerLevel[levelNumber]++;
                this.bricks.Add(go);
                break;

            case '2':
                go = Instantiate(brickPrefab.gameObject, position, Quaternion.identity, parent.transform) as GameObject;
                go.GetComponent<Brick>().hitableReaction = Hitable.HitableReaction.DamageSelf;
                go.GetComponent<Brick>().durability = 2;
                go.GetComponent<SpriteRenderer>().sprite = this.breakableBrickPics[Mathf.Min(1, this.breakableBrickPics.Length - 1)];
                go.SetActive(true);
                this.bricksPerLevel[levelNumber]++;
                this.bricks.Add(go);
                break;

            case '3':
                go = Instantiate(brickPrefab.gameObject, position, Quaternion.identity, parent.transform) as GameObject;
                go.GetComponent<Brick>().hitableReaction = Hitable.HitableReaction.DamageSelf;
                go.GetComponent<Brick>().durability = 3;
                go.GetComponent<SpriteRenderer>().sprite = this.breakableBrickPics[Mathf.Min(2, this.breakableBrickPics.Length - 1)];
                go.SetActive(true);
                this.bricksPerLevel[levelNumber]++;
                this.bricks.Add(go);
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
        uiManager.InitializeVars();
        this.bricksPerLevel = new int[arkanoidLevels.Length];
        this.gameLost = false;
        this.currentLifes = maximumLifes;
    }

    private void InitializeGUI()
    {
        uiManager.InitializeGUI(this.maximumLifes, this.currentLevel, this.currentScore);
    }

    private void UpdateLevelGUI()
    {
        uiManager.UpdateLevelGUI(this.currentLevel);
    }

    private void UpdateScoreGUI()
    {
        uiManager.UpdateScoreGUI(this.currentScore);
    }

    private void LifeLostGUI()
    {
        uiManager.LifeLostGUI(this.currentLifes);
    }

    private void ShowLevelMessageGUI()
    {
        uiManager.ShowLevelMessageGUI(this.currentLevel);
    }

    private void HideLevelMessageGUI()
    {
        uiManager.HideLevelMessageGUI();
    }

    private void CleanBricks()
    {
        if (bricks != null)
        {
            foreach (GameObject brick in bricks)
            {
                Destroy(brick.gameObject);
            }
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

    public GameObject GetRacketGO()
    {
        return this.racket.gameObject;
    }


    public GameObject GetBallGO()
    {
        return this.ball.gameObject;
    }
}
