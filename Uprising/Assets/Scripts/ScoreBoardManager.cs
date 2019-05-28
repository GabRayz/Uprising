using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Players;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviour
{
    public Text result;
    public Text scoreboard;
    public GameObject lign1;
    public GameObject lign2;
    public GameObject lign3;
    public GameObject lign4;
    public GameObject lign5;
    public GameObject lign6;
    public GameObject lign7;
    public GameObject lign8;
    public GameObject lign9;
    public GameObject lign10;

    private GameObject[] ligns;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ScoreBoard scene loaded");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ligns = new GameObject[] { lign1, lign2, lign3, lign4, lign5, lign6, lign7, lign8, lign9, lign10 };
    }

    public void SetStats(Stack<Player> scoreboard, Dictionary<Player, PlayerStats> players)
    {
        int i = 0;
        Player winner;
        while(scoreboard.Count > 0)
        {
            Player player = scoreboard.Pop();
            if (i == 0)
                players[player].winner = true;

            ligns[i].SetActive(true);
            ligns[i].transform.Find("Name").GetComponent<Text>().text = string.IsNullOrEmpty(player.NickName) ? "Player " + player.ActorNumber : player.NickName;
            ligns[i].transform.Find("Kills").GetComponent<Text>().text = players[player].kills.ToString();
            ligns[i].transform.Find("Time").GetComponent<Text>().text = GetTime((int)players[player].time);
            ligns[i].transform.Find("Points").GetComponent<Text>().text = GetScore(players[player]).ToString();
            i++;

            Debug.Log(players[player].pseudo + " Shots : " + players[player].belettesShot + "; hits : " + players[player].hits);
        }

        Debug.Log("ScoreBoard filled");
    }

    public void Leave()
    {
        GameObject.Find("_network").GetComponent<NetworkManager>().QuitGame(PhotonNetwork.CurrentRoom.PlayerCount == 1);
    }

    private string GetTime(int time)
    {
        int min = time / 60;
        string res = (min.ToString().Length < 2) ? "0" + min.ToString() : min.ToString();
        int s = time % 60;
        res += ":" + ((s.ToString().Length < 2) ? "0" + s.ToString() : s.ToString());
        return res;
    }

    private int GetScore(PlayerStats player)
    {
        int score = (int)player.time;
        score += player.kills * 100;
        Debug.Log("score : " + score);
        if(player.belettesShot != 0)
            score += player.hits * 5;
        Debug.Log("score with ratio : " + score);
        Debug.Log("Shots : " + player.belettesShot + "; hits : " + player.hits);
        return score;
    }
}
