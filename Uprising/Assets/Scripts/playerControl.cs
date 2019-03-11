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
        
        CheckGroundStatus();
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
    
    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * 0.1f));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, 0.1f))
        {
            // m_GroundNormal = hitInfo.normal;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            // m_GroundNormal = Vector3.up;
        }
    }
}
