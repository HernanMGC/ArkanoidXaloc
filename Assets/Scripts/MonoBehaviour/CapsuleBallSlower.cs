using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleBallSlower : Capsule
{
    public override void ApplyEffect()
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        GameObject ball = gameManager.GetBallGO();
        if (ball != null)
        {
            ball.GetComponent<BallMovement>().DecrementSpeed();
        }
    }
}
