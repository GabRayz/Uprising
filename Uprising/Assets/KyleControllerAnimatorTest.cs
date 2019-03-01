using UnityEngine;

public class KyleControllerAnimatorTest : MonoBehaviour
{
    public Animator animator;
    private bool isGrounded = true;
    public int jumpsLeft = 1;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        bool forward = Input.GetKey(KeyCode.Z);
        bool backward = Input.GetKey(KeyCode.S);
        bool right = Input.GetKey(KeyCode.D);
        bool left = Input.GetKey(KeyCode.Q);
        bool jump = Input.GetKeyDown(KeyCode.Space);

        CheckGroundStatus();

        if (isGrounded)
        {
            HandleGroundedMovement(forward, backward, right, left);
            animator.SetBool("Jumping", false);
            animator.applyRootMotion = true;
        }
        else
        {
            HandleMidAirMovement(forward, backward, right, left);
            animator.SetBool("Jumping", true);
            animator.applyRootMotion = false;
        }

        if (isGrounded)
        {
            jumpsLeft = jumpsLeft > 1 ? jumpsLeft : 1;
        }
        if (jump && jumpsLeft > 0)
        {
            animator.applyRootMotion = false;
            this.GetComponent<Rigidbody>().AddForce(this.transform.up * 5, ForceMode.VelocityChange);
            jumpsLeft--;
        }

    }

    void HandleGroundedMovement(bool forward, bool backward, bool right, bool left)
    {
        // Animator
        if (forward) // Handle forward movement
        {
            animator.SetFloat("Forward", 1);
            if (right) this.transform.Translate(this.transform.right * 2 * Time.deltaTime);
            else if (left) this.transform.Translate(this.transform.right * -2 * Time.deltaTime);
        }

        else if (backward)
        {
            animator.SetFloat("Forward", -1);
            if (right) this.transform.Translate(this.transform.right * 2 * Time.deltaTime);
            else if (left) this.transform.Translate(this.transform.right * -2 * Time.deltaTime);
        }
        else animator.SetFloat("Forward", 0);

        if (right && !(forward || backward))
            animator.SetFloat("Strafe", 1);
        else if (left && !(forward || backward))
            animator.SetFloat("Strafe", -1);
        else animator.SetFloat("Strafe", 0);

    }

    void HandleMidAirMovement(bool forward, bool backward, bool right, bool left)
    {
        if(forward) transform.Translate(transform.forward * Time.deltaTime);
        if (backward) transform.Translate(transform.forward * -1 * Time.deltaTime);
        if (right) transform.Translate(transform.right * Time.deltaTime);
        if (left) transform.Translate(transform.forward * -1 * Time.deltaTime);
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * 0.3f));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, 0.3f))
        {
            // m_GroundNormal = hitInfo.normal;
            isGrounded = true;
            // m_Animator.applyRootMotion = true;
        }
        else
        {
            isGrounded = false;
            // m_GroundNormal = Vector3.up;
            // m_Animator.applyRootMotion = false;
        }
    }
}
