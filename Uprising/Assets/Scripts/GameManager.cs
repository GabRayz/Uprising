﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using Uprising.Players;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    SpawnPlayers[] spawnSpots;
    GameObject lava;
    public new PhotonView photonView;
    public Text topText;
    float lavaRisingSpeed = 0.1f;
    public bool OfflineMode = false;
    private byte PlayerEliminationEvent = 0;
    public int playersCount;
    public Dictionary<Player, bool> players;
    Stack<Player> scoreBoard;
    PlayerStats localPlayer;
    List<PlayerStats> playerStats = new List<PlayerStats>();
    public float lavaLevel;

    // Cooldown before starting the game
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
        scoreBoard = new Stack<Player>();
        topText.text = "Waiting for players";
    }

    private void Update()
    {
        if(isStarted) // Raise lava
            lava.transform.Translate(Vector3.up * lavaRisingSpeed * Time.deltaTime);
        lavaLevel = lava.transform.position.y;
        // Finish game
        if(playersCount <= 1 && !OfflineMode)
        {
            FinishGame();
        }

        // Cooldown
        if (isCooldownStarted && startCooldown > 0)
        {
            startCooldown = startCooldown - Time.deltaTime;
            topText.text = "Start in " + Mathf.Floor(startCooldown);
            if(startCooldown <= 0)
            {
                Debug.Log("Begin!");
                topText.text = "";
                isCooldownStarted = false;
                isStarted = true;
            }
        }
    }

    void FinishGame()
    {
        Debug.Log("Game over");
        foreach (var playerStatus in players)
        {
            if (playerStatus.Value && !playerStatus.Key.IsInactive)
                scoreBoard.Push(playerStatus.Key);
        }

        GameObject.Find("_network").GetComponent<NetworkManager>().LeaveToScoreBoard(scoreBoard, playerStats);
    }

    [PunRPC]
    public void SetPlayerStat(PlayerStats playerStats)
    {
        this.playerStats.Add(playerStats);
    }

    [PunRPC]
    public void SetReady(Player player)
    {
        players[player] = true;
        int readyCount = 0;
        foreach(var playerReady in players)
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
        isCooldownStarted = true;
        Debug.Log("All players ready, starting cooldown");
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            players[player.Value] = true;
        }
        topText.text = "";
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
            scoreBoard.Push(player);
            Debug.LogFormat("Player {0} eliminated", player.ActorNumber);
        }
        return playersCount;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        EliminatePlayer(otherPlayer);
    }
}
