using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingPlatform : MonoBehaviour
{
    public float speed = 15f;

    void Start()
    {
    }

    void OnCollisionEnter(Collision other)
    {
        other.rigidbody.AddForce(Vector3.up * speed, ForceMode.Impulse);

    }

    private void Update()
    {
    }
}
