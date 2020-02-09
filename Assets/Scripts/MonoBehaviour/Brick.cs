using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : Hitable
{
    private Transform brickTransform;
    public int durability;
    public GameObject gameManager;

    private void OnEnable()
    {
        brickTransform = this.transform;
        this.gameManager = GameObject.FindWithTag("GameController");        
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

