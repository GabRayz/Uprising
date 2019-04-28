using System;
using Uprising.Players;
using UnityEngine;
using Photon.Pun;

namespace Uprising.Items
{
    public class Mine : Item
    {
        public Mine(int time, GameObject player)
        {
            this.durability = time;
            this.player = player;
        }

        public override void Use()
        {
            PhotonNetwork.InstantiateSceneObject("PlacedMine", this.player.transform.position + this.player.transform.forward / 2 + player.transform.up, this.player.transform.rotation);
            player.SendMessage("ClearItem", this as Item);
        }

        protected override void StopUsing()
        {
            // Nothing to do
        }
    }
}
