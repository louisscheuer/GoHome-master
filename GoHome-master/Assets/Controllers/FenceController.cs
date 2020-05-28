using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceController : MonoBehaviour


{
    public GameObject ball;
    public GameObject kid;
    private float _timer;
    private float _ballTime;
    private float _kidTime;
    private bool _ballOut;
    private bool _kidOut;
    // Start is called before the first frame update
    void Start()
    {
        _ballTime = 3;
        _kidTime = 6;
        _ballOut = false;
        _kidOut = false;
    }

    // Update is called once per frame
    void Update()
    {
      // Debug.Log(_timer);

      if (!_kidOut)
      {
          _timer += Time.deltaTime;
      }


      if ((_timer > _ballTime) & !_ballOut)
      {
          _ballOut = true;
          Instantiate(ball, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
      }
      if ((_timer > _kidTime) & !_kidOut)
      {
          Instantiate(kid, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
          _kidOut = true;
      }
    }

    public void KidReturned()
    {
        _timer = 0;
        _ballOut = false;
        _kidOut = false;
    }
}
