using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1LeftSpawnerController : MonoBehaviour
{
    private float _PaperBoyFrequency = 10;
    private float _CarFrequency = 5;
    private float _timer = 0;
    public GameObject PaperBoy;
    public GameObject Car;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      _timer += Time.deltaTime;
      if (_timer > _PaperBoyFrequency)
      {
        Instantiate(PaperBoy,new Vector2 (transform.position.x, transform.position.y), Quaternion.identity);
        _PaperBoyFrequency = _PaperBoyFrequency + _PaperBoyFrequency;
      }
      if (_timer > _CarFrequency)
      {
        Instantiate(Car,new Vector2 (transform.position.x, transform.position.y), Quaternion.identity);
        _CarFrequency = _CarFrequency + _CarFrequency;
      }
    }
}
