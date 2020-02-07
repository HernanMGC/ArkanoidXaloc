﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{  
    private Vector2 currentSpeed;
    private float boundaryLeft;
    private float boundaryRight;
    private BoxCollider2D characterCollider;

    public Vector2 iniSpeed;

    // Start is called before the first frame update
    void Start()
    {
        this.characterCollider = this.GetComponent<BoxCollider2D>();

        this.ResetSpeed();
        this.InitializeBoundaries();

        Debug.Log(characterCollider);
    }

    //Update is called once per frame

    void FixedUpdate()
    {
        // Get Horizontal Input
        float hMove = Input.GetAxisRaw("Horizontal");
        
        Vector2 currentPosition = new Vector2(Mathf.Clamp(this.transform.position.x, boundaryLeft, boundaryRight), this.transform.position.y);
        Vector2 newPosition = currentPosition + this.currentSpeed * hMove * Time.fixedDeltaTime;
        this.transform.position = newPosition;
    }

    private void ResetSpeed()
    {
        this.currentSpeed = this.iniSpeed;

        return;
    }

    private void InitializeBoundaries()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(this.transform.position, Vector2.left, Mathf.Infinity);
        RaycastHit2D hitRight = Physics2D.Raycast(this.transform.position, Vector2.right, Mathf.Infinity);
        float characterOffset = this.characterCollider.size.x / 2;
        float spriteWidth = this.GetComponent<SpriteRenderer>().bounds.size.x;
        float colliderWidthX = this.GetComponent<BoxCollider2D>().size.x;
        float colliderOffsetX = this.GetComponent<BoxCollider2D>().offset.x;
        Debug.Log("spriteWidth " + spriteWidth);
        Debug.Log("colliderWidthX " + colliderWidthX);
        Debug.Log("colliderOffsetX " + colliderOffsetX);

        if (hitLeft.transform != null)
        {
            this.boundaryLeft = hitLeft.point.x + spriteWidth/2;
            if (colliderOffsetX > 0) {
                this.boundaryLeft += colliderOffsetX * 2;
            }
            Debug.Log("B Left " + this.boundaryLeft);
            Debug.Log("B Left.X " + hitLeft.point.x);
        }

        if (hitRight.transform != null)
        {
            this.boundaryRight = hitRight.point.x - spriteWidth/2;
            if (colliderOffsetX < 0)
            {
                this.boundaryRight -= colliderOffsetX * 2;
            }
            Debug.Log("B Right" + this.boundaryRight);
            Debug.Log("B Right.X " + hitRight.point.x);
        }

        //this.boundaryLeft = -0.6f;
        //this.boundaryRight = 0.6f;

        return;
    }
}
