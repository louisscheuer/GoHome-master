using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidController : MonoBehaviour
{
    public GameObject Ball;
    private GameObject fence;
    public Animator anim;
    public Rigidbody2D rb;

    public float KidSpeed = 0.04f;
    public float TimeBetweenJumps = 1f;
    public float JumpForce = 2f;

    private float _speed;

    // Kid states
    private bool _hasBall = false;
    private bool _isReaching = false;

    // Kid motion
    private float _ballTransform;
    private float _minimumTransformY;
    private bool isFacingRight;

    // Kid jumping fields
    private bool _isOnFloor;
    private bool _canJump = true;
    private float _canJumpTimer;


    void Start()
    {
        fence = GameObject.Find("Fence");

        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        _speed = KidSpeed;

        anim.speed = 0.15f;

        // Limitation: Kid must spawn at lowest point, meaning fence can't be on a hill.
        _minimumTransformY = gameObject.transform.position.y;
    }

    void Update()
    {
        UpdateJumpTimer();
        SetKidSpeed();

        Ball = GameObject.Find("Ball(Clone)");
        if (Ball)
        {
            _ballTransform = Ball.transform.position.x;
        }
        else
        {
            _ballTransform = 0f;
        }


        var kidXPostion = gameObject.transform.position.x;

        if (gameObject.transform.position.y < _minimumTransformY)
        {
            gameObject.transform.position = new Vector2(kidXPostion, _minimumTransformY);
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        if (!_hasBall)
        {
            if ((_ballTransform > (kidXPostion + 0.1)) & _canJump)
            {
                transform.Translate(_speed, 0, 0);
                _isReaching = false;
                if (!isFacingRight)
                {
                    Flip();
                    isFacingRight = true;
                    _isReaching = false;

                }
            }
            else if ((_ballTransform < (kidXPostion - 0.1)) & _canJump)
            {
                transform.Translate(-_speed, 0, 0);
                _isReaching = false;
                if (isFacingRight)
                {
                    Flip();
                    isFacingRight = false;
                }
            }
            else
            {
                _speed = 0;
                _isReaching = true;
                if (_isOnFloor && _canJump)
                {
                    _canJump = false;

                    rb.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
                }
            }
        }
        else if (_hasBall)
        {
            transform.Translate(_speed, 0, 0);

            if (!isFacingRight)
            {
                Flip();
                isFacingRight = true;
            }
        }

        UpdateAnimation();
    }

    private void SetKidSpeed()
    {
        if (_isReaching)
        {
            _speed = 0f;
        }
        else
        {
            _speed = KidSpeed;
        }
    }

    private void UpdateAnimation()
    {
        anim.SetBool("isReaching", _isReaching);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ball")
        {
            Destroy(col.gameObject);
            _hasBall = true;
            _isReaching = false;
            _speed = KidSpeed;
            anim.SetBool("_hasBall", _hasBall);
            transform.Translate(_speed, 0, 0);
            if (!isFacingRight)
            {
                Flip();
                isFacingRight = true;
            }
        }

        if (col.gameObject.name == "LastHouse")
        {
            if (_hasBall)
            {
                fence.GetComponent<FenceController>().KidReturned();
                Destroy(gameObject);
            }
        }

        if (col.gameObject.tag == "Player")
        {
            var controller = col.gameObject.GetComponent<AlienController>();

            if (!controller.CharacterMoving())
            {
                controller.GetNudged();
            }
        }

    }

    public void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Floor")
        {
            _isOnFloor = true;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Floor")
        {
            _isOnFloor = false;
        }
    }

    protected void Flip()
    {
        var theScale = gameObject.transform.localScale;
        theScale.x *= -1;
        gameObject.transform.localScale = theScale;
    }

    private void UpdateJumpTimer()
    {
        if (!_canJump)
        {
            _canJumpTimer += Time.deltaTime;

            if (_canJumpTimer > TimeBetweenJumps)
            {
                _canJump = true;
                _canJumpTimer = 0f;
            }
        }
    }

    


}
