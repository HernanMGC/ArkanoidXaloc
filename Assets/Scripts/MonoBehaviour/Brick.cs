﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : Hitable
{
    private Transform brickTransform;
    private int currentDurability;

    public int durability;
    public GameObject gameManager;

    private void OnEnable()
    {
        this.brickTransform = this.transform;
        this.currentDurability = this.durability;
        this.gameManager = GameObject.FindWithTag("GameController");        
    }

    public override void Hit(GameObject go) {
        switch (this.hitableReaction)
        {
            case HitableReaction.DamageSelf:
                this.currentDurability--;

                if (this.currentDurability == 0) {
                    this.gameManager.GetComponent<GameManager>().BrickDestroyed(brickTransform, this.durability);
                    AudioManager.instance.PlaySingle(this.destroyClip);
                    Destroy(this.gameObject);
                }
                break;

            case HitableReaction.DestroySelf:
                this.gameManager.GetComponent<GameManager>().BrickDestroyed(brickTransform, this.durability);
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

