using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyingPlatform : MonoBehaviour
{
    public float speed = 5f;

    void Start()
    {
    }
    void OnTriggerEnter (collided other)
    {
        if (other.transform.tag == "Player")
        {
            Destroy(this, 2);
        }
    }
}
