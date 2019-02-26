using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingPlatform : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rigid;
    void Start()
    {
    }
    void OnTriggerEnter(Collider other)
    {
         other.GetComponent<Rigidbody>().AddForce(Vector3.up * speed, ForceMode.Acceleration);
    }
}
