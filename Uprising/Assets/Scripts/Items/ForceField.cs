using System;
using UnityEngine;
using Photon.Pun;

namespace Uprising.Items
{
    public class ForceField : Item
    {
        public ForceField(int time, GameObject player)
        {
            this.player = player;
            this.durability = time;
            this.type = ItemType.ForceField;
        }

        public override void Use()
        {
            if (!this.isCurrentlyUsed)
            {
                isCurrentlyUsed = true;
                this.player.transform.Find("active_ForceField").gameObject.SetActive(true);
                player.SendMessage("ClearItem", this as Item);
            }
        }

        protected override void StopUsing()
        {
            // Does nothing
        }
    }
}
