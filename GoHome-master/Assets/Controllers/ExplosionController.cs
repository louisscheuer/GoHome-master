using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplosionController : MonoBehaviour
{
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        anim.speed = 0.4f;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            KillPlayer();
        }
    }

    //  TODO: Once theres a few things that can all kill the player we should move KillPlayer() onto the player
    //  and have the objects call that instead. 
    public void KillPlayer()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void RemoveObject()
    {
        Destroy(gameObject);
    }
}
