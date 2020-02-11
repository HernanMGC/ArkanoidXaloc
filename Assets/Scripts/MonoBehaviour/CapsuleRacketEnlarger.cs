using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleRacketEnlarger : Capsule
{
    public override void ApplyEffect()
    {
        GameObject racket = GameObject.FindGameObjectWithTag("Player");
        if (racket != null)
        {
            racket.GetComponent<Racket>().EnlargeRacket();
        }
    }
}
