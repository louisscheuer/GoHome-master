using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public GameObject AlienTop;
    public GameObject Alien;
    public GameObject ShakeResidue;
    public Rigidbody2D rb;
    public Animator anim;
    private float speed = 1f;
    private GameObject ClosestTree;
    private Transform TreeTransform;
    private int _birdAction;
    private float lastPositionX;
    private float currentPositionX;
    private bool _isFacingRight;
    private bool _birdIdle;

    // Start is called before the first frame update
    void Start()
    {
        AlienTop = GameObject.FindWithTag("AlienTop");
        Alien = GameObject.FindWithTag("Player");
        ShakeResidue = GameObject.FindWithTag("shakeresidue");
        rb = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        lastPositionX = transform.position.x;
        _isFacingRight = true;
        _birdIdle = false;

        //_birdaction: 1, Still. 2, Idle. 3, Fiddling. 4, Flying. 5, Pecking - DO NOT DELETE
    }

    // Update is called once per frame
    void Update()
    {
        currentPositionX = transform.position.x;

        ClosestTree = FindTarget();

        if (Alien.GetComponent<AlienController>().Sticky && transform.position.x > (Alien.transform.position.x - 2) && transform.position.x < (Alien.transform.position.x + 2))
        {
            _birdIdle = false;
            var step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, AlienTop.transform.position, step);
            if ((_birdAction == 1) || (_birdAction == 2) || (_birdAction == 3))
            {
                _birdAction = 4;
            }
        }
        else
        {
            var step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, ClosestTree.transform.position, step);
        }

        if ((gameObject.transform.position.x == ClosestTree.transform.position.x) && (gameObject.transform.position.y == ClosestTree.transform.position.y))
        {
            _birdIdle = true;
            if (currentPositionX != lastPositionX)
            {
                BirdIdle();
            }
        }
        else
        {
            _birdIdle = false;
        }


        AnimationUpdate();

        //_isFacingRight checks along with setting the lastPositionX in start()


        if ((currentPositionX < lastPositionX) & _isFacingRight)
        {
            Flip();
            _isFacingRight = false;
        }
        else if ((currentPositionX > lastPositionX) & !_isFacingRight)
        {
            Flip();
            _isFacingRight = true;
        }

        lastPositionX = currentPositionX;
    }

    public GameObject FindTarget()
    {
        var gos = GameObject.FindGameObjectsWithTag("Tree");
        GameObject closest = null;
        var distance = Mathf.Infinity;
        var position = transform.position;

        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        return closest;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == AlienTop)
        {
            _birdAction = 5;
            _birdIdle = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject == Alien) || (collision.gameObject == AlienTop) || (collision.gameObject == ClosestTree))
        {
            _birdIdle = false;
            _birdAction = 4;
        }
    }

    private void AnimationUpdate()
    {
        anim.SetInteger("BirdAction 0", _birdAction);
    }

    protected void Flip()
    {
        var theScale = gameObject.transform.localScale;
        theScale.x *= -1;
        gameObject.transform.localScale = theScale;
    }

    private void BirdIdle()
    {
        if (_birdIdle)
        {
            _birdAction = Random.Range(1, 4);
        }

    }

    private void BirdIdle2()
    {
        if (_birdIdle)
        {
            _birdAction = Random.Range(1, 3);
            // if (_isFacingRight = false)
            // {
            //   Flip();
            //   _isFacingRight = true;
            // }
        }
    }

    private void BirdPeck()
    {
        var alien = GameObject.FindWithTag("Player");
        var alienController = alien.gameObject.GetComponent<AlienController>();
        alienController.GetNudged();
        _birdIdle = false;
    }

}
