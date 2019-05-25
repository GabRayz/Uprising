using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Players;

namespace Uprising.Items
{
    public class MachineGun : Weapon
    {
        public MachineGun(int durability, float accuracy, float firerate, float knockback, GameObject player)
        {
            this.type = ItemType.MachineGun;
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