using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using Uprising.Players;
using UnityEngine.UI;
using Uprising.Items;

public class GameManager : MonoBehaviourPunCallbacks
{
    public NetworkManager network;
    SpawnPlayers[] spawnSpots;
    public GameObject lava;
    public new PhotonView photonView;
    public Text topText;
    float lavaRisingSpeed = 0.1f;
    public bool OfflineMode = false;
    private byte PlayerEliminationEvent = 0;
    public int playersCount;
    // public Dictionary<Player, bool> players;
    Stack<Player> scoreBoard;
    public PlayerStats localPlayer;
    // List<PlayerStats> playerStats = new List<PlayerStats>();
    public Dictionary<Player, PlayerStats> players;
    public Dictionary<Player, bool> playersReady;
    public float lavaLevel;

    public float lavaLevelMax = 100;
    public float lavaLevelMin = 0;

    public bool isStarted;

    // Start is called before the first frame update
    void Start()
    {
        network = GameObject.Find("_network").GetComponent<NetworkManager>();
        localPlayer = network.localPlayerGameStats;
        localPlayer.Reset();

        photonView = GetComponent<PhotonView>();
        Debug.Log("Map 1 scene loaded !");
        players = new Dictionary<Player, PlayerStats>();
        playersReady = new Dictionary<Player, bool>();

        // Get all possible spawn points
        spawnSpots = FindObjectsOfType<SpawnPlayers>();
        if(!OfflineMode) SpawnPlayers();
        lava = GameObject.Find("Lava");
        scoreBoard = new Stack<Player>();
        topText.text = "Waiting for players";
    }

    private void Update()
    {
        if(isStarted) // Raise lava
            lava.transform.Translate(Vector3.up * lavaRisingSpeed * Time.deltaTime);
        lavaLevel = lava.transform.position.y;
        
        for(int i = 9; i>0; i--)
        {
            if (lavaLevel <= lavaLevelMax * (i + 1) / 10 && lavaLevel >= lavaLevelMax * i / 10)
                localPlayer.playerControl.inventory.hud.transform.Find("Canvas").Find("HUD Left").Find("Maplevel").Find(i.ToString()).gameObject.SetActive(true);
        }


        // Finish game
        if (playersCount <= 1 && !OfflineMode && isStarted)
        {
            isStarted = false;
            // if (GameObject.Find("_network").GetComponent<NetworkManager>().isInGame)
                FinishGame();
        }
    }

    private IEnumerator StartCooldown()
    {
        topText.text = "";
        float cooldown = 5;
        while (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            topText.text = "Start in " + Mathf.Floor(cooldown);
            yield return null;
        }
        StartGame();
    }

    void StartGame()
    {
        Debug.Log("Begin!");
        topText.text = "";
        isStarted = true;
        foreach(var player in players)
        {
            player.Value.playerControl.inventory.GiveItem(new DefaultGun(999, 100, 20, 20, player.Value.playerControl.gameObject));
        }
    }

    void FinishGame()
    {
        Debug.Log("Game over");
        foreach (var playerStatus in playersReady)
        {
            if (playerStatus.Value && !playerStatus.Key.IsInactive)
                scoreBoard.Push(playerStatus.Key);
        }

        GameObject.Find("_network").GetComponent<NetworkManager>().LeaveToScoreBoard(scoreBoard, players);
    }

    public void SetPlayerStat(PlayerStats playerStats)
    {
        this.players[playerStats.owner] = playerStats;
    }

    [PunRPC]
    public void SetReady(Player player)
    {
        playersReady[player] = true;
        int readyCount = 0;
        foreach(var playerReady in playersReady)
        {
            if (playerReady.Value) readyCount++;
        }

        Debug.Log("Players ready : " + readyCount + "/" + playersCount);
        if (readyCount == playersCount)
            photonView.RPC("OnAllPlayersReady", RpcTarget.All);
    }

    [PunRPC]
    public void OnAllPlayersReady()
    {
        Debug.Log("All players ready, starting cooldown");
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            playersReady[player.Value] = true;
        }
        StartCoroutine("StartCooldown");
    }

    public void SpawnPlayers()
    {
        // Place the local player at the corresponding spawn point, according to his order in the list of room's players
        int i = 0;
        Player self = PhotonNetwork.LocalPlayer;
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log("Spawn player " + player.Value.UserId);
            playersCount++;
            playersReady.Add(player.Value, false);
            players.Add(player.Value, null);
            if (self.UserId == player.Value.UserId)
            {
                if(player.Key <= spawnSpots.Length)
                    spawnSpots[player.Key - 1].SendMessage("SpawnPlayer");
            }
            i++;
        }
        localPlayer.playerControl.inventory.hud.GetComponent<HUD>().ChangeRemain(playersCount);
    }

    [PunRPC]
    public int EliminatePlayer(Player player)
    {
        if(playersReady[player])
        {
            playersReady[player] = false;
            playersCount--;
            scoreBoard.Push(player);
            Debug.LogFormat("Player {0} eliminated", player.ActorNumber);

            // Destroy gameobject
            // Destroy(players[player].playerControl.gameObject);
        }
        localPlayer.playerControl.inventory.hud.GetComponent<HUD>().ChangeRemain(playersCount);
        return playersCount;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        EliminatePlayer(otherPlayer);
    }
}
