using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Uprising.Players
{
    public class PlayerStats
    {
        public PlayerControl playerControl;
        public Player owner;

        public PlayerStats(PlayerControl playerControl)
        {
            this.playerControl = playerControl;
        }
    }
}
