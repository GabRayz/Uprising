using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public bool inMatchMaking = false;
    public Text matchMakingText;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void PlayRandom()
    {
        // Try to join a random room..
        PhotonNetwork.JoinRandomRoom();
        matchMakingText.text = "Joinning...";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No available room found. Creating a new one...");
        PhotonNetwork.CreateRoom(null);
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
    }

    private void Update()
    {
        if (inMatchMaking)
        {
            Photon.Realtime.Room currentRoom = PhotonNetwork.CurrentRoom;
            if (currentRoom == null) return;
            matchMakingText.text = "Waiting for players... " + currentRoom.PlayerCount + "/4";
        }
    }
}
