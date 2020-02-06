using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovementWithVelocity : MonoBehaviour
{
    private Vector2 currentSpeed;
    private Rigidbody2D ball;

    public Vector2 iniSpeed;

    // Start is called before the first frame update
    void Start()
    {
        this.ball = this.GetComponent<Rigidbody2D>();
        this.ResetSpeed();
        Debug.Log("Ball: " + this.ball);
    }

    void Update()
    {
        this.currentSpeed = this.ball.velocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("CollisionEntered");
        Bounce(collision.contacts[0].normal);
    }

    private void ResetSpeed()
    {
        this.currentSpeed = this.iniSpeed;
        this.ball.velocity = this.currentSpeed;

        return;
    }

    private void Bounce(Vector2 collisionNormal)
    {
        var speed = this.currentSpeed.magnitude;
        var direction = Vector2.Reflect(this.currentSpeed.normalized, collisionNormal);

        Debug.Log("Out Direction: " + direction);
        this.ball.velocity = direction * speed;
    }
}
