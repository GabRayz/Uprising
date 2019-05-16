using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Players;

namespace Uprising.Items
{
    public class Grapnel : Item
    {
        GrapnelController grapnel;

        public Grapnel(int durability, GameObject player)
        {
            this.durability = durability;
            this.player = player;
            this.type = ItemType.Grapnel;
        }

        public override void Use()
        {
            if (!isCurrentlyUsed)
            {
                durability--;
                this.isCurrentlyUsed = true;
                grapnel = this.player.GetComponent<PlayerControl>().hand.transform.Find("h_Grapnel").GetComponent<GrapnelController>();
                grapnel.Shoot(this);
            }

        }

        public override void Unselect()
        {
            player.GetComponent<PlayerControl>().hand.transform.Find("h_" + type.ToString()).gameObject.SetActive(false);
            Detach();
        }

        protected override void StopUsing()
        {
            if(isCurrentlyUsed)
                Detach();
        }

        public void Detach()
        {
            if (grapnel != null && grapnel.flyingHook != null)
            {
                grapnel.Detach();


                isCurrentlyUsed = false;
                if (durability <= 0)
                {
                    player.GetComponent<InventoryManager>().ClearItem(this);
                }
            }
        }
    }

}