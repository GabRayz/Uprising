using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Uprising.Players
{
    public class SpectatorController : MonoBehaviour
    {
        public GameObject canvas;
        public Text playersRemainingText;
        GameManager gameManager;
        public PlayerStats playerStats;
        public new GameObject camera;

        // Start is called before the first frame update
        void Start()
        {
            gameManager = GameObject.Find("Game(Clone)").GetComponent<GameManager>();
        }

        // Update is called once per frame
        void Update()
        {
            playersRemainingText.text = "Players remaining : " + gameManager.playersCount + "/" + gameManager.playersReady.Count;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameObject.Find("_network").GetComponent<NetworkManager>().QuitGame(PhotonNetwork.CurrentRoom.PlayerCount == 1);
            }

            Move();
        }

        public void SetPlayerStats(PlayerStats playerStats)
        {
            this.playerStats = playerStats;
        }

        void Move()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            this.transform.Translate(Vector3.forward * moveVertical * Time.deltaTime * 10);
            this.transform.Translate(Vector3.right * moveHorizontal * Time.deltaTime * 10);

            transform.Rotate(transform.up * Input.GetAxis("Mouse X") * 3);

            float rotationY = camera.transform.rotation.x + Input.GetAxis("Mouse Y") * 2;
            if (rotationY > 180)
                rotationY -= 360;
            rotationY = Mathf.Clamp(rotationY, -90, 90);

            // camera.transform.rotation = Quaternion.Euler(rotationY, camera.transform.rotation.y, camera.transform.rotation.z);
            camera.transform.Rotate(new Vector3(-rotationY, 0, 0));



        }
    }

}
