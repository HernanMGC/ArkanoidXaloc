using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Bricks Settings")]
    public Sprite[] breakableBrickPics;
    public Sprite unbreakableBrickPic;

    [Header("Capsules Settings")]
    [Range(0.0f,1.0f)]
    public float capsuleDropProbability;
    public CapsulePrize[] capsulePrizes;

    [Header("LevelSettings")]
    public GameObject brickStartingPoint;
    public Brick brickPrefab;
    public ArkanoidLevel[] arkanoidLevels;
    public float vBrickOffset;
    public float hBrickOffset;

    private void Start()
    {
        InitializeLevel();    
    }


    public void BrickDestroyed(Transform brickTransform) {
        Debug.Log("BrickDestroyed");
        if (Random.Range(0f,1f) < capsuleDropProbability)
        {
            Debug.Log("BrickDestroyed and prize");
            int weightSum = 0;
            foreach (CapsulePrize capsulePrize in capsulePrizes)
            {
                weightSum += capsulePrize.capsuleWeight;
            }

            int selectedWeight = Random.Range(1, weightSum);
            Debug.Log("weightSum " + weightSum);
            Debug.Log("selectedWeight " + selectedWeight);
            int i = 0;
            while (capsulePrizes[i].capsuleWeight < selectedWeight)
            {
                selectedWeight -= capsulePrizes[i].capsuleWeight;
                i++;
            }

            var go = Instantiate(capsulePrizes[i].capsule.gameObject, brickTransform.position, Quaternion.identity) as GameObject;
        }
    }

    private void InitializeLevel()
    {
        string levelDefinition = arkanoidLevels[0].levelDefinition.ToUpper();

        Debug.Log(arkanoidLevels[0].levelDefinition);

        char[] delims = new[] { '\r', '\n' };
        string[] levelDefinitionArray = levelDefinition.Split(delims, System.StringSplitOptions.RemoveEmptyEntries);

        float vBrickPosition = 0.0f;
        float hBrickPosition = 0.0f;

        for (int i = 0; i < levelDefinitionArray.Length; i++)
        {
            vBrickPosition = i * vBrickOffset;
            string line = levelDefinitionArray[i].Substring(0, Mathf.Min(levelDefinitionArray[i].Length, 10));

            for (int j = 0; j < line.Length; j++)
            {
                hBrickPosition = j * hBrickOffset;
                Vector3 newBrickPosition = new Vector3(brickStartingPoint.transform.position.x + hBrickPosition, brickStartingPoint.transform.position.y - vBrickPosition, 0f);

                InstantiateBrick(line[j], newBrickPosition);
            }
        }
    }

    private void InstantiateBrick(char brickChar, Vector3 position) {
        GameObject go;

        switch (brickChar)
            {
                case 'U':
                    go = Instantiate(brickPrefab.gameObject, position, Quaternion.identity) as GameObject;
                    go.GetComponent<Brick>().hitableReaction = Hitable.HitableReaction.DoNothing;
                    go.GetComponent<SpriteRenderer>().sprite = this.unbreakableBrickPic;
                    go.SetActive(true);
                    break;

                case '1':
                    go = Instantiate(brickPrefab.gameObject, position, Quaternion.identity) as GameObject;
                    go.GetComponent<Brick>().hitableReaction = Hitable.HitableReaction.DestroySelf;
                    go.GetComponent<Brick>().durability = 1;
                    go.GetComponent<SpriteRenderer>().sprite = this.breakableBrickPics[Mathf.Min(0, this.breakableBrickPics.Length - 1)];
                    go.SetActive(true);
                    break;

                case '2':
                    go = Instantiate(brickPrefab.gameObject, position, Quaternion.identity) as GameObject;
                    go.GetComponent<Brick>().hitableReaction = Hitable.HitableReaction.DestroySelf;
                    go.GetComponent<Brick>().durability = 2;
                    go.GetComponent<SpriteRenderer>().sprite = this.breakableBrickPics[Mathf.Min(1, this.breakableBrickPics.Length - 1)];
                    go.SetActive(true);
                    break;

                case '3':
                    go = Instantiate(brickPrefab.gameObject, position, Quaternion.identity) as GameObject;
                    go.GetComponent<Brick>().hitableReaction = Hitable.HitableReaction.DestroySelf;
                    go.GetComponent<Brick>().durability = 3;
                    go.GetComponent<SpriteRenderer>().sprite = this.breakableBrickPics[Mathf.Min(2, this.breakableBrickPics.Length - 1)];
                    go.SetActive(true);
                    break;

                default:
                    break;
        }
    }
}
