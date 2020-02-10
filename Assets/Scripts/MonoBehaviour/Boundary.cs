using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : Hitable
{
    public override void Hit(GameObject go)
    {
        switch (this.hitableReaction)
        {
            case HitableReaction.DestroySelf:
                Destroy(this.gameObject);
                break;

            case HitableReaction.DestroyHitter:
                if (go.GetComponent<BallMovement>() != null) {
                    go.SetActive(false);
                }
                else
                {
                    Destroy(go);
                }
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

