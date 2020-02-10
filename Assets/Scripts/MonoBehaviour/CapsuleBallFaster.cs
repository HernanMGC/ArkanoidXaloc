using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleBallFaster : Capsule
{
    public override void ApplyEffect() {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null) {
           ball.GetComponent<BallMovement>().IncrementSpeed();
        }
    }
}
