using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    SpawnPlayers[] spawnSpots;

    // Start is called before the first frame update
    void Start()
    {
        // Get all possible spawn points
        spawnSpots = FindObjectsOfType<SpawnPlayers>();
        SpawnPlayers();
    }

    public void SpawnPlayers()
    {
        // Place the local player at the corresponding spawn point, according to his order in the list of room's players
        int i = 0;
        Player self = PhotonNetwork.LocalPlayer;
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            if(self.UserId == player.Value.UserId)
            {
                spawnSpots[i].SendMessage("SpawnPlayer");
            }
            i++;
        }
    }
}
