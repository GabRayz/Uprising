using System;
using UnityEngine;
using Photon.Pun;


namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    [RequireComponent(typeof(InventoryManager))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        public GameObject head;
        public float mouseSensitivity = 2;

        public Rigidbody rb;
        private GameObject model;
        public float speedModifier = 1;
        public PhotonView photonView;
        private InventoryManager inventory;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            photonView = GetComponent<PhotonView>();
            model = rb.gameObject;
            speedModifier = 1;

            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
            inventory = GetComponent<InventoryManager>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = Input.GetButtonDown("Jump");
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            GetInventoryInput();
            GetMovement();
        }

        private void GetInventoryInput()
        {
            if (Input.GetButtonDown("Select 1")) inventory.SelectItem(0);
            if (Input.GetButtonDown("Select 2")) inventory.SelectItem(1);
            if (Input.GetButtonDown("Select 3")) inventory.SelectItem(2);
            if (Input.GetButtonDown("Select 4")) inventory.SelectItem(3);

            if (Input.GetAxis("Mouse ScrollWheel") > 0) inventory.SelectItem((inventory.GetSelectedItem() + 1) % 4);
            if (Input.GetAxis("Mouse ScrollWheel") < 0) inventory.SelectItem((inventory.GetSelectedItem() - 1));
        }

        private void GetMovement()
        {
            // read inputs
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            //Rotate body
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0));

            // Move head
            float rotationX = head.transform.eulerAngles.x - Input.GetAxis("Mouse Y");

            //if(v < 0)
            //{
            //    this.transform.Translate(new Vector3(0, 0, v));
            //    v = 0;
            //}

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump, speedModifier);
            m_Jump = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("item"))
            {
                other.gameObject.SendMessage("Collect", rb.gameObject);
            }
        }

        public void ModifySpeed(float modifier)
        {
            speedModifier += modifier;
        }
    }
}
