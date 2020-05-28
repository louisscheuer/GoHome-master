using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject explosion;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Don't collide with the object that spawned you. 
        if (collision.gameObject.tag != "BombSpawner")
        {
            Explode();
        }
    }

    public void Explode()
    {
        // Create explosion and destroy self.
        Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
