using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Photon.Pun;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text matchMakingText;
    private NetworkManager networkManager;

    public void Start()
    {
        networkManager = GameObject.Find("_network").GetComponent<NetworkManager>();
        networkManager.SetMainMenu(this);
    }

    public void PlayRandom()
    {
        networkManager.PlayRandom();
    }

    public void CancelPlay()
    {
        networkManager.CancelPlay();
    }
}
