using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Uprising.Items
{
    public class Shield : Item
    {
        ShieldControl shieldControl;

        public Shield(int durability, GameObject player)
        {
            this.durability = durability;
            this.player = player;

            this.type = ItemType.Shield;
        }

        public override void Select()
        {
            if (shieldControl == null)
            {
                this.shieldControl = player.transform.Find("Shield").GetComponent<ShieldControl>();
                shieldControl.Init(durability, this);
            }

            shieldControl.gameObject.SetActive(true);
            playerControl.ModifySpeed(-2.5f);
            playerControl.ModifyJumpHeight(-400);
        }

        public override void Unselect()
        {
            shieldControl.gameObject.SetActive(false);
            playerControl.ModifySpeed(2.5f);
            playerControl.ModifyJumpHeight(400);
        }

        public void Break()
        {
            this.Unselect();
            playerControl.inventory.ClearItem(this as Item);
        }

        public override void Use()
        {
            //
        }

        protected override void StopUsing()
        {
            //
        }
    }
}