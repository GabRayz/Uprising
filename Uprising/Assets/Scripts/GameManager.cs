using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviour
{
    SpawnPlayers[] spawnSpots;
    GameObject lava;
    float lavaRisingSpeed = 0.5f;
    public bool OfflineMode = false;
    private byte PlayerEliminationEvent = 0;
    private int playersCount;
    Dictionary<Player, bool> players;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Map 1 scene loaded !");
        players = new Dictionary<Player, bool>();
        // Get all possible spawn points
        spawnSpots = FindObjectsOfType<SpawnPlayers>();
        if(!OfflineMode) SpawnPlayers();
        lava = GameObject.Find("Lava");
    }

    private void Update()
    {
        lava.transform.Translate(Vector3.up * lavaRisingSpeed * Time.deltaTime);
        if(playersCount <= 1 && !OfflineMode)
        {
            Debug.Log("Game over");
            PhotonNetwork.LeaveRoom();
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(1));
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));
        }
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
            players.Add(player.Value, true);
            if (self.UserId == player.Value.UserId)
            {
                if(player.Key <= spawnSpots.Length)
                    spawnSpots[player.Key - 1].SendMessage("SpawnPlayer");
            }
            i++;
        }
    }

    public int EliminatePlayer(Player player)
    {
        if(players[player])
        {
            players[player] = false;
            playersCount--;
        }
        return playersCount;
    }
}
