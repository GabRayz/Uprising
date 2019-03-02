using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Uprising.Item
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
            Belette NewBelette = new Belette(range, belette, this);
            //GameObject NewBelette = Instantiate(belette, this.transform);
            //NewBelette.GetComponent<Rigidbody>().AddForce(this.transform.forward * 100);
        }

        public override void Select()
        {
            weapon.SetActive(true);
        }

        public override void Unselect()
        {
            weapon.SetActive(false);
        }
    }
}