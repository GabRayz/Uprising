using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Players;

namespace Uprising.Items
{
    public class Sniper : Weapon
    {

        public Sniper(int durability, float accuracy, float firerate, float knockback, GameObject player)
        {
            this.type = ItemType.Sniper;
            this.durability = durability;
            this.accuracy = accuracy;
            this.firerate = firerate;
            this.knockback = knockback;
            this.player = player;

            fireratetime = firerate;
        }

        public override void Aim()
        {
            Debug.Log("Aimed.");

        }

        public override void Use()
        {
            if (fireratetime >= firerate)
            {
                player.GetComponent<PlayerControl>().hand.transform.Find("h_Sniper").GetComponent<belettegen>().shoot(durability, this.player.transform.forward*25, this);
                durability--;
                fireratetime = 0;
            }
            if (durability < 0)
            {
                player.SendMessage("ClearItem", this as Item);
            }
        }
    }
}
