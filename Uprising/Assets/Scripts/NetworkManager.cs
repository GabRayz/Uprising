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
    private bool isInGame = false;
    public Text StartingText;
    public MainMenu mainMenu;


    // Start is called before the first frame update
    void Start()
    {
        StartingText.text = "Connection...";
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnected()
    {
        Debug.Log("Connected!");
        StartingText.text = "";
        JoinMainMenu();
    }

    public void JoinMainMenu()
    {
        Debug.Log("Loading Main Menu");
        // Load the main menu scene. SetMainMenu() will then be called to get the Main Menu
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    public void SetMainMenu(MainMenu mainMenu)
    {
        Debug.Log("Main menu loaded!");
        this.mainMenu = mainMenu;
    }

    public void PlayRandom()
    {
        if(PhotonNetwork.IsConnected)
        {
            // Try to join a random room..
            PhotonNetwork.JoinRandomRoom();
            mainMenu.matchMakingText.text = "Joinning...";
        }
        else
        {
            mainMenu.matchMakingText.text = "Connecting to server...";
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
            mainMenu.matchMakingText.text = "Waiting for players... " + currentRoom.PlayerCount + "/"+ MaxPlayer;

            if(currentRoom.PlayerCount == MaxPlayer && !isInGame)
            {
                // All players ready, start the game
                StartGame();
            }
        }
    }

    private void StartGame()
    {
        // Unload main menu, display loading time
        SceneManager.UnloadSceneAsync("Main Menu");
        StartingText.text = "Loading world...";

        // Add map 1 to loaded scene
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        Debug.Log("Load map 1 scene");
        inMatchMaking = false;
        isInGame = true;
    }

    public void OnGameStarted()
    {
        //
    }

    public override void OnLeftRoom()
    {
        isInGame = false;
        Debug.Log("Room left");
    }
}
