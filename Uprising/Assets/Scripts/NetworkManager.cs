using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public bool inMatchMaking = false;
    public Text matchMakingText;
    public int MaxPlayer = 2;
    public GameObject playButton;
    public GameObject cancelButton;
    private bool isInGame = false;


    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
        matchMakingText.text = "Connecting to server...";
    }

    public override void OnConnected()
    {
        matchMakingText.text = "";
        playButton.SetActive(true);
    }

    public void PlayRandom()
    {
        if(PhotonNetwork.IsConnected)
        {
            // Try to join a random room..
            PhotonNetwork.JoinRandomRoom();
            matchMakingText.text = "Joinning...";
        }
        else
        {
            matchMakingText.text = "Connecting to server...";
            PlayRandom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No available room found. Creating a new one...");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)MaxPlayer;
        roomOptions.PublishUserId = true;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        inMatchMaking = true;
        Debug.Log("Room joined");
    }

    public void CancelPlay()
    {
        inMatchMaking = false;
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinLobby();
        Debug.Log("Room left");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Lobby joined");
        isInGame = false;
    }

    private void Update()
    {
        // Check if all players are ready to start the game
        if (inMatchMaking)
        {
            Photon.Realtime.Room currentRoom = PhotonNetwork.CurrentRoom;
            if (currentRoom == null) return;
            matchMakingText.text = "Waiting for players... " + currentRoom.PlayerCount + "/"+ MaxPlayer;

            if(currentRoom.PlayerCount == MaxPlayer && PhotonNetwork.IsMasterClient && !isInGame)
            {
                // All players ready, start the game
                SceneManager.LoadScene(1, LoadSceneMode.Additive);
                Debug.Log("Load map 1 scene");
                isInGame = true;
                playButton.SetActive(true);
                cancelButton.SetActive(false);
            }
        }
    }

    public override void OnLeftRoom()
    {
        isInGame = false;
        Debug.Log("Room left");
    }
}
