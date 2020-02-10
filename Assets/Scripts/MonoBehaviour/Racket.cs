using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racket : Hitable
{
    public Vector3 initialPosition;
    public Sprite smallSprite;
    public Sprite bigSprite;

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

    public void ResetRacket()
    {
        this.transform.position = initialPosition;
        this.GetComponent<SpriteRenderer>().sprite = this.smallSprite;
        this.UpdateColliderSize();
    }

    private void UpdateColliderSize() {
        this.GetComponent<BoxCollider2D>().size = new Vector2(this.GetComponent<SpriteRenderer>().bounds.size.x - Mathf.Abs(this.GetComponent<BoxCollider2D>().offset.x)*2, this.GetComponent<BoxCollider2D>().size.y);
        this.GetComponent<CharacterMovement>().InitializeBoundaries();
    }

    public void EnlargeRacket()
    {
        this.GetComponent<SpriteRenderer>().sprite = this.bigSprite;
        this.UpdateColliderSize();
    }

    public void ShrinkRacket()
    {
        this.GetComponent<SpriteRenderer>().sprite = this.smallSprite;
        this.UpdateColliderSize();
    }
}

