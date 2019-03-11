using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    public int speed = 5;
    public Rigidbody rb;
    public int jump = 5;
    public bool isGrounded;
    public int jumpsLeft = 2;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        transform.Translate(moveHorizontal * Time.deltaTime * speed, 0, moveVertical*Time.deltaTime*speed);

        if(isGrounded)
            jumpsLeft = 2;

        if (Input.GetKeyDown(KeyCode.Space) && jumpsLeft>0)
        {
            rb.AddForce(transform.up *jump, ForceMode.VelocityChange);
            jumpsLeft--;
        }
    }    
}
