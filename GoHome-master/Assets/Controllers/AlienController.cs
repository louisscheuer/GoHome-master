using System.Collections;
using System.Collections.Generic;
using Assets.Controllers.Models;
using UnityEngine;

public class AlienController : MonoBehaviour
{
    public float speed;
    public float crouchSpeed;
    public float jumpForce;
    public float birdtimer;

    public bool Sticky { get; set; }
    public bool IsOnFloor { get; set; }
    public bool IsNudged { get; set; }
    public bool IsMovingHorizontally { get; set; }

    public GameObject shakeresidue;
    public Rigidbody2D rb;
    public BoxCollider2D AlienCollider;
    public Animator anim;
    public GameObject beam;

    private bool _isCrouching = false;
    private bool _isBeamed = false;
    private bool _isUnderFloor = false;
    private bool _isFacingRight = true;
    private bool _isBeaming = false;
    private bool _isKeying = false;
    private bool _birdtimeron = false;
    private float _speed;
    private float _lastKnownVelocity;

    void Start()
    {
        rb = GetRigidBody();
        AlienCollider = GetComponent<BoxCollider2D>();
        anim = gameObject.GetComponent<Animator>();
        _speed = speed;

        anim.speed = 0.3f;

        birdtimer = 0f;

    }

    void Update()
    {
        if (!_isBeamed)
        {
            UpdateCrouching();
            UpdateJumping();
        }

        if (IsNudged)
        {
            var beamcontroller = beam.GetComponent<BeamController>();
            if (beamcontroller._beaming || beamcontroller._beamed)
            {
                var beamcontroller2 = beam.GetComponent<BeamController>();
                beamcontroller2.ReverseAnimation();
            }
        }

        // if (_birdtimeron)
        // {
        //   birdtimer += Time.deltaTime;
        //   if (birdtimer > 2)
        //   {
        //     _sticky = true;
        //   }
        // }

        AnimationUpdate();
    }

    void FixedUpdate()
    {
        var horizontalInput = !_isBeamed ? Input.GetAxisRaw("Horizontal") : 0;
        if (horizontalInput != 0)
        {
            if (horizontalInput > 0 && !_isFacingRight || horizontalInput < 0 && _isFacingRight)
            {
                var theScale = gameObject.transform.localScale;
                theScale.x *= -1;
                gameObject.transform.localScale = theScale;
            }

            IsMovingHorizontally = true;
            rb.velocity = new Vector2(_speed * horizontalInput, rb.velocity.y);

            _isFacingRight = horizontalInput > 0;
        }
        else
        {
            IsMovingHorizontally = false;
        }

        _lastKnownVelocity = rb.velocity.magnitude;
    }

    public bool CharacterMoving()
    {
        return IsMovingHorizontally || _lastKnownVelocity != 0;
    }

    public Rigidbody2D GetRigidBody()
    {
        return gameObject.GetComponent<Rigidbody2D>();
    }

    public void SetIsOnFloor(bool isOnFloor)
    {
        IsOnFloor = isOnFloor;
    }

    public void SetIsUnderFloor(bool isUnderFloor)
    {
        _isUnderFloor = isUnderFloor;
    }

    public void SetIsBeaming(bool isBeaming)
    {
        _isBeaming = isBeaming;
    }

    public void SetIsBeamed(bool isBeamed)
    {
        _isBeamed = isBeamed;
    }

    public void SetIsKeying(bool isKeying)
    {
        _isKeying = isKeying;
    }

    public void BeamedUp()
    {
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        var rigidBody = gameObject.GetComponent<Rigidbody2D>();
        rigidBody.isKinematic = true;
    }

    private void UpdateCrouching()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (!_isCrouching)
            {
                _isCrouching = true;
                AlienCollider.offset = new Vector2(0.001262188f, -0.118648f);
                AlienCollider.size = new Vector2(0.2487133f, 0.4027036f);
                _speed = crouchSpeed;
            }
        }
        else
        {
            if (_isCrouching && !_isUnderFloor)
            {
                _isCrouching = false;
                AlienCollider.offset = new Vector2(0.004914999f, -0.05013318f);
                AlienCollider.size = new Vector2(0.1701241f, 0.5397336f);
                _speed = speed;
            }
        }
    }

    private void UpdateJumping()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && IsOnFloor && !_isUnderFloor)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }

    }

    private void AnimationUpdate()
    {
        anim.SetBool("IsCrouching", _isCrouching);
        anim.SetBool("IsFacingRight", _isFacingRight);
        anim.SetBool("IsOnFloor", IsOnFloor);
        anim.SetBool("IsMovingHorizontally", IsMovingHorizontally);
        anim.SetBool("IsNudged", IsNudged);
        anim.SetBool("IsBeaming", _isBeaming);
        anim.SetBool("IsKeying", _isKeying);
        anim.SetBool("IsBeamed", _isBeamed);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if ((col.gameObject.layer == Layers.Projectiles || col.gameObject.layer == Layers.Characters) & IsOnFloor & !IsMovingHorizontally)
        {
            IsNudged = true;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if ((col.gameObject.layer == Layers.Projectiles || col.gameObject.layer == Layers.Characters) & IsOnFloor & !IsMovingHorizontally)
        {
            IsNudged = true;
        }
        if (col.gameObject.tag == "Shake")
        {
            shakeresidue = GameObject.FindWithTag("shakeresidue");
            shakeresidue.GetComponent<ShakeresidueController>().ResetOpacity();
            Sticky = true;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "shakeresidue")
        {
            _birdtimeron = true;
            if (_birdtimeron)
            {
                birdtimer += Time.deltaTime;
                if (birdtimer > 2)
                {
                    Sticky = true;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "shakeresidue")
        {
            Sticky = false;
            birdtimer = 0f;
            _birdtimeron = false;
            Debug.Log("ShakeResiduenotcollidingiwthalien");
        }
    }

    void IsNotNudged()
    {
        IsNudged = false;
    }


    public void GetNudged()
    {
        float force = 100f;
        float dir;

        if (_isFacingRight)
        {
            dir = -1f;
        }
        else
        {
            dir = 1f;

        }

        var vector = new Vector2
        {
            x = dir,
            y = 0
        };

        rb.AddForce(vector * force);

        IsNudged = true;
    }

    public void Waiting()
    {
        var beamcontroller = beam.GetComponent<BeamController>();
        beamcontroller.PlayAnimation();
    }

    public void WinGame()
    {
        //Application.LoadLevel(Application.loadedLevel);
    }
}
