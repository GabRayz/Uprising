using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using Uprising.Players;

public class GameManager : MonoBehaviour
{
    SpawnPlayers[] spawnSpots;
    GameObject lava;
    public PhotonView photonView;
    float lavaRisingSpeed = 0.1f;
    public bool OfflineMode = false;
    private byte PlayerEliminationEvent = 0;
    private int playersCount;
    Dictionary<Player, bool> players;

    public bool isStarted;
    public float startCooldown = 5;
    public bool isCooldownStarted;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        Debug.Log("Map 1 scene loaded !");
        players = new Dictionary<Player, bool>();
        // Get all possible spawn points
        spawnSpots = FindObjectsOfType<SpawnPlayers>();
        if(!OfflineMode) SpawnPlayers();
        lava = GameObject.Find("Lava");
    }

    private void Update()
    {
        if(isStarted) // Raise lava
            lava.transform.Translate(Vector3.up * lavaRisingSpeed * Time.deltaTime);
        // Finish game
        if(playersCount <= 1 && !OfflineMode)
        {
            Debug.Log("Game over");
            if(PhotonNetwork.CurrentRoom.PlayerCount > 1 && PhotonNetwork.IsMasterClient)
            {
                foreach (var player in PhotonNetwork.CurrentRoom.Players)
                {
                    if (player.Value != PhotonNetwork.LocalPlayer) {
                        this.photonView.TransferOwnership(player.Value);
                        break;
                    }
                }
                GameObject.Find("_network").GetComponent<NetworkManager>().QuitGame();
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                GameObject.Find("_network").GetComponent<NetworkManager>().QuitGame(true);
            }
            else
            {
                GameObject.Find("_network").GetComponent<NetworkManager>().QuitGame();
            }
        }

        if(!isStarted)
        {
            // Check if players are ready to start
            if(!players.ContainsValue(false))
            {
                isCooldownStarted = true;
            }
        }

        // Cooldown
        if (isCooldownStarted && startCooldown > 0)
        {
            startCooldown = startCooldown - Time.deltaTime;
            if(startCooldown <= 0)
            {
                isCooldownStarted = false;
                isStarted = true;
            }
        }
    }

    [PunRPC]
    public void SetReady(Player player)
    {
        players[player] = true;
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
            players.Add(player.Value, false);
            if (self.UserId == player.Value.UserId)
            {
                if(player.Key <= spawnSpots.Length)
                    spawnSpots[player.Key - 1].SendMessage("SpawnPlayer");
            }
            i++;
        }
    }

    [PunRPC]
    public int EliminatePlayer(Player player)
    {
        if(players[player])
        {
            players[player] = false;
            playersCount--;
            Debug.LogFormat("Player {0} eliminated", player.ActorNumber);
        }
        return playersCount;
    }
}
