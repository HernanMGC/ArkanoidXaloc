using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : Hitable
{ 
    public int durability;
    public GameObject gameManeger;

    public void Start() {
        this.gameManeger = GameObject.FindWithTag("GameController");        

        if (this.hitableReaction == HitableReaction.DamageSelf)
        {
            Sprite[] breakableBrickPics = gameManeger.GetComponent<GameManager>().breakableBrickPics;
            this.GetComponent<SpriteRenderer>().sprite = breakableBrickPics[Mathf.Min(this.durability-1, breakableBrickPics.Length-1)];

        } else if (this.hitableReaction == HitableReaction.DoNothing) {
            Sprite unbreakableBrickPic = gameManeger.GetComponent<GameManager>().unbreakableBrickPic;
            this.GetComponent<SpriteRenderer>().sprite = unbreakableBrickPic;
        }
    }

    public override void Hit(GameObject go) {
        switch (this.hitableReaction)
        {
            case HitableReaction.DamageSelf:
                Debug.Log("Hitable DamageSelf");
                durability--;

                if (durability == 0) {
                    Destroy(this.gameObject);
                }
                break;

            case HitableReaction.DestroySelf:
                Debug.Log("Hitable DestroySelf");
                Destroy(this.gameObject);
            break;

            case HitableReaction.DestroyHitter:
                Debug.Log("Hitable DestroyHitter");
                Destroy(go);
            break;


            case HitableReaction.DoNothing:
                Debug.Log("Hitable DoNothing");
            break;


            case HitableReaction.AttachHitter:
                    Debug.Log("Hitable AttachHitter");
            break;


            default:
                break;
        }
    }
}

