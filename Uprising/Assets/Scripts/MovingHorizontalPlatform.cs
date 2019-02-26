using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingHorizontalPlatform : MonoBehaviour
{
    public float speed = 2f;
    private bool b = true;
    public Vector3 pos;
    public Vector3 mouvement;
    private float x;
    public int distance = 10;



    // Start is called before the first frame update
    void Start()
    {
        x = transform.position.x;
    }

    private void OnCollisionEnter(Collision other)
    {
        other.collider.transform.SetParent(transform);
    }

    private void OnCollisionExit(Collision other)
    {
        other.collider.transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (b)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            if (transform.position.x >= distance)
                b = false;
        }
        else
        {
            transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
            if (transform.position.x <= x)
                b = true;
        }
    }
}