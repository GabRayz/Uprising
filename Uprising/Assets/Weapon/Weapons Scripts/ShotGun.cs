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
                player.GetComponent<PlayerControl>().hand.transform.Find("h_ShotGun").GetComponent<belettegen>().shoot(durability, this.player.transform.forward, this);
                player.GetComponent<PlayerControl>().hand.transform.Find("h_ShotGun").GetComponent<belettegen>().shoot(durability, new Vector3(-0.3f, 0.2f, 0.4f), this);
                player.GetComponent<PlayerControl>().hand.transform.Find("h_ShotGun").GetComponent<belettegen>().shoot(durability, new Vector3(0.3f, 0.1f, 0.4f), this);
                player.GetComponent<PlayerControl>().hand.transform.Find("h_ShotGun").GetComponent<belettegen>().shoot(durability, new Vector3(0.3f, 0.2f, 0.4f), this);
                player.GetComponent<PlayerControl>().hand.transform.Find("h_ShotGun").GetComponent<belettegen>().shoot(durability, new Vector3(-0.3f, 0f, 0.3f), this);
                player.GetComponent<PlayerControl>().hand.transform.Find("h_ShotGun").GetComponent<belettegen>().shoot(durability, new Vector3(0f, 0f, 0.2f), this);
                durability--;
                fireratetime = 0;
            }

            if (durability < 0)
            {
                StopUsing();
            }
        }
    }
}