using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public Vector2 currentSpeed;
    private GameObject previousHitted = null;
    private readonly float incrementStep = 1f;

    public Vector2 iniSpeed;

    // Start is called before the first frame update
    void Start()
    {
        this.ResetSpeed();
    }

    //Update is called once per frame

    void FixedUpdate()
    {
        Vector2 newRelativePosition = this.currentSpeed * Time.fixedDeltaTime;
        this.transform.Translate(newRelativePosition.x, newRelativePosition.y, 0.0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        GameObject go = collision.gameObject;
        Bounce(collision);

        if (go.GetComponent<Hitable>() != null)
        {
            go.GetComponent<Hitable>().Hit(this.gameObject);

        }

        this.previousHitted = go;
    }

    float hitFactor(Vector2 ballPos, Vector2 racketPos,
                float racketWidth)
    {
        // ascii art:
        //
        // 1  -0.5  0  0.5   1  <- x value
        // ===================  <- racket
        //
        return (ballPos.x - racketPos.x) / racketWidth;
    }

    private void ResetSpeed()
    {
        this.currentSpeed = this.iniSpeed;

        return;
    }

    private void Bounce(Collision2D collision)
    {

        var speed = this.currentSpeed.magnitude;
        var direction = Vector2.Reflect(currentSpeed.normalized, collision.contacts[0].normal);



        if (collision.gameObject.tag == "Player")
        {
            float x = hitFactor(transform.position, collision.collider.transform.position, collision.collider.bounds.size.x);
            direction.x = x;
        }


        this.currentSpeed = direction * speed;
    }

    public void IncrementSpeed() {
        Debug.Log("Ball speed incremented!");
        this.currentSpeed = this.currentSpeed * (1f + this.incrementStep);
    }

    public void DecrementSpeed() {
        Debug.Log("Ball speed decremented!");
        this.currentSpeed = this.currentSpeed * 1/(1f + this.incrementStep);
    }
}
