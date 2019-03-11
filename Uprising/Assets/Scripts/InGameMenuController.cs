using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Players;

public class InGameMenuController : MonoBehaviour
{
    public PlayerControl player;

    public void SetOwner(PlayerControl player)
    {
        this.player = player;
        Debug.Log("new owner set");
    }

    public void Quit()
    {
        player.Quit();
    }
}
