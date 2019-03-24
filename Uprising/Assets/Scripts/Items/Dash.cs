using System;
using UnityEngine;
using Uprising.Players;

namespace Uprising.Items
{
    public class Dash : Item
    {
        public Dash(int dashCount, GameObject player)
        {
            this.type = ItemType.Dash;

            this.durability = dashCount;
            this.player = player;
        }

        public override void Use()
        {
            if(!isCurrentlyUsed)
            {
                this.isCurrentlyUsed = true;
                this.player.GetComponent<PlayerControl>().dashLeft = durability;
                player.SendMessage("ClearItem", this as Item);
            }
        }

        protected override void StopUsing()
        {
            // Does nothing
        }
    }
}
