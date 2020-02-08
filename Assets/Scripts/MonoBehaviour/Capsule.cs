using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Capsule : MonoBehaviour
{
    private readonly float vo = 0.0f;
    private float v;
    private float y;

    public float acceleration = 0.001f;

    // Start is called before the first frame update
    void Start()
    {
        this.y = this.transform.position.y;
        this.v = this.vo;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.v = this.v - this.acceleration * (Time.fixedDeltaTime);
        this.transform.Translate(0, this.v, 0);
    }

    public virtual void ApplyEffect() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            this.ApplyEffect();
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Boundary" && collision.gameObject.GetComponent<Hitable>().hitableReaction == Hitable.HitableReaction.DestroyHitter)
        {
            Destroy(this.gameObject);
        }
    }
}
