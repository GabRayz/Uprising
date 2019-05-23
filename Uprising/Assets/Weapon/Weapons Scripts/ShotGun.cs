using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Players;

namespace Uprising.Items
{
    public class ShotGun : Weapon
    {

        public ShotGun(int durability, float accuracy, float firerate, float knockback, GameObject player)
        {
            this.type = ItemType.ShotGun;
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
                for (var i = 0; i < 6; i++)
                {
                    Shoot();
                }

                fireratetime = 0;
                durability--;
            }

            if (durability <= 0)
            {
                player.SendMessage("ClearItem", this as Item);
            }
        }
    }
}