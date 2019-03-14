using System;
using UnityEngine;

namespace Uprising.Items
{
    public class Invisibility : Effect
    {
        public Invisibility(int time, GameObject player)
        {
            this.type = ItemType.Invisibility;
            this.durability = time;
            this.player = player;
        }
    }
}
