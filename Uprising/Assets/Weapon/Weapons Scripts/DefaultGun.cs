using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Players;

namespace Uprising.Items
{
    public class DefaultGun : Weapon
    {
        public Camera cam;

        public DefaultGun(int durability, float accuracy, float firerate, float knockback, GameObject player)
        {
            this.type = ItemType.DefaultGun;
            this.durability = durability;
            this.accuracy = accuracy;
            this.firerate = firerate;
            this.knockback = knockback;
            this.player = player;

            // cam = player.GetComponent<Camera>();
            fireratetime = firerate;
        }

        public override void Aim()
        {
            Debug.Log("Aimed.");
            
        }

        //public override void Use()
        //{
        //    if (fireratetime >= firerate)
        //    {
        //        if (belettegen == null)
        //            belettegen = player.GetComponent<PlayerControl>().hand.transform.Find("h_DefaultGun").GetComponent<belettegen>();
        //        belettegen.shoot(this);
        //        if(player.GetComponent<PlayerControl>().playerStats != null)
        //            player.GetComponent<PlayerControl>().playerStats.belettesShot += 1;
        //        fireratetime = 0;
        //    }
        //    if(durability < 0)
        //    {
        //        StopUsing();
        //    }
        //}
    }
}