using UnityEngine;
using Uprising.Players;

namespace Uprising.Items
{
    public class Drugs : Effect
    {
        public Drugs(int time, GameObject player)
        {
            durability = time;
            this.player = player;

            type = ItemType.Drugs;
        }
    }
}