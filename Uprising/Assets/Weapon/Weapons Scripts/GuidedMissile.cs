using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Players;

namespace Uprising.Items
{
    public class GuidedMissile : Weapon
    {
        public GuidedMissile(int ammo, GameObject player)
        {
            this.durability = ammo;
            this.player = player;
            type = ItemType.GuidedMissile;
        }

        public override void Aim()
        {
            Debug.Log("Aimed.");
        }

        public override void Use()
        {
            if (fireratetime >= firerate)
            {
                this.player.GetComponent<PlayerControl>().hand.transform.Find("h_GuidedMissile").GetComponent<RocketGen>().Shoot(this);
                durability--;
                if (durability <= 0)
                    player.SendMessage("ClearItem", this);
                fireratetime = 0;
            }
        }

        protected override void StopUsing()
        {
            throw new System.NotImplementedException();
        }
    }
}

