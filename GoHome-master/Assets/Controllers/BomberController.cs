using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberController : MonoBehaviour
{
    public GameObject bomb;
    public float planeSpeed;
    public float timeBetweenBombs;
    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        _timer = timeBetweenBombs;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(planeSpeed, 0f, 0f);

        _timer -= Time.deltaTime;

        if (_timer < 0)
        {
            DropBomb();
            _timer = timeBetweenBombs;
        }

    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "GameWorld")
        {
            Destroy(gameObject);
        }
    }

    public void DropBomb()
    {
        Instantiate(bomb, gameObject.transform.position, Quaternion.identity);
    }
}
