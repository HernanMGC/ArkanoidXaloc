using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racket : Hitable
{
    public Vector3 initialPosition;
    public override void Hit(GameObject go)
    {
        switch (this.hitableReaction)
        {
            case HitableReaction.DestroySelf:
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

    internal void ResetRacket()
    {
        this.transform.position = initialPosition;
    }
}

