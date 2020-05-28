using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeController : MonoBehaviour
{
    public Rigidbody2D shakerb;
    public Vector2 shakethrust;
    private float _rotateSpeed = 650.0f;
    // Start is called before the first frame update
    void Start()
    {
      shakethrust = new Vector2(-3,2);
      shakerb = gameObject.GetComponent<Rigidbody2D>();
      shakerb.AddForce(shakethrust, ForceMode2D.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, _rotateSpeed * Time.deltaTime);

    }

    void OnTriggerStay2D(Collider2D collision)
    {
      if (collision.gameObject.tag == "Player")
      {
        Destroy(this.gameObject);
      }
    }
}
