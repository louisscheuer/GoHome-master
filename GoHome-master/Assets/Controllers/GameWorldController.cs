using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorldController : MonoBehaviour
{
    public GameObject PaperBoy;
    // Do we need this and the one on the plane?
    public void OnTriggerExit2D(Collider2D collider)
    {
        Destroy(collider.gameObject);
        if(collider.gameObject.name == "PaperBoy")
        {
            Instantiate(PaperBoy,new Vector2 (transform.position.x, transform.position.y), Quaternion.identity);
        }
    }
}

//Alien can't jump when standing on ball                            L&A
//Alien goes VERY weird when a paper is on his head for a while     L&A
//Animate & assign Alien Jump, Alien Sneak Beamed                   L
//Animate + program kid coming out                                  L
//Have kid nudge alien                                              L&A       Sot of- does so in -x direction every time
//Make tree sprite                                                  A
//Finish cat sprites - pounce, spooked, run                         L
//Assign cat sprites                                                L&A
//Finish cat behaviour - including some way to keep him at bay      A


//Polished animations + sprites all round
//Kid in pram, throws toy, mum collects it
//Car that appears after Paper Boy in cycle, throw a drink at you
//Bird which pecks on your head, especially when you've got drink on you, shits on you otherwise
//Another "enemy" that follows the car before it cycles back round to the paper boy

// Bugs!
// Alien can't jump while crouch walking left
// Aliens collision isn't as tall as his sprite
