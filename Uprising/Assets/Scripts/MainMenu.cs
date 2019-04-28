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
    public RawImage avatar;
    public Text pseudo;
    public Text level;
    public GameObject levelProgress;

    public void Start()
    {
        networkManager = GameObject.Find("_network").GetComponent<NetworkManager>();
        networkManager.SetMainMenu(this);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayRandom()
    {
        networkManager.PlayRandom();
    }

    public void CancelPlay()
    {
        networkManager.CancelPlay();
        matchMakingText.text = "Canceling...";
    }
}
