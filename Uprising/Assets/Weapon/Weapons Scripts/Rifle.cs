using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Players;

namespace Uprising.Items
{
    public class Rifle : Weapon
    {
        public Rifle(int durability, float accuracy, float firerate, float knockback, GameObject player)
        {
            this.type = ItemType.Rifle;
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

        //public override void Use()
        //{
        //    if (fireratetime >= firerate)
        //    {
        //        player.GetComponent<PlayerControl>().hand.transform.Find("h_Rifle").GetComponent<belettegen>().shoot(durability, this.target.transform.forward, this);
        //        if (player.GetComponent<PlayerControl>().playerStats != null)
        //            player.GetComponent<PlayerControl>().playerStats.belettesShot += 1;
        //        fireratetime = 0;
        //        durability--;
        //    }
        //    if (durability < 0)
        //    {
        //        player.SendMessage("ClearItem", this as Item);
        //    }
        //}

        //IEnumerator ShootSeries()
        //{
        //    float time = 0;
        //    for (var i = 0; i < 2; i++)
        //    {
        //        while (time < 0.3)
        //        {
        //            time += Time.deltaTime;
        //            yield return null;
        //        }

        //        player.GetComponent<PlayerControl>().hand.transform.Find("h_Rifle").GetComponent<belettegen>().shoot(durability, this.target.transform.forward, this);
        //        if (player.GetComponent<PlayerControl>().playerStats != null)
        //            player.GetComponent<PlayerControl>().playerStats.belettesShot += 1;
        //        durability--;
        //    }
        //}
    }
}
