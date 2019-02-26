using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyingPlatform : MonoBehaviour
{
    private Rigidbody rigid;

    void Start()
    {
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "player")
            Destroy(this.transform.gameObject, 2);
    }
}
