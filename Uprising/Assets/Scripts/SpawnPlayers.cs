using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public bool available = true;

    public void SpawnPlayer()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
        available = false;
    }
}
