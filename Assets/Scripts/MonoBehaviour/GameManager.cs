using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {
    private int[] bricksPerLevel;
    private int currentLevel = 0;
    private int remainingBricks;
    private bool gameOver = true;
    private int currentLifes;
    private int currentScore = 0;
    private bool isLevelChanging = false;

    [Header("Player Settings")]
    public BallMovement ball;
    public Racket racket;

    [Header("Bricks Settings")]
    public Sprite[] breakableBrickPics;
    public Sprite unbreakableBrickPic;

    [Header("Capsules Settings")]
    [Range(0.0f, 1.0f)]
    public float capsuleDropProbability;
    public CapsulePrize[] capsulePrizes;

    [Header("LevelSettings")]
    public int maximumLifes = 3;
    public GameObject brickStartingPoint;
    public Brick brickPrefab;
    public ArkanoidLevel[] arkanoidLevels;
    public float vBrickOffset;
    public float hBrickOffset;

    [Header("GUI")]
    public Canvas canvas;

    private void Start()
    {
        this.InitializeVars();
        this.InitializeLevel(currentLevel);
        this.InitializeGUI();
    }

    private void Update()
    {
        if (this.remainingBricks <= 0 && !this.gameOver && this.currentLevel + 1 < this.arkanoidLevels.Length)
        {
            if (!isLevelChanging)
            {
                this.currentLevel++;
                StartCoroutine(InitializeLevelCoroutine(1, this.currentLevel));
            }
        }
    }

    public void LifeLost()
    {
        this.currentLifes--;
        if (currentLifes > 0)
        {
            StartCoroutine(this.ActivateBall());
        }
        else
        {
            this.gameOver = true;
        }

        this.LifeLostGUI();
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
        this.bricksPerLevel = new int[arkanoidLevels.Length];
        this.gameOver = false;
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
        this.ball.GetComponent<BallMovement>().ResetBall();
        this.racket.GetComponent<CharacterMovement>().AttachBall(this.ball.gameObject);
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

    private void ResumeGame() {
        this.racket.GetComponent<CharacterMovement>().SetMove(true);
        if (this.ball.gameObject.transform.parent == null)
        {
            this.ball.GetComponent<BallMovement>().SetMove(true);
        }
    }

    private void PauseGame()
    {
        this.racket.GetComponent<CharacterMovement>().SetMove(false);
        this.ball.GetComponent<BallMovement>().SetMove(false);
    }
}
