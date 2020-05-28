using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ball:
//  -   Round object: Moves horizontally by rolling
//  -   Bouncy: When it hits a wall it bounces off with less speed than before
//  -   Nudgemeister: When it hits the player AND the player is not moving it nudges the player
//  -   Max Speed: The ball cannot move faster than it's maximum speed
//  -   Polite: When stood on the ball stops completely and allows the player to beam away
//  -   Corporeal: The ball doesnt phase through walls when pushed against them, for christs sake.
//  -   Over Fence: Make it so it doesnt nudge Player when it first appears behind wall, already start w/o collision.

public class BallController : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject alien;
    public float maxSpeed = 0f;
    public float thrust = 0f;
    public float rotatyBoi = 300;
    private double _lastknownwhereabouts;
    private bool _isStill;
    private bool _isOnFloor;
    private bool _beingStoodOn;
    private SpriteRenderer sprite;

    public void Start()
    {
        rb.AddForce(new Vector2(-2, 2) * thrust);

        sprite = this.gameObject.GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
      sprite = this.gameObject.GetComponent<SpriteRenderer>();
      // if (rb.velocity.y < -1)
      // {
      //   sprite.sortingLayerName = "Default";
      //   GetComponent<CircleCollider2D>().enabled = true;
      // }
    }

    public void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        _isStill = CheckIfStill(rb.position);
        _lastknownwhereabouts = GetWhereabouts(rb.position);

        if (!IsBallFrozen() && CanFreezeBall())
        {
            FreezeBall();
        }

        if (_isOnFloor)
        {
            // Update the ball's rotation while it is on the floor so that it rolls in a satisfying way.
            UpdateBallRotation();
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Multiple types of collision, Big Hit , Small hit and No Hit? Depending on the velocity of the ball
            // So if ball super slow force = 0 , If ball slow force = smallHitForce, If ball fast force = bigHitForce
            // Make bigHitForce, smallHitForce and the cutOffVelocity (e.g If (rb.velocity.x < cutOffVelocity) {force = 0}
            // for small hit be public variable (so we can set them in unity)
            Rigidbody2D alienrb = collision.gameObject.GetComponent<Rigidbody2D>();
            var controller = collision.gameObject.GetComponent<AlienController>();

            float force = GetCollisionForce(rb.velocity.magnitude);

            if (!controller.CharacterMoving())
            {
                rb.AddForce(transform.up * thrust);

                var dir = collision.gameObject.transform.position.x - collision.contacts[0].point.x;
                var vector = new Vector2
                {
                    x = dir,
                    y = 0
                };

                alienrb.AddForce(vector * force);
            }
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        // While the ball is on a floor set isOnFloor to true.
        if (collision.gameObject.tag == "Floor")
        {
            _isOnFloor = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        // New Code: Track whether ball is on the floor and don't freeze the ball if it is in mid air.
        if (collision.gameObject.tag == "Floor")
        {
            // When ball freezed it gets set to kinematic which causes OnCollisionExit to fire, ensure that this does not make the ball not be on the floor.
            if (!IsBallFrozen())
            {
                _isOnFloor = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "AlienFeet")
        {
            _beingStoodOn = false;
            rb.isKinematic = false;
            var alien = collider.gameObject.GetComponentInParent<Rigidbody2D>();

            // NEW CODE: Impart some of the aliens velocity to the ball when you jump off it so you can do pro football moves.
            rb.velocity = new Vector2(alien.velocity.x * 0.5F, alien.velocity.y * 0.2F);
        }
        if (collider.gameObject.name == "Fence")
        {
          sprite.sortingLayerName = "Default";
          GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "AlienFeet")
        {
            _beingStoodOn = true;
        }
    }

    public void StuckInTree()
    {
        rb.velocity = new Vector2(0, 0);
        rb.isKinematic = true;
    }

    public void UnstickFromTree()
    {
        rb.isKinematic = false;
    }

    private double GetWhereabouts(Vector2 position)
    {
        return Math.Round(position.x, 2) + Math.Round(position.y, 2);
    }

    private bool CheckIfStill(Vector2 position)
    {
        return _lastknownwhereabouts == GetWhereabouts(rb.position); ;
    }

    private bool CanFreezeBall()
    {
        // Only freeze ball if it is not moving and is on a floor and is currently being stood on.
        return _isStill && _isOnFloor && _beingStoodOn;
    }

    private bool IsBallFrozen()
    {
        return rb.isKinematic;
    }

    private void FreezeBall()
    {
        rb.isKinematic = true;
        rb.velocity = new Vector2();
    }

    // Ensure ball rotates realistically by matching it's angular rotation to its horizontal movement.
    private void UpdateBallRotation()
    {
        rb.angularVelocity = -1 * rb.velocity.x * rotatyBoi;
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

//shouldn't make game jitter when being pushed against a wall by Alien (doesn't that much)
//should nudge alien AT ALL TIMES (is CharacterMoving really working?)
//must succesfully get nudged upwards when hit by alien (quite rarely does)
