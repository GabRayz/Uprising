using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MovingHorizontalPlatform : MonoBehaviour
{
    public float speed = 2f;
    private bool b = true;
    private bool a = true;

    private float x;
    private float y;
    public int X = 10;
    public int Y = 0;



    // Start is called before the first frame update
    void Start()
    {
        x = 0;
        y = 0;  
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
        if (X > 0)
        {
            if (b)
            {
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                x += Math.Abs(speed) * Time.deltaTime;
                if (x >= X)
                    b = false;
            }
            else
            {
                transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
                x -= Math.Abs(speed) * Time.deltaTime;
                if (x <= 0)
                    b = true;
            }
        }

        if (Y > 0)
        {
            if (a)
            {
                transform.position += new Vector3(0, 0, speed * Time.deltaTime);
                y += Math.Abs(speed) * Time.deltaTime;
                if (y >= Y)
                    a = false;
            }
            else
            {
                transform.position += new Vector3(0, 0, -speed * Time.deltaTime);
                y -= Math.Abs(speed) * Time.deltaTime;
                if (y <= 0)
                    a = true;
            }
        }
    }
}