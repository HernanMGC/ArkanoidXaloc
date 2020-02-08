using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : Hitable
{
    private Transform brickTransform;
    public int durability;
    public GameObject gameManager;

    public void Start() {
        brickTransform = this.transform;
        this.gameManager = GameObject.FindWithTag("GameController");        

        if (this.hitableReaction == HitableReaction.DamageSelf)
        {
            Sprite[] breakableBrickPics = gameManager.GetComponent<GameManager>().breakableBrickPics;
            this.GetComponent<SpriteRenderer>().sprite = breakableBrickPics[Mathf.Min(this.durability-1, breakableBrickPics.Length-1)];

        } else if (this.hitableReaction == HitableReaction.DoNothing) {
            Sprite unbreakableBrickPic = gameManager.GetComponent<GameManager>().unbreakableBrickPic;
            this.GetComponent<SpriteRenderer>().sprite = unbreakableBrickPic;
        }
    }

    public override void Hit(GameObject go) {
        switch (this.hitableReaction)
        {
            case HitableReaction.DamageSelf:
                durability--;

                if (durability == 0) {
                    this.gameManager.GetComponent<GameManager>().BrickDestroyed(brickTransform);
                    Destroy(this.gameObject);
                }
                break;

            case HitableReaction.DestroySelf:
                this.gameManager.GetComponent<GameManager>().BrickDestroyed(brickTransform);
                Destroy(this.gameObject);
            break;

            case HitableReaction.DestroyHitter:
                Destroy(go);
            break;


            case HitableReaction.DoNothing:
            break;


            case HitableReaction.AttachHitter:
            break;


            default:
                break;
        }
    }

 
}

