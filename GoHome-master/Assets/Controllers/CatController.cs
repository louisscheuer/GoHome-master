using Assets.Controllers.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// Cat:
//  Little bastard that follows the player around and pounces on him.
//  Stalker:    Follows the player from a distance, should always turn up when the player least wants him to.
//  Vicious:    Hates the player, likes to jump on the players head.
//  Cowardly:   Runs from the player when the player deliberately moves towards it.


// TODO:
// Refactor
// Add minimum speed for spooking so player getting nudged doesnt trigger it
// Spook cat when he gets hit by ball
// Add timer for cat after he's run away
// Hiding functionality

public class CatController : MonoBehaviour
{
    public GameObject alien;
    public Rigidbody2D rb;
    public Animator anim;

    // Cats Speed
    public float walkingSpeed = 1f;
    public float prowlingSpeed = 0.02f;
    public float runningSpeed = 0.2f;

    // Cat's state
    public bool IsRunningAway
    {
        get
        {
            return _runningAway;
        }
        set
        {
            _runningAway = value;
            anim.SetBool("IsRunningAway", value);
        }
    }

    public bool IsIdle
    {
        get
        {
            return _isIdle;
        }
        set
        {
            _isIdle = value;
            anim.SetBool("IsIdle", value);
        }
    }

    public bool IsPreparing
    {
        get
        {
            return _isPreparing;
        }
        set
        {
            _isPreparing = value;
            anim.SetBool("IsPreparing", value);

        }
    }

    public bool IsOnFloor
    {
        get
        {
            return _isOnFloor;
        }
        set
        {
            _isOnFloor = value;
            anim.SetBool("IsOnFloor", value);
        }
    }

    private bool _runningAway;
    private bool _isIdle;
    private bool _isPreparing;
    private bool _isOnFloor;

    public bool HasPounced { get; set; }

    // Distance of the player to trigger the players behaviour
    public float pounceDistance = 2f;
    public float runDistance = 5f;
    public float spookDistance = 2f;

    // Cat behaviour timing
    public float idleTime = 3f;
    public float pounceTime = 1f;

    // Jump force for cat
    public float pounceY = 100f;
    public float pounceX = 100f;

    // Last known position of the player (for calculating change in distance.
    private float _lastPlayerPostion;
    private float _catSpeed;

    private TimerService _idleTimer = new TimerService();
    private TimerService _prepareTimer = new TimerService();

