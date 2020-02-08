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
        this.transform.position = new Vector3 (this.transform.position.x + newRelativePosition.x, this.transform.position.y + newRelativePosition.y, 0.0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        GameObject go = collision.gameObject;
        Bounce(collision.contacts[0].normal);

        if (go.GetComponent<Hitable>() != null)
        {
            go.GetComponent<Hitable>().Hit(this.gameObject);

        }

        this.previousHitted = go;


    }
    private void ResetSpeed()
    {
        this.currentSpeed = this.iniSpeed;

        return;
    }

    private void Bounce(Vector2 collisionNormal)
    {
        var speed = this.currentSpeed.magnitude;
        var direction = Vector2.Reflect(currentSpeed.normalized, collisionNormal);

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
