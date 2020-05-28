using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBoyController : MonoBehaviour
{
    public Animator anim;
    public GameObject paper;
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
      anim = gameObject.GetComponent<Animator>();
      anim.speed = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
      transform.Translate(speed,0,0);

    }

    private void EndLoop()
    {
      anim.SetInteger("ThrowCount", anim.GetInteger("ThrowCount") + 1);
    }

    private void ThrowPaper()
    {
      anim.SetInteger("ThrowCount", 0);
      Instantiate(paper,new Vector2 (transform.position.x, transform.position.y), Quaternion.identity);
    }
}
