using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeresidueController : MonoBehaviour
{

    public Animator anim;
    public GameObject alien;
    private float _timer = 0f;
    public float _opacity = 1f;
    private SpriteRenderer sprite;
    private Material m_Material;
    public Color altColor;

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();

        _opacity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        altColor.r = 1;
        altColor.g = 0;
        altColor.b = 1;
        altColor.a = _opacity;

        sprite.material.color = altColor;

        if (_opacity > 0.01)
        {
                _timer += Time.deltaTime;
        }
        if (_timer > 3)
        {
          _opacity -= 0.01f;
        }
        if (_opacity < 0.01)
        {
          _opacity = 0;
          _timer = 0f;
          SetInActive();
        }
    }

    public void SetInActive()
    {
      alien = GameObject.FindWithTag("Player");
      var AlienController = alien.GetComponent<AlienController>();
      AlienController.Sticky = false;
      // GetComponent<SpriteRenderer>().enabled = false;
    }

    public void ResetOpacity()
    {
        _opacity = 1;
    }
}
