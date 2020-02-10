using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleRacketShrinker : Capsule
{
    public override void ApplyEffect()
    {
        GameObject racket = GameObject.FindGameObjectWithTag("Player");
        if (racket != null)
        {
            racket.GetComponent<Racket>().ShrinkRacket();
        }
    }
}
