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
        public Player killer;
        public int belettesShot;
        public int hits;
        public int kills;
        public int time;
        public bool isActive;

        public PlayerStats(PlayerControl playerControl)
        {
            this.playerControl = playerControl;
            this.owner = playerControl.photonView.Owner;
        }
    }
}
