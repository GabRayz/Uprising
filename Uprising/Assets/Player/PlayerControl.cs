using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Animator animator;
    private bool isGrounded = true;
    public int jumpsLeft = 1;

    void Start()
    {
        // The animator will just contain the forward movement for the 1st presentation
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        //bool forward = Input.GetKey(KeyCode.Z);
        //bool backward = Input.GetKey(KeyCode.S);
        //bool right = Input.GetKey(KeyCode.D);
        //bool left = Input.GetKey(KeyCode.Q);
        bool jump = Input.GetKeyDown(KeyCode.Space);

        CheckGroundStatus();

        if (isGrounded)
        {
            HandleGroundedMovement(moveVertical, moveHorizontal);
            //animator.SetBool("Jumping", false);
            //animator.applyRootMotion = true;

            // Recharge the jump if needed
            jumpsLeft = (jumpsLeft > 1) ? jumpsLeft : 1;
        }
        else
        {
            HandleMidAirMovement(moveVertical, moveHorizontal);
            //animator.SetBool("Jumping", true);
            //animator.applyRootMotion = false;
        }

        // Player rotation
        transform.Rotate(transform.up * Input.GetAxis("Mouse X") * 3);

        if (jump && jumpsLeft > 0)
        {
            animator.SetFloat("Forward", 0);
            this.GetComponent<Rigidbody>().AddForce(transform.up * 10, ForceMode.VelocityChange);
            jumpsLeft--;
        }

    }

    void HandleGroundedMovement(float moveVertical, float moveHorizontal)
    {
        // Animator
        if (moveVertical > 0) // Handle forward movement
        {
            animator.SetFloat("Forward", 1);
            if (moveHorizontal > 0) this.transform.Translate(Vector3.right * 2 * Time.deltaTime);
            else if (moveHorizontal < 0) this.transform.Translate(Vector3.right * -2 * Time.deltaTime);
        }

        else if (moveVertical < 0)
        {
            animator.SetFloat("Forward", 0); // Set to -1 when the backward movement is added to the animator
            if (moveHorizontal > 0) this.transform.Translate(Vector3.right * 2 * Time.deltaTime);
            else if (moveHorizontal < 0) this.transform.Translate(Vector3.right * -2 * Time.deltaTime);

            this.transform.Translate(Vector3.forward * -5 * Time.deltaTime); // Temporary
        }
        else
        {
            animator.SetFloat("Forward", 0);
            if (moveHorizontal > 0) // To be replaced by Animator
                transform.Translate(Vector3.right * 5 * Time.deltaTime);
            if (moveHorizontal < 0)
                transform.Translate(Vector3.right * -5 * Time.deltaTime, Space.Self);
        }

        //if (right && !(forward || backward))
        //    animator.SetFloat("Strafe", 1);
        //else if (left && !(forward || backward))
        //    animator.SetFloat("Strafe", -1);
        //else animator.SetFloat("Strafe", 0);

    }

    void HandleMidAirMovement(float moveVertical, float moveHorizontal)
    {
        if (moveVertical > 0) transform.Translate(Vector3.forward * 5 * Time.deltaTime);
        if (moveVertical < 0) transform.Translate(transform.forward * -5 * Time.deltaTime);
        if (moveHorizontal > 0) transform.Translate(Vector3.right * 5 * Time.deltaTime);
        if (moveHorizontal < 0) transform.Translate(Vector3.right * -5 * Time.deltaTime);
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
