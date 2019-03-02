using System;
using UnityEngine;

namespace Uprising.Items
{
    public class SpeedBoost : Effect
    {
        public SpeedBoost(int time, GameObject player)
        {
            this.type = ItemType.SpeedBoost;
            // time is in millisecond
            this.durability = time;
            this.player = player;
        }
    }
}
