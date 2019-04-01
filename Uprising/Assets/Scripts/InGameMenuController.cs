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
    }

    public void Resume()
    {
        player.ToggleMenu();
    }

    public void Quit()
    {
        player.Quit();
    }
}
