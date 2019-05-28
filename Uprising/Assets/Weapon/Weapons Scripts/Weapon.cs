using UnityEngine;
using Uprising.Players;
using Photon.Pun;

namespace Uprising.Items
{
    public abstract class Weapon : Item 
    {
        public GameObject belette;
        public GameObject weapon;
        public belettegen belettegen;

        public float accuracy;
        public float range;
        public float firerate;
        public float fireratetime;
        public float knockback = 1;

        public abstract void Aim(); //Aim

        public override void Use()
        {
            if (fireratetime >= firerate)
            {
                Shoot();
                fireratetime = 0;
                durability--;
            }


            if (durability <= 0)
                playerControl.inventory.ClearItem(this as Item);
        }

        protected void Shoot()
        {
            if (belettegen == null) // Get the belletegen
                belettegen = player.GetComponent<PlayerControl>().hand.transform.Find("h_" + type.ToString()).GetComponent<belettegen>();

            // Shoot
            belettegen.shoot(this);

            // Update statistics
            if (PhotonNetwork.IsConnected)
                player.GetComponent<PlayerControl>().photonView.RPC("OnShoot", RpcTarget.All, 1);
        }

        protected override void StopUsing()
        {

        }

        public override void Reload()
        {
            fireratetime += playerControl.firerateModifier;
        }
    }
}