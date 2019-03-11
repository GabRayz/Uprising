using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Uprising.Items
{
    public class DefaultGun : Weapon
    {
        public DefaultGun(int durability, float accuracy, float range, float firerate, float knockback)
        {
            this.durability = durability;
            this.accuracy = accuracy;
            this.range = range;
            this.firerate = firerate;
            this.knockback = knockback;
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

            player.transform.Find("h_defaultgun").GetComponent<belettegen>().shoot(range, durability);
            Debug.Log("message");

        }
    }
}