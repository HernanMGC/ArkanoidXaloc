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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
