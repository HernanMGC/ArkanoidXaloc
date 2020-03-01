using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleRacketEnlarger : Capsule
{
    public override void ApplyEffect()
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        GameObject racket = gameManager.GetRacketGO();
        if (racket != null)
        {
            racket.GetComponent<Racket>().EnlargeRacket();
        }
    }
}
