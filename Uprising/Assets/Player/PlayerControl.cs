using Photon.Pun;
using UnityEngine;

namespace Uprising.Players
{
    [RequireComponent(typeof(InventoryManager))]
    public class PlayerControl : MonoBehaviour {
        public GameObject menu;
        public GameObject hud;
        public Animator animator;
        public new GameObject camera;
        public Camera cam;
        public GameObject hand;
        private bool isGrounded = true;
        public int jumpsLeft = 1;
        InventoryManager inventory;
        public bool debugMode = false;

        public float speedModifier = 5;
        public PhotonView photonView;

        void Start()
        {
            // The animator will just contain the forward movement for the 1st presentation
            animator = GetComponent<Animator>();
            inventory = GetComponent<InventoryManager>();
            photonView = GetComponent<PhotonView>();
            cam = camera.GetComponent<Camera>();
            if(!debugMode)
            {
                cam.enabled = false;
                if (photonView.IsMine) cam.enabled = true;
            }

            menu = Instantiate(menu);
            menu.SetActive(false);
            menu.GetComponent<InGameMenuController>().SetOwner(this);

            hud = Instantiate(hud);
        }


        void Update()
        {
            if (debugMode || photonView.IsMine)
            {
                float moveHorizontal = Input.GetAxis("Horizontal");
                float moveVertical = Input.GetAxis("Vertical");

                //bool forward = Input.GetKey(KeyCode.Z);
                //bool backward = Input.GetKey(KeyCode.S);
                //bool right = Input.GetKey(KeyCode.D);
                //bool left = Input.GetKey(KeyCode.Q);
                bool jump = Input.GetKeyDown(KeyCode.Space);

                CheckGroundStatus();
                animator.SetBool("Grounded", isGrounded);
                if (!isGrounded) animator.applyRootMotion = false;
                else animator.applyRootMotion = true;

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
                // Apply current speed
                animator.SetFloat("SpeedModifier", speedModifier / 5);

                // Player rotation
                transform.Rotate(transform.up * Input.GetAxis("Mouse X") * 3);
                // Camera rotation
                float rotationX = camera.transform.parent.transform.eulerAngles.x - Input.GetAxis("Mouse Y") * 2;

                //Limit head rotation (up and bottom)
                if (rotationX > 180)
                    rotationX -= 360;
                rotationX = Mathf.Clamp(rotationX, -90, 90);
                // Apply rotation
                camera.transform.parent.transform.rotation = Quaternion.Euler(rotationX, camera.transform.parent.transform.eulerAngles.y, 0);

                // Jump
                if (jump && jumpsLeft > 0)
                {
                    animator.SetFloat("Forward", 0);
                    this.GetComponent<Rigidbody>().AddForce(transform.up * 10, ForceMode.VelocityChange);
                    jumpsLeft--;
                }

                ReadInventoryInputs();
            }
        }

        void ReadInventoryInputs()
        {
            // Selection
            if (Input.GetButtonDown("Select 1")) inventory.SelectItem(0);
            if (Input.GetButtonDown("Select 2")) inventory.SelectItem(1);
            if (Input.GetButtonDown("Select 3")) inventory.SelectItem(2);
            if (Input.GetButtonDown("Select 4")) inventory.SelectItem(3);

            if (Input.GetAxis("Mouse ScrollWheel") > 0) inventory.SelectItem((inventory.GetSelectedItem() + 1) % 4);
            if (Input.GetAxis("Mouse ScrollWheel") < 0) inventory.SelectItem((inventory.GetSelectedItem() - 1));

            // Use an item
            if (Input.GetButtonDown("Use Item")) inventory.UseSelectedItem();
        }

        void HandleGroundedMovement(float moveVertical, float moveHorizontal)
        {
            // Animator
            if (moveVertical > 0) // Handle forward movement
            {
                animator.SetFloat("Forward", 1);
                if (moveHorizontal > 0) this.transform.Translate(Vector3.right * speedModifier /2 * Time.deltaTime);
                else if (moveHorizontal < 0) this.transform.Translate(Vector3.right * -speedModifier /2 * Time.deltaTime);
            }

            else if (moveVertical < 0)
            {
                animator.SetFloat("Forward", 0); // Set to -1 when the backward movement is added to the animator
                if (moveHorizontal > 0) this.transform.Translate(Vector3.right * speedModifier /2 * Time.deltaTime);
                else if (moveHorizontal < 0) this.transform.Translate(Vector3.right * -speedModifier /2 * Time.deltaTime);

                this.transform.Translate(Vector3.forward * -5 * Time.deltaTime); // Temporary
            }
            else
            {
                animator.SetFloat("Forward", 0);
                if (moveHorizontal > 0) // To be replaced by Animator
                    transform.Translate(Vector3.right * speedModifier * Time.deltaTime);
                if (moveHorizontal < 0)
                    transform.Translate(Vector3.right * -speedModifier * Time.deltaTime, Space.Self);
            }

            //if (right && !(forward || backward))
            //    animator.SetFloat("Strafe", 1);
            //else if (left && !(forward || backward))
            //    animator.SetFloat("Strafe", -1);
            //else animator.SetFloat("Strafe", 0);

        }

        void HandleMidAirMovement(float moveVertical, float moveHorizontal)
        {
            if (moveVertical > 0) transform.Translate(Vector3.forward * speedModifier * Time.deltaTime);
            if (moveVertical < 0) transform.Translate(Vector3.forward * -speedModifier * Time.deltaTime);
            if (moveHorizontal > 0) transform.Translate(Vector3.right * speedModifier * Time.deltaTime);
            if (moveHorizontal < 0) transform.Translate(Vector3.right * -speedModifier * Time.deltaTime);
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("item"))
            {
                other.gameObject.SendMessage("Collect", this.gameObject);
            }
        }

        public void ModifySpeed(float modifier)
        {
            speedModifier += modifier;
        }
    }
}