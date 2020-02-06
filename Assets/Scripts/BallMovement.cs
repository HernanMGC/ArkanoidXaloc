using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Vector2 currentSpeed;

    public Vector2 iniSpeed;
    private void Bounce(Vector2 collisionNormal)
    {
        var speed = this.currentSpeed.magnitude;
        var direction = Vector2.Reflect(currentSpeed.normalized, collisionNormal);

        Debug.Log("Out Direction: " + direction);
        this.currentSpeed = direction * speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.ResetSpeed();
    }

    //Update is called once per frame

    void FixedUpdate()
    {
        Vector2 currentPosition = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 newRelativePosition = this.currentSpeed * Time.fixedDeltaTime;
        this.transform.Translate(newRelativePosition.x, newRelativePosition.y, 0.0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("CollisionEntered");
        Bounce(collision.contacts[0].normal);
    }
    private void ResetSpeed()
    {
        this.currentSpeed = this.iniSpeed;

        return;
    }

}