    // Start is called before the first frame update
    void Start()
    {
        _catSpeed = walkingSpeed;

        anim.speed = 0.1f;

        SetCatIdle(true);

        // Flip();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        Rigidbody2D alienrb = alien.GetComponent<Rigidbody2D>();
        var relativePosition = GetPlayersRelativeXPostion(alienrb);
        Debug.Log(IsRunningAway);
        var beingChased = CheckIfPlayerChasingCat(relativePosition, _lastPlayerPostion);

        if (beingChased)
        {
            StartRunningAway();
        }

        if (IsRunningAway)
        {
            if (CheckIfFarEnoughAway(relativePosition))
            {
                IsRunningAway = false;
                SetCatIdle(true);
            }
            else
            {
                if (IsOnFloor && rb.velocity.y == 0)
                {
                    RunAwayFromPlayer(relativePosition);
                }
            }

        }
        else if (IsIdle)
        {
            if (_idleTimer.CheckTimer())
            {
                SetCatIdle(false);
            }

            _idleTimer.IncrementTimer(Time.deltaTime);
        }
        else
        {
            if (Math.Abs(relativePosition) < spookDistance)
            {
                //cat prepares to pounce, is very slow
                _catSpeed = prowlingSpeed;
            }
            else if (Math.Abs(relativePosition) > spookDistance)
            {
                //cat is prowling normally
                _catSpeed = walkingSpeed;
            }


            if (Math.Abs(relativePosition) > pounceDistance)
            {
                HasPounced = false;
                MoveTowardsPlayer(relativePosition);
            }
            else if (!HasPounced)
            {
                if (!IsPreparing)
                {
                    PrepareToPounce();
                }
                else
                {

                    if (_prepareTimer.CheckTimer())
                    {
                        IsPreparing = false;
                        Pounce(relativePosition);
                        StartRunningAway();
                    }
                    else
                    {
                        _prepareTimer.IncrementTimer(Time.deltaTime);
                    }
                }
            }
        }

        anim.SetBool("IsMoving", !IsIdle && !IsRunningAway && !IsPreparing);
        _lastPlayerPostion = relativePosition;
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        // While the ball is on a floor set isOnFloor to true.
        if (collision.gameObject.CompareTag("Floor"))
        {
            IsOnFloor = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        // While the ball is on a floor set isOnFloor to true.
        if (collision.gameObject.CompareTag("Floor"))
        {
            IsOnFloor = false;
        }
    }

    private void SetCatIdle(bool catIdle)
    {
        if (catIdle)
        {
            IsIdle = true;
            _idleTimer.SetTimer(idleTime);
            _idleTimer.ResetAndStartTimer();
        }
        else
        {
            IsIdle = false;
            _idleTimer.ResetTimer();
        }

    }

    private void PrepareToPounce()
    {
        IsPreparing = true;
        _prepareTimer.SetTimer(pounceTime);
        _prepareTimer.ResetAndStartTimer();
    }

    private void Pounce(float relativePosition)
    {
        HasPounced = true;
        var direction = GetPlayerDirection(relativePosition);
        var jumpForce = new Vector2(pounceX * direction, pounceY);
        rb.AddForce(jumpForce);
    }

    private bool CheckIfFarEnoughAway(float relativePosition)
    {
        return Math.Abs(relativePosition) > runDistance;
    }

    private void MoveTowardsPlayer(float relativePosition)
    {
        var direction = GetPlayerDirection(relativePosition);
        rb.velocity = new Vector2(_catSpeed * direction, 0);
    }

    private void RunAwayFromPlayer(float relativePosition)
    {
        var direction = GetPlayerDirection(relativePosition);
        rb.velocity = new Vector2(-1 * _catSpeed * direction, 0);
    }

    private void StartRunningAway()
    {
        if (!IsRunningAway)
        {
            // Flip();
        }
        IsRunningAway = true;
        _catSpeed = runningSpeed;

    }

    private float GetPlayersRelativeXPostion(Rigidbody2D alienrb)
    {
        return alienrb.position.x - rb.position.x;
    }

    private float GetPlayerDirection(float relativePosition)
    {
        if (relativePosition >= 0)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    private bool CheckIfPlayerChasingCat(float relativePosition, float lastPlayerPostion)
    {
        Rigidbody2D alienrb = alien.GetComponent<Rigidbody2D>();

        var distanceToAlien = Math.Abs(relativePosition);

        if (alienrb.velocity.x > 0)
        {
            // alien is walking right
            if (alien.transform.position.x > transform.position.x)
            {
                //the cat is behind the alien
                return false;
            }
            else
            {
                //the cat is in front of the alien
                if (spookDistance < distanceToAlien)
                {
                    //cat is not being chased
                    return false;
                }
                else
                {
                    //cat is being chased, and spooks
                    return true;
                }
            }
        }
        if (alienrb.velocity.x < 0)
        {
            // alien is walking left
            if (alien.transform.position.x < transform.position.x)
            {
                //the cat is behind the alien
                return false;
            }
            else
            {
                //the cat is in front of the alien
                if (spookDistance < distanceToAlien)
                {
                    //cat is not being chased
                    return false;
                }
                else
                {
                    //cat is being chased, and spooks
                    return true;
                }
            }
        }

        return false;
    }

    private void Flip()
    {
      var theScale = gameObject.transform.localScale;
      theScale.x *= -1;
      gameObject.transform.localScale = theScale;
    }
}
