using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Uprising.Players
{
    public class PlayerStats
    {
        public string pseudo;
        public int level;
        public float levelProgress;
        public int xp;

        public PlayerControl playerControl;
        public Player owner;
        public Player killer;
        public int belettesShot;
        public int hits;
        public int kills;
        public float time;
        public bool isActive;

        public bool winner;

        public PlayerStats(PlayerControl playerControl)
        {
            this.pseudo = "";
            level = 0;
            levelProgress = 0;
            xp = 0;
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

        public void OnGameEnd()
        {
            int earnedXP = ComputeXP();
            xp += earnedXP;
            if (xp > 100 * Math.Pow(2, level - 1))
            {
                level++;
            }
        }

        public int ComputeXP()
        {
            int res = (int)time;
            res += kills * 100;
            if (belettesShot != 0)
                res += (hits / belettesShot) * belettesShot * 2;
            return res;
        }
    }

    [Serializable]
    public class PlayerStatsJson
    {
        public int xp;
        public bool winner;
        public int shotCount;
        public int accurateShotCount;
    }
}
