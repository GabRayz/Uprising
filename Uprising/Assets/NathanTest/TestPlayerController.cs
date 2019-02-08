using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject head;

    private int speed = 10;
    
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.cyan);
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //Movement
        Vector3 movement = Quaternion.LookRotation(transform.forward, transform.up) * new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.position += movement * Time.fixedDeltaTime * speed;


        //Rotate head
        float rotationX = head.transform.eulerAngles.x - Input.GetAxis("Mouse Y") * speed;

        //Limit head rotation (up and bottom)
        if (rotationX > 180)
            rotationX -= 360;
        rotationX = Mathf.Clamp(rotationX, -90, 90);

        head.transform.rotation = Quaternion.Euler(rotationX, head.transform.eulerAngles.y, 0);


        //Rotate body
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * speed, 0));
    }
}
