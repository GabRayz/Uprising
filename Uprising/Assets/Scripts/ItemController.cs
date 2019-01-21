using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Item controller.
/// Declares classes for collected item, currently in tha player's inventory.
/// </summary>

public class ItemController : MonoBehaviour
{
    private abstract class Item
    {
        protected int durability;
        public bool isCurrentlyUsed = false;
        public GameObject player;

        public int GetDurability()
        {
            return durability;
        }

        // Called by Player's behavior to use the item.
        public abstract void Use();
    }

    private abstract class Effect : Item
    {
        // Called every frame
        public void Update()
        {
            // Remove from durability the time passed since the last frame
            this.durability -= (int)(Time.deltaTime * 1000);
            if(this.durability <= 0)
            {
                this.StopUsing();
            }
        }

        protected abstract void StopUsing();
    }

    private class SpeedBoost : Effect
    {
        public SpeedBoost(int time, GameObject player)
        {
            // time is in millisecond
            this.durability = time;
            this.player = player;
        }

        public override void Use()
        {
            if(!isCurrentlyUsed)
            {
                this.isCurrentlyUsed = true;
                this.player.SendMessage("ModifySpeed", 0.5);
            }
        }

        protected override void StopUsing()
        {
            if(isCurrentlyUsed)
            {
                this.isCurrentlyUsed = false;
                this.player.SendMessage("ModifySpeed", -0.5);
            }
        }
    }
}
