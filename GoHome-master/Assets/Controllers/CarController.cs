using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float carspeed = 0.1f;
    public GameObject shake;
    public GameObject alien;
    private float _timer = 0;
    private bool _thrown = false;
    // Start is called before the first frame update
    void Start()
    {
        alien = GameObject.Find("alien");
    }

    // Update is called once per frame
    void Update()
    {
              transform.Translate(carspeed,0,0);

              if (gameObject.transform.position.x > alien.transform.position.x)
              {
                _timer += Time.deltaTime;
              }

              if (!_thrown && _timer > 0.3)
              {
                Instantiate(shake,new Vector2 (transform.position.x, transform.position.y), Quaternion.identity);
                _thrown = true;
              }
    }
}
