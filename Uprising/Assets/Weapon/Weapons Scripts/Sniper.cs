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
    }
}
