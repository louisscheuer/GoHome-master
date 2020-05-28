using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienTopController : MonoBehaviour
{
    public GameObject alien;
    // TODO: Do we need to Floor tags here? If so why?

    void OnTriggerStay2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Floor")
        {
            var controller = alien.GetComponent<AlienController>();
            controller.SetIsUnderFloor(true);
        }
    }

    void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Floor")
        {
            var controller = alien.GetComponent<AlienController>();
            controller.SetIsUnderFloor(false);
        }
    }

}
