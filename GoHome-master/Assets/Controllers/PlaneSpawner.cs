using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSpawner : MonoBehaviour
{
    public GameObject plain;
    public float timeBetweenPlanes;
    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        _timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer < 0)
        {
            CreatePlane();
            _timer = timeBetweenPlanes;
        }
    }

    public void CreatePlane()
    {
        Instantiate(plain, gameObject.transform.position, Quaternion.identity);
    }
}
