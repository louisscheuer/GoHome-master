using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Controllers.Services;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public float BallStickTime;
    private bool _ballStuck;
    private TimerService _timer;

    private BallController _ballController;

    // Start is called before the first frame update
    void Start()
    {
        _timer = new TimerService();
        _timer.SetTimer(BallStickTime);
    }

    // Update is called once per frame
    void Update()
    {
        _timer.IncrementTimer(Time.deltaTime);

        if (_timer.CheckTimer())
        {
            UnstickBall();

            _timer.ResetTimer();
        }
    }

    private void UnstickBall()
    {
        _ballController.UnstickFromTree();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Ball")
        {
            _ballController = collider.GetComponent<BallController>();
            _ballController.StuckInTree();

            _timer.StartTimer();
        }
    }
}
