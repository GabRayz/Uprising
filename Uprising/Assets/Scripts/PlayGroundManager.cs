using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayGroundManager : MonoBehaviour
{
    private NetworkManager networkManager;
    public GameObject game;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2));
        networkManager = GameObject.Find("_network").GetComponent<NetworkManager>();
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject newGame = PhotonNetwork.InstantiateSceneObject("Game", Vector3.zero, Quaternion.identity);
        }
        networkManager.OnGameLoaded();
    }
}
