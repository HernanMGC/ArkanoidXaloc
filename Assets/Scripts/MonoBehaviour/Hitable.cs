using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hitable : MonoBehaviour
{
    [System.Serializable]
    public enum HitableReaction
    {
        DamageSelf,
        DestroySelf,
        DestroyHitter,
        DoNothing,
        AttachHitter
    }

    public HitableReaction hitableReaction;
    public AudioClip destroyClip;

    public virtual void Hit(GameObject go) { }
}
