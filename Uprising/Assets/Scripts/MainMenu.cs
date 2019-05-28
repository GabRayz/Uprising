using System;
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

    public string[] resolutions;
    public Dropdown resDD;
    public Toggle fullscreenToggle;
    int width;
    int height;
    bool fullscreen;

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

#if UNITY_WEBGL
    public void Quit()
    {
        Debug.Log("Quit Uprising");

        networkManager.QuitApp();
    }
#endif

    public void ApplyPref()
    {
        Debug.Log("Apply settings");

        string res = resolutions[resDD.value];
        fullscreen = fullscreenToggle.isOn;
        string[] resSplit = res.Split('x');
        width = Int32.Parse(resSplit[0]);
        height = Int32.Parse(resSplit[1]);

        Screen.SetResolution(width, height, fullscreen);
    }
}
