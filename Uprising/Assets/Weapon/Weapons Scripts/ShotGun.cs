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
        }

        public override void Aim()
        {
            Debug.Log("Aimed.");
        }

        public override void Use()
        {
            player.GetComponent<PlayerControl>().hand.transform.Find("h_ShotGun").GetComponent<belettegen>().shoot( durability, this.player.transform.forward);
            player.GetComponent<PlayerControl>().hand.transform.Find("h_ShotGun").GetComponent<belettegen>().shoot(durability, new Vector3(-0.1f, 0.1f, 0.3f));
            player.GetComponent<PlayerControl>().hand.transform.Find("h_ShotGun").GetComponent<belettegen>().shoot( durability, new Vector3(0.1f, 0.1f, 0.2f));
            player.GetComponent<PlayerControl>().hand.transform.Find("h_ShotGun").GetComponent<belettegen>().shoot( durability, new Vector3(0.1f, 0.1f, 0.3f));
            player.GetComponent<PlayerControl>().hand.transform.Find("h_ShotGun").GetComponent<belettegen>().shoot( durability, new Vector3(-0.1f, 0f, 0.2f));
            player.GetComponent<PlayerControl>().hand.transform.Find("h_ShotGun").GetComponent<belettegen>().shoot( durability, new Vector3(0f, 0f, 0.2f));
        }
    }
}