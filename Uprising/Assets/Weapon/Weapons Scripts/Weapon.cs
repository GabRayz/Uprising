
using UnityEngine;
namespace Uprising.Items
{
    public abstract class Weapon : Item 
    {
        public GameObject belette;
        public GameObject weapon;

        public float accuracy;
        public float range;
        public float firerate;
        public float fireratetime;
        public float knockback = 1;

        public abstract void Aim(); //Aim

        protected override void StopUsing()
        {
            //Destroy(this);
            Debug.Log("stopuseweapon.");
        }

        public override void Reload()
        {
            fireratetime += playerControl.firerateModifier;
        }
    }
}