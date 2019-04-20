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

            // this.scoreboard.text += i + ". Player " + player.ActorNumber + "\n";
            ligns[i].SetActive(true);
            ligns[i].transform.Find("Name").GetComponent<Text>().text = player.NickName;
            ligns[i].transform.Find("Kills").GetComponent<Text>().text = players[player].kills.ToString();
            ligns[i].transform.Find("Time").GetComponent<Text>().text = "00:00";
            ligns[i].transform.Find("Points").GetComponent<Text>().text = "0";
            i++;
        }

        Debug.Log("ScoreBoard filled");
    }

    public void Leave()
    {
        GameObject.Find("_network").GetComponent<NetworkManager>().QuitGame(PhotonNetwork.CurrentRoom.PlayerCount == 1);
    }
}
