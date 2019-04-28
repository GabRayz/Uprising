using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Photon.Pun;
using UnityEngine.UI;
using Uprising.Players;

public class MainMenu : MonoBehaviour
{
    public Text matchMakingText;
    private NetworkManager networkManager;
    public RawImage avatar;
    public Text pseudo;
    public Text level;
    public Slider levelProgress;

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

    public void SetPlayerInfo()
    {
        PlayerStats stats = networkManager.localPlayerGameStats;
        this.pseudo.text = networkManager.localPlayerGameStats.pseudo;
        this.level.text = networkManager.localPlayerGameStats.level.ToString();

        this.levelProgress.minValue = stats.level > 1 ? 100 * Mathf.Pow(2, stats.level - 2) : 0;
        levelProgress.maxValue = 100 * Mathf.Pow(2, stats.level - 1);
        levelProgress.value = stats.xp;
    }
}
