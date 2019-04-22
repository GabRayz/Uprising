using System;
using UnityEngine;
using Photon.Pun;
using Uprising.Players;

namespace Uprising.Items
{
    public class BearTrap : Item
    {
        public BearTrap(GameObject player)
        {
            this.player = player;
            this.type = ItemType.BearTrap;
        }

        public override void Use()
        {
            PhotonNetwork.InstantiateSceneObject("PlacedBearTrap", this.player.transform.position + this.player.transform.forward / 2 + player.transform.up, this.player.transform.rotation);
            PhotonNetwork.Instantiate("PlacedBearTrap", this.player.transform.position, this.player.transform.rotation);
            player.SendMessage("ClearItem", this as Item);
        }

        protected override void StopUsing()
        {
            // Nothing to do
        }
    }
}
