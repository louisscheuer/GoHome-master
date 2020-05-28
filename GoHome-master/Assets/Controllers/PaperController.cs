using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperController : MonoBehaviour
{
    public Rigidbody2D paperrb;
    public SpriteRenderer sprite;
    public float thrust = 0f;
    private float _rotateSpeed = 650.0f;
    private float _throw = 70.0f;

    // Start is called before the first frame update
    void Start()
    {
        paperrb = gameObject.GetComponent<Rigidbody2D>();
        paperrb.AddForce(transform.up * _throw);
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, _rotateSpeed * Time.deltaTime);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        _rotateSpeed = 0f;

        if (collision.gameObject.tag == "Player")
        {
            Rigidbody2D alienrb = collision.gameObject.GetComponent<Rigidbody2D>();
            var controller = collision.gameObject.GetComponent<AlienController>();

            float force = GetCollisionForce(paperrb.velocity.magnitude);

            if (!controller.CharacterMoving())
            {
                paperrb.AddForce(transform.up * thrust);

                var dir = collision.gameObject.transform.position.x - collision.contacts[0].point.x;
                var vector = new Vector2
                {
                    x = dir,
                    y = 0
                };

                alienrb.AddForce(vector * force);
            }
        }
        if (collision.gameObject.tag == "Floor")
        {
          Destroy(this.gameObject);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
      if (collision.gameObject.tag == "GardenFence")
      {
            // Debug.Log("HitGardenFence");
            sprite.sortingLayerName = "Gardens";
      }
    }

    private float GetCollisionForce(float magnitude)
    {
        var force = Math.Abs(magnitude);
        if (force < 0)
        {
            return 0;
        }
        else if (force < 200)
        {
            return 200;
        }
        else if (force > 500)
        {
            return 500;
        }

        return force;
    }
}
