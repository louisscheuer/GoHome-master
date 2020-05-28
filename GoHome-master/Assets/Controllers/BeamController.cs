using System.Collections;
using System.Collections.Generic;
using Assets.Controllers.Services;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BeamController : MonoBehaviour
{
    public Animator anim;
    public GameObject alien;
    public TimerService timer;
    public bool _beaming;
    public bool _beamed;
    private bool _keying;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        anim.SetFloat("Speed", 0f);
        timer = new TimerService();
        // PauseAnimationStart();
        var controller = alien.GetComponent<AlienController>();
        controller.SetIsBeamed(false);
        _beaming = false;
        _keying = false;
        _beamed = false;
        timer.SetTimer(2);
    }

    // Update is called once per frame
    void Update()
    {
        var controller = alien.GetComponent<AlienController>();

        if (!controller.CharacterMoving() && !_keying && !_beaming && !_beamed)
        {
              controller.SetIsKeying(true);
              controller.SetIsBeaming(false);
              _keying = true;
              _beaming = false;
        }
        else if (!controller.CharacterMoving() && _beaming & !_beamed)
        {
              controller.SetIsKeying(false);
              controller.SetIsBeaming(true);
              _keying = false;
              _beaming = true;
              PlayAnimation();
        }
        else if (!controller.CharacterMoving() && _beamed)
        {
          // _beamed = true;
          PauseAnimationEnd();
          }
        else if (controller.CharacterMoving() && _keying)
        {
              controller.SetIsKeying(false);
              controller.SetIsBeaming(false);
              _keying = false;
              _beaming = false;
        }
        else if (controller.CharacterMoving() && _beaming)
        {
                timer.ResetTimer();
                ReverseAnimation();
                controller.SetIsBeaming(false);
                controller.SetIsKeying(false);
                _keying = false;
                _beaming = false;
        }
        else if ((controller.IsNudged|| controller.IsMovingHorizontally || controller.IsOnFloor == false) && _beaming)
        {
            timer.ResetTimer();
            // Debug.Log("ShouldReverse");
            ReverseAnimation();
        }


        // else if (controller.IsMovingHorizontally)
        // {
        //   timer.ResetTimer();
        //   Debug.Log("ShouldReverse");
        //   ReverseAnimation();
        // }


    }

    public void PlayAnimation()
    {
        anim.SetFloat("Speed", 1f);
        _beaming = true;
        _keying = false;
    }

    public void PauseAnimationStart()
    {

        anim.SetFloat("Speed", 0f);
        _beaming = false;
        _keying = false;
        // Debug.Log("BeamBack2Start");
    }

    public void PauseAnimationEnd()
    {
        anim.SetFloat("Speed", 0f);
        var controller = alien.GetComponent<AlienController>();
        controller.SetIsBeamed(true);
        _beamed = true;
        // Debug.Log("BeamAtEnd");
    }

    public void ReverseAnimation()
    {
        anim.SetFloat("Speed", -2f);
        // Debug.Log("ShouldReallyBeReversing");
        var controller = alien.GetComponent<AlienController>();
        controller.SetIsBeaming(false);
        controller.SetIsBeamed(false);
        _beaming = false;
        _beamed = false;
        timer.ResetTimer();
    }
}
