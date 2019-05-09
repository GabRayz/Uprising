using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Players;

namespace Uprising.Items
{
    public class Grapnel : Item
    {
        public Grapnel(int durability, GameObject player)
        {
            this.durability = durability;
            this.player = player;
            this.type = ItemType.Grapnel;
        }

        public override void Use()
        {
            this.player.GetComponent<PlayerControl>().hand.transform.Find("h_Grapnel").GetComponent<GrapnelController>().Shoot();
        }

        protected override void StopUsing()
        {
            throw new System.NotImplementedException();
        }
    }

}