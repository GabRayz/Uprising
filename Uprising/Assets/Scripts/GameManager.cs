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
        spawnSpots = FindObjectsOfType<SpawnPlayers>();
        SpawnPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayers()
    {
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
