using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Players;

namespace Uprising.Items
{
    public class DefaultGun : Weapon
    {

        public DefaultGun(int durability, float accuracy, float firerate, float knockback, GameObject player)
        {
            this.type = ItemType.DefaultGun;
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
            //Belette NewBelette = new Belette(range, belette, this);
            //GameObject NewBelette = Instantiate(belette, weapon.gameObject.transform);

            //NewBelette.GetComponent<Rigidbody>().AddForce(weapon.gameObject.transform.forward * 100);

            //GameObject NewBelette = Instantiate(belette, this.transform);
            //NewBelette.GetComponent<Rigidbody>().AddForce(this.transform.forward * 100);

            player.GetComponent<PlayerControl>().hand.transform.Find("h_DefaultGun").GetComponent<belettegen>().shoot( durability, this.player.transform.forward);
        }
    }
}