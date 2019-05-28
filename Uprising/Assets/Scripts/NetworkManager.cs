using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Uprising.Players;
using Photon.Pun;
using Photon.Realtime;

using System.Runtime.InteropServices;
using UnityEngine.Networking;

public class PlayerInfo
{
    public string username;
    public int xp;

    public static PlayerInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<PlayerInfo>(jsonString);
    }
}

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public bool inMatchMaking = false;
    public bool inMainMenu = false;
    public Text matchMakingText;
    public int MaxPlayer = 2;
    public bool isInGame = false;
    public Text StartingText;
    public MainMenu mainMenu;
    public PlayerStats localPlayerGameStats;
    Dictionary<Player, PlayerStats> players;
    Stack<Player> scoreboard;
    public bool debug = false;

    public void Awake()
    {
        App.networkManager = this;
        App.debug = debug;
    }

    // Start is called before the first frame update
    public void Start()
    {
        StartingText.text = "Connection...";
        
        StartCoroutine(Authenticate());
        // PhotonNetwork.ConnectToRegion("eu");
        // PhotonNetwork.AutomaticallySyncScene = true;
    }

    IEnumerator Authenticate()
    {
        InitLocalPlayer();
        
#if UNITY_WEBGL
        if (!debug)
        {
            var www = UnityWebRequest.Get("/auth/data");

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Server response");
                Debug.Log(www.downloadHandler.text);
                var info = PlayerInfo.CreateFromJSON(www.downloadHandler.text);

                PhotonNetwork.LocalPlayer.NickName = info.username;
                localPlayerGameStats.xp = info.xp;
                localPlayerGameStats.pseudo = info.username;
            }
        }
#else
        yield return null;
#endif

        PhotonNetwork.ConnectUsingSettings();
    }

    void InitLocalPlayer()
    {
        this.localPlayerGameStats = new PlayerStats(null);
        App.localPlayer = localPlayerGameStats;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected!");
        PhotonNetwork.JoinLobby();
        StartingText.text = "";
        if(!inMainMenu)
            JoinMainMenu();
    }

    public void JoinMainMenu()
    {
        inMainMenu = true;
        Debug.Log("Loading Main Menu");
        // Load the main menu scene. SetMainMenu() will then be called to get the Main Menu
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    public void SetMainMenu(MainMenu mainMenu)
    {
        Debug.Log("Main menu loaded!");
        this.mainMenu = mainMenu;
        mainMenu.matchMakingText.text = PhotonNetwork.LocalPlayer.NickName;
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        mainMenu.SetPlayerInfo();
    }

    public void PlayRandom()
    {
        if(PhotonNetwork.InLobby)
        {
            // Try to join a random room..
            PhotonNetwork.JoinRandomRoom();
            mainMenu.matchMakingText.text = "Joinning...";
        }
        else
        {
            mainMenu.matchMakingText.text = "Connecting to server...";
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No available room found. Creating a new one...");
        RoomOptions roomOptions = new RoomOptions();

        Hashtable property = new Hashtable();
        property.Add("size", 3);
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        roomOptions.CustomRoomProperties.Add("s", 3);

        roomOptions.MaxPlayers = (byte)MaxPlayer;
        roomOptions.PublishUserId = true;



        bool success = PhotonNetwork.CreateRoom(null, roomOptions);
        if(!success)
        {
            PhotonNetwork.Disconnect();
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(1));
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (cause == DisconnectCause.DisconnectByClientLogic)
        {
            StartingText.text = "Connection...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinedRoom()
    {
        Dictionary<object, object> prop = PhotonNetwork.CurrentRoom.CustomProperties;
        Debug.Log(prop.Count);
        foreach (var p in prop)
        {
            Debug.Log(p + " : " + prop);
        }
        inMatchMaking = true;
        Debug.Log("Room joined");
    }

    public void CancelPlay()
    {
        inMatchMaking = false;
        mainMenu.matchMakingText.text = "";
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinLobby();
        Debug.Log("Room left");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Lobby joined");
        mainMenu.matchMakingText.text = "";
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

        Random.InitState((int)Time.realtimeSinceStartup);

        localPlayerGameStats.winner = false;

        // Lock the room
         if (PhotonNetwork.LocalPlayer.IsMasterClient)
            PhotonNetwork.CurrentRoom.IsOpen = false;


        // Unload main menu, display loading time
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Main Menu"));
        StartingText.text = "Loading world...";

        // Add map 1 to loaded scene
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        Debug.Log("Load map 1 scene");
        inMatchMaking = false;
        isInGame = true;
        inMainMenu = false;
    }

    public void OnGameLoaded()
    {
        StartingText.text = "";
    }

    public void LeaveToScoreBoard(Stack<Player> scoreboard, Dictionary<Player, PlayerStats> players)
    {
        if (!isInGame) return;
        Debug.Log("Leaving to scoreBoard...");
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(2));
        this.scoreboard = scoreboard;
        // this.localPlayerGameStats = stats;
        this.players = players;
        StartCoroutine("LoadScoreBoardScene");
    }

    IEnumerator LoadScoreBoardScene()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync("Scoreboard", LoadSceneMode.Additive);
        while(!loadScene.isDone)
        {
            // Wait until loading is completed
            yield return null;
        }

        ScoreBoardManager scoreBoardManager = GameObject.Find("ScoreBoard").GetComponent<ScoreBoardManager>();
        scoreBoardManager.SetStats(this.scoreboard, this.players);
    }

    public void QuitGame(bool isLastInRoom = false, bool isInScoreBoardScene = true)
    {
        Debug.Log("Quit game");
        // Update profile
        localPlayerGameStats.OnGameEnd();

        // Send player's stats to server
        SendStats();

        // Leave Room
        if (isLastInRoom)
            PhotonNetwork.CurrentRoom.IsOpen = true;
        if(isLastInRoom && PhotonNetwork.LocalPlayer.IsMasterClient)
            PhotonNetwork.DestroyAll();
        isInGame = false;

        Scene scoreScene = SceneManager.GetSceneByName("Scoreboard");
        if (scoreScene.IsValid())
            SceneManager.UnloadSceneAsync(scoreScene);

        // Unload scenes
        if (!isInScoreBoardScene)
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(2));

        PhotonNetwork.LeaveRoom(); // Leaving the room will automatically re-join the server, and then call OnConnected()
        Debug.Log("room left");
    }

    void SendStats()
    {
        Debug.Log("Shots : " + localPlayerGameStats.hits);
#if UNITY_WEBGL
        if (!debug)
        {
            /*var stats = new PlayerStatsJson();

            stats.xp = localPlayerGameStats.xp;
            stats.winner = localPlayerGameStats.pseudo == "Vardiak";
            stats.shotCount = localPlayerGameStats.belettesShot;
            stats.accurateShotCount = localPlayerGameStats.hits;

            var json = JsonUtility.ToJson(stats);*/
            
            
            var form = new WWWForm();
            form.AddField("xp", localPlayerGameStats.xp);
            form.AddField("winner", localPlayerGameStats.winner ? 1 : 0);
            form.AddField("shotCount", localPlayerGameStats.belettesShot);
            form.AddField("accurateShotCount", localPlayerGameStats.hits);
            
            
            var www = UnityWebRequest.Post("/game", form);

            www.SendWebRequest();
        }
#endif
    }
}
