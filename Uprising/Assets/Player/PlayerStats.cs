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
        public float time;
        public bool isActive;

        public PlayerStats(PlayerControl playerControl)
        {
            if (playerControl == null) return;
            this.playerControl = playerControl;
            this.owner = playerControl.photonView.Owner;
        }

        public void Reset()
        {
            playerControl = null;
            killer = null;
            belettesShot = 0;
            hits = 0;
            kills = 0;
            time = 0f;
            isActive = false;
        }
    }
}
