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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ScoreBoard scene loaded");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetStats(Stack<Player> scoreboard, PlayerStats stats)
    {
        this.scoreboard.text = "";
        int i = 0;
        Player winner;
        while(scoreboard.Count > 0)
        {
            Player player = scoreboard.Pop();
            if (i == 0)
            {
                if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                    result.text = "Victory !";
                else
                    result.text = "Defeat !";
            }
            this.scoreboard.text += i + ". Player " + player.ActorNumber + "\n";
            i++;
        }

        Debug.Log("ScoreBoard filled");
    }

    public void Leave()
    {
        GameObject.Find("_network").GetComponent<NetworkManager>().QuitGame(PhotonNetwork.CurrentRoom.PlayerCount ==1);
    }
}
