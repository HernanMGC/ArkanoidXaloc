using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private GameObject previousHitted = null;
    private readonly float incrementStep = 1f;
    private bool canMove = false;
    private GameManager gameManager;

    public Vector2 currentSpeed;
    public Vector2 iniSpeed;
    public GameObject initPosition;
    public AudioClip hitClip;


    // Start is called before the first frame update
    void Start()
    {
        this.gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        this.ResetBall();
    }

    //Update is called once per frame

    void FixedUpdate()
    {
        if (this.canMove)
        {
            Vector2 newRelativePosition = this.currentSpeed * Time.fixedDeltaTime;
            this.transform.Translate(newRelativePosition.x, newRelativePosition.y, 0.0f);
        }
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

    private void OnDisable()
    {
        this.gameManager.LifeLost();
    }

    float hitFactor(Vector2 ballPos, Vector2 racketPos,
                float racketWidth)
    {
        return (ballPos.x - racketPos.x) / racketWidth;
    }

    private void ResetSpeed()
    {
        this.currentSpeed = this.iniSpeed;

        return;
    }
    private void ResetPosition()
    {
        this.transform.position = this.initPosition.transform.position;

        return;
    }

    private void Bounce(Collision2D collision)
    {

        var speed = this.currentSpeed.magnitude;
        Vector2 direction;

        AudioManager.instance.PlaySingle(this.hitClip);

        if (collision.gameObject.GetComponent<Brick>() != null)
        {
            direction = currentSpeed.normalized;

            if (collision.gameObject.transform.position.y > this.gameObject.transform.position.y)
            {
                direction.y = Mathf.Abs(direction.y) * -1;
            }
            else
            {
                direction.y = Mathf.Abs(direction.y);
            }

            if (collision.gameObject.transform.position.x > this.gameObject.transform.position.x)
            {
                direction.x = Mathf.Abs(direction.x) * -1;
            }
            else
            {
                direction.x = Mathf.Abs(direction.x);
            }
        }
        else
        {
            direction = Vector2.Reflect(currentSpeed.normalized, collision.contacts[0].normal);
        }


        if (collision.gameObject.tag == "Player")
        {
            float x = hitFactor(transform.position, collision.collider.transform.position, collision.collider.bounds.size.x);
            direction.x = x;
        }


        this.currentSpeed = direction * speed;
    }

    public void IncrementSpeed()
    {
        this.currentSpeed = this.currentSpeed * (1f + this.incrementStep);
    }

    public void DecrementSpeed()
    {
        this.currentSpeed = this.currentSpeed * 1 / (1f + this.incrementStep);
    }

    public void SetMove(bool moveState)
    {
        this.canMove = moveState;
    }

    public void ResetBall()
    {
        this.SetMove(false);
        this.ResetSpeed();
        this.ResetPosition();
        this.gameObject.SetActive(true);
    }

    public void PlayBall()
    {
        this.SetMove(true);
    }
}
