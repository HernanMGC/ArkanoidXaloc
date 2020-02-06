using System.Collections;
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

        if (hitLeft.transform != null)
        {
            this.boundaryLeft = hitLeft.transform.position.x + characterOffset;
            Debug.Log("B Left " + this.boundaryLeft);
            Debug.Log("B Left.X " + hitLeft.transform.position.x);
        }
        else {
            Debug.Log("DANG!" + this.boundaryLeft);
        }

        if (hitRight.transform != null)
        {
            this.boundaryRight = hitRight.transform.position.x - characterOffset;
            Debug.Log("B Right" + this.boundaryRight);
            Debug.Log("B Right.X " + hitRight.transform.position.x);
        }
        else
        {
            Debug.Log("DANG2!" + this.boundaryRight);
        }

        //this.boundaryLeft = -0.6f;
        //this.boundaryRight = 0.6f;

        return;
    }
}
