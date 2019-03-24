using System;
using UnityEngine;

namespace Uprising.Items
{
    public class JumpBoost : Effect
    {
        public JumpBoost(int time, GameObject player)
        {
            this.durability = time;
            this.player = player;
            this.type = ItemType.JumpBoost;
        }
    }
}
