using System;
using UnityEngine;
using Photon.Pun;

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
            Debug.Log("Bear trap placed");
            PhotonNetwork.InstantiateSceneObject("BearTrap", this.player.transform.position + this.player.transform.forward, this.player.transform.rotation);
            player.SendMessage("ClearItem", this as Item);
        }

        protected override void StopUsing()
        {
            // Nothing to do
        }
    }
}
