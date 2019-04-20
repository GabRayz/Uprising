using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public bool available = true;

    public void SpawnPlayer()
    {
        PhotonNetwork.Instantiate("Player", this.transform.position, this.transform.rotation);
        available = false;
        Debug.Log("Player " + PhotonNetwork.LocalPlayer.ActorNumber + " spawned");
    }
}
