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

