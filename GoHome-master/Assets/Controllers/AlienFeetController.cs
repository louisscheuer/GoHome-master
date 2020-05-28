using System.Collections;
using System.Collections.Generic;
using Assets.Controllers.Models;
using UnityEngine;

public class AlienFeetController : MonoBehaviour
{
    public GameObject alien;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Layers.Ground)
        {
            var controller = alien.GetComponent<AlienController>();
            controller.SetIsOnFloor(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Layers.Ground)
        {
            var controller = alien.GetComponent<AlienController>();
            controller.SetIsOnFloor(false);
        }
    }
}
