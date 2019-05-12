using UnityEngine;
using Uprising.Players;

namespace Uprising.Items
{
    public class RocketLauncher : Weapon
    {
        public RocketLauncher(int ammo, GameObject player)
        {
            this.durability = ammo;
            this.player = player;
            type = ItemType.RocketLauncher;
        }

        public override void Aim()
        {
            Debug.Log("Aimed.");
        }

        public override void Use()
        {
            if (fireratetime >= firerate)
            {
                this.player.GetComponent<PlayerControl>().hand.transform.Find("h_RocketLauncher").GetComponent<RocketGen>().Shoot(this);
                durability--;
                if (durability <= 0)
                    player.SendMessage("ClearItem", this);
                fireratetime = 0;
            }
        }

        protected override void StopUsing()
        {
            throw new System.NotImplementedException();
        }
    }

}