using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Uprising.Items;

namespace Uprising.Players
{
    [RequireComponent(typeof(InventoryManager))]
    public class PlayerControl : MonoBehaviour
    {
        public GameObject menu;
        public Player lastHitter;
        public Animator animator;
        public new GameObject camera;
        public Camera cam;
        public GameObject hand;
        private bool isGrounded = true;
        public int jumpsLeft = 1;
        public int dashLeft;
        public int jump = 700;
        private bool jumping = false;
        public float dash = 700;
        private bool isDashing = false;
        public float dashTime = 0.3f;
        public InventoryManager inventory;
        public bool debugMode = false;
        private Vector3 move;
        private Vector3 dashvector;
        public bool contrallable = true;

        public float backwardSpeed = 3;
        public float runSpeed = 5;
        
        public IKControl IkControl;
        
        public float speedModifier = 5;
        public PhotonView photonView;
        Rigidbody rb;

        private readonly byte PlayerEliminationEvent = 0;
        public GameObject spectatorPrefab;
        public bool aim = false;
        public int counter = 0;
        public GameObject Scope;

        public PlayerStats playerStats;

        GameManager gameManager;

        void Start()
        {
            // Get Component
            animator = GetComponent<Animator>();
            photonView = GetComponent<PhotonView>();

            if (!debugMode)
                gameManager = GameObject.Find("Game(Clone)").GetComponent<GameManager>();

            if (!debugMode && photonView.IsMine)
            {
                playerStats = gameManager.localPlayer;
                playerStats.playerControl = this;
                playerStats.owner = PhotonNetwork.LocalPlayer;
            }
            else if (!debugMode && playerStats == null)
                playerStats = new PlayerStats(this);




            inventory = GetComponent<InventoryManager>();
            rb = GetComponent<Rigidbody>();
            IkControl = GetComponent<IKControl>();


            cam = camera.GetComponent<Camera>();
            if (debugMode || photonView.IsMine)
            {
                menu = Instantiate(menu);
                menu.SetActive(false);
                menu.GetComponent<InGameMenuController>().SetOwner(this);
            }
            if (!debugMode)
            {
                cam.enabled = photonView.IsMine;


                // Player is ready
                gameManager.SetPlayerStat(this.playerStats);
                if (photonView.IsMine)
                {
                    gameManager.gameObject.GetPhotonView().RPC("SetReady", RpcTarget.MasterClient, this.photonView.Owner);
                    this.photonView.RPC("SetPlayerInfo", RpcTarget.OthersBuffered);
                }
            }
            else
            {
                cam.enabled = contrallable;
            }




            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        [PunRPC]
        public void SetPlayerInfo()
        {
            // Fill info on playerStats
        }

        void Update()
        {
            if(debugMode && contrallable || photonView.IsMine && gameManager.isStarted)
            {
                
                if (Input.GetKeyDown(KeyCode.Space) && jump > 0)
                {
                    if (jumpsLeft > 0 && isGrounded)
                    {
                        Debug.Log("Jumping");
                        rb.AddForce(Vector3.up * jump);
                        jumpsLeft--;
                        animator.SetTrigger("jump");
                    }
                    else if (dashLeft > 0 && !isGrounded && !isDashing)
                    {
                        Debug.Log("Dashing");
                        //rb.AddForce(400, 0, 0);
                        dashLeft--;
                        isDashing = true;
                    }
                }

                ReadInventoryInputs();
                if (Input.GetKeyDown(KeyCode.Escape)) ToggleMenu();
                if (inventory.GetSelectedItem() != null)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse1) && inventory.GetSelectedItem() is Weapon)
                    {
                        if(inventory.GetSelectedItem() is Sniper)
                        {
                            Scope.SetActive(!aim);
                        }
                        aim = !aim;
                        counter = 6;
                        toggleaim();
                    }

                    if (!aim && !(inventory.GetSelectedItem() is Sniper))
                        Scope.SetActive(false);
                }
                else
                {
                    Scope.SetActive(false);
                    if (aim)
                        aim = !aim;
                }
            }
            playerStats.time += Time.deltaTime;
        }

        void FixedUpdate()
        {
            if (debugMode && contrallable || (photonView.IsMine && gameManager.isStarted))
            {
                if (!menu.activeSelf)
                {
                    CheckGroundStatus();
                    float moveHorizontal = Input.GetAxis("Horizontal");
                    float moveVertical = Input.GetAxis("Vertical");
                    
                    if (moveVertical < 0)
                    {
                        if (speedModifier > 0)
                            transform.Translate(Vector3.forward * moveVertical * speedModifier * Time.deltaTime);
                    }
                    
                    else
                    {
                        if(moveVertical > 0 || moveHorizontal != 0)
                        {
                            transform.Translate(Vector3.forward * moveVertical * (speedModifier > 0 ? speedModifier : 0) * Time.deltaTime);
                            transform.Translate(Vector3.right * moveHorizontal * (speedModifier > 0 ? speedModifier : 0) * Time.deltaTime);
                        
                        }
                    }
                    
                    animator.SetBool("Run", (moveVertical > 0 || moveHorizontal != 0 && moveVertical == 0) && isGrounded);
                    animator.SetBool("WalkBackward", moveVertical < 0 && isGrounded);


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

                    if (isGrounded)
                    {
                        jumpsLeft = 1;
                        animator.SetBool("Jumping", false);
                    }
                    else
                        animator.SetBool("Jumping", true);
                    
                    if (isDashing)
                    { 
                        if (dashTime < 0)
                        {
                            dashTime = 0.3f;
                            isDashing = false;
                            dash = 800f;
                        }
                        isDashing = false;
                        rb.AddForce(transform.forward*dash);
                        dashTime -= Time.deltaTime;
                    }
                }
            }
        }
        public void toggleaim()
        {
            if (aim)
            {
                StartCoroutine("IncreaseFOV");
                
            }
            else
            {
                StartCoroutine("DecreaseFOV");
            }                
                /*
                if (counter > 0)
                {
                    camera.transform.position = Vector3.Lerp(camera.transform.position, camera.transform.position + this.transform.forward, Time.deltaTime * 10f);
                    counter--;
                }
            }
            else
            {
                if (counter > 0)
                {
                    camera.transform.position = Vector3.Lerp(camera.transform.position, camera.transform.position - this.transform.forward, Time.deltaTime * 10f);
                    counter--;
                }*/
            
        }

        private IEnumerator IncreaseFOV() { float fov = 60f; while (fov > 30) { fov-=3; cam.fieldOfView = fov; yield return null; } }
        private IEnumerator DecreaseFOV() { float fov = 30f; while (fov < 60) { fov+=3; cam.fieldOfView = fov; yield return null; } }

        [PunRPC]
        public void Hit(Player hitter, Vector3 direction, float power)
        {
            Debug.Log("Player hit!");
            rb.AddForce (direction * power / 2, ForceMode.Impulse);
            if (!debugMode)
                lastHitter = hitter;
        }

        [PunRPC]
        public void OnTargetHit()
        {
            this.playerStats.hits += 1;
        }

        public void ToggleMenu()
        {
            menu.SetActive(!menu.activeSelf);
            Cursor.lockState = (menu.activeSelf) ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = menu.activeSelf;
        }

        public void Quit()
        {
            if (!debugMode)
            {
                Eliminate("Client left the battle", false);
            }
        }

        void CheckGroundStatus()
        {
            RaycastHit hitInfo;
#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(transform.position + (Vector3.up * 0.2f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * 0.15f), Color.white);
#endif
            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, 0.15f))
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
            if (other.gameObject.CompareTag("lava") && photonView.IsMine)
            {
                Debug.Log("lava");
                Eliminate("Tried to swim into lava");
            }

            if (other.gameObject.CompareTag("belette") && ((debugMode && !contrallable) || (photonView.IsMine && this.photonView.Owner.ActorNumber != other.gameObject.GetComponent<PhotonView>().Owner.ActorNumber)))
            {
                Debug.Log("hit");
                Belette belette = other.GetComponent<Belette>();
                if(!debugMode)
                {
                    PlayerControl enemy = gameManager.players[belette.photonView.Owner].playerControl;
                    enemy.photonView.RPC("OnTargetHit", RpcTarget.All);

                    this.photonView.RPC("Hit", RpcTarget.All, belette.photonView.Owner, belette.transform.forward, belette.power);
                    PhotonNetwork.Destroy(belette.photonView);
                }
                else
                {
                    Hit(null, belette.transform.forward, belette.power);
                }
            }
        }

        public void ModifySpeed(float modifier)
        {
            Debug.Log("Speed set to " + (speedModifier + modifier));
            speedModifier += modifier;
        }

        public void ModifyJumpHeight(int modifier)
        {
            jump += modifier;
        }

        public void Eliminate(string deathMessage, bool stayAsASpectator = true)
        {
            Debug.Log(deathMessage);
            if (lastHitter != null)
            {
                playerStats.killer = lastHitter;
                gameManager.players[lastHitter].playerControl.photonView.RPC("OnTargetKilled", RpcTarget.All);
            }

            if(stayAsASpectator)
            {
                GameObject spec = Instantiate(spectatorPrefab, new Vector3(0, 15, -40), Quaternion.identity);
                spec.SendMessage("SetPlayerStats", playerStats);
            }
            else
            {
                GameObject.Find("_network").GetComponent<NetworkManager>().QuitGame(false, false);
            }
            gameManager.photonView.RPC("EliminatePlayer", RpcTarget.All, GetComponent<PhotonView>().Owner);

            Destroy(this.inventory.hud);
            if (debugMode)
                Destroy(this.gameObject);
            else
                PhotonNetwork.Destroy(this.photonView);
        }

        [PunRPC]
        public void OnTargetKilled()
        {
            this.playerStats.kills += 1;
            if (photonView.IsMine)
            {
                Debug.Log("Target killed !");
            }
        }

        [PunRPC]
        public void ToggleInvisibility()
        {
            this.gameObject.SetActive(!this.gameObject.activeSelf);
        }

        void ReadInventoryInputs()
        {
            // Selection
            if (Input.GetButtonDown("Select 1")) inventory.SelectItem(0);
            if (Input.GetButtonDown("Select 2")) inventory.SelectItem(1);
            if (Input.GetButtonDown("Select 3")) inventory.SelectItem(2);
            if (Input.GetButtonDown("Select 4")) inventory.SelectItem(3);

            if (Input.GetAxis("Mouse ScrollWheel") > 0) inventory.SelectItem((inventory.GetSelectedItemIndex() + 1) % 4);
            if (Input.GetAxis("Mouse ScrollWheel") < 0) inventory.SelectItem((inventory.GetSelectedItemIndex() - 1));

            IkControl.ikActive = inventory.GetSelectedItem() is Weapon;

            if (Input.GetKeyDown(KeyCode.L))
            {
                gameManager.lava.transform.Translate(gameManager.lava.transform.up * -5);
                //gameManager.lavaLevel = 0;
            }




            // Use an item
            if (Input.GetButton("Use Item")) inventory.UseSelectedItem();
        }
    }
}