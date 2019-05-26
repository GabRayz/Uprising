using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingVerticalPlatform : MonoBehaviour
{
    public float speed = 2f;
    private bool b = true;
    
    private float y;
    public int height = 10;

    // Start is called before the first frame update
    void Start()
    {
        y = transform.position.y;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("player"))
            other.collider.transform.SetParent(transform);
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("player"))
            other.collider.transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (b)
        {
            transform.position += new Vector3(0,speed * Time.deltaTime, 0);
            if (transform.position.y >= height + y)
                b = false;
        }
        else
        {
            transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
            if (transform.position.y <= y)
                b = true;
        }
    }
}
