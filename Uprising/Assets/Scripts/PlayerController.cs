using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // This script is to be attached to the player.
    public Rigidbody rb;
    private GameObject model;
    public float speed = 2;
    public float speedModifier = 1;

    public float GetSpeed()
    {
        return speed * speedModifier;
    }
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        model = rb.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Player's movement, to be improved
        if(Input.GetKey(KeyCode.Z))
        {
            this.transform.position += this.transform.forward * GetSpeed() * Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += -1 * transform.forward * (GetSpeed() / 2) * Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            this.transform.position += -1 * transform.right * (GetSpeed() / 2) * Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += transform.right * (GetSpeed() / 2) * Time.fixedDeltaTime;
        }

        if(Input.GetKey(KeyCode.A))
        {
            Quaternion deltaRot = Quaternion.Euler(new Vector3(0, 50, 0) * Time.deltaTime * GetSpeed());
            rb.MoveRotation(deltaRot * rb.rotation);
        }

        if (Input.GetKey(KeyCode.E))
        {
            Quaternion deltaRot = Quaternion.Euler(new Vector3(0, -50, 0) * Time.deltaTime * GetSpeed());
            rb.MoveRotation(deltaRot * rb.rotation);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("item"))
        {
            other.gameObject.SendMessage("Collect", rb.gameObject);
        }
    }

    public void GiveItem()
    {

    }
}