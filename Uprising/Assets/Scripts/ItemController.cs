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
    public abstract class Item
    {
        public ItemType type;
        public int durability;
        public bool isCurrentlyUsed = false;
        public GameObject player;

        public int GetDurability()
        {
            return durability;
        }

        // Called by Player's behavior to use the item.
        public abstract void Use();
        protected abstract void StopUsing();
    }

    public abstract class Effect : Item
    {
        // Called every frame
        public void Update()
        {
            // Remove from durability the time passed since the last frame.
            this.durability -= (int)(Time.deltaTime * 1000);
            if (this.durability <= 0)
            {
                this.StopUsing();
            }
        }

        public override void Use()
        {
            if(!isCurrentlyUsed)
            {
                this.isCurrentlyUsed = true;
                this.player.SendMessage("ApplyEffect", this);
            }
        }

        protected override void StopUsing()
        {
            if (isCurrentlyUsed)
            {
                this.isCurrentlyUsed = false;
                this.player.SendMessage("UnApplyEffect", this);
            }
        }
    }

    public class SpeedBoost : Effect
    {
        public SpeedBoost(int time, GameObject player)
        {
            this.type = ItemType.SpeedBoost;
            // time is in millisecond
            this.durability = time;
            this.player = player;
        }

        //public override void Use()
        //{
        //    if (!isCurrentlyUsed)
        //    {
        //        this.isCurrentlyUsed = true;
        //        this.player.SendMessage("ModifySpeed", 1);
        //    }
        //}

        //protected override void StopUsing()
        //{
        //    if (isCurrentlyUsed)
        //    {
        //        this.isCurrentlyUsed = false;
        //        this.player.SendMessage("ModifySpeed", -1);
        //    }
        //}
    }

    public abstract class Weapon : Item
    {
        //
        public Weapon()
        {

        }

        // Called every frame
        public void Update()
        {
            if (this.durability <= 0)
            {
                this.StopUsing();
            }
        }

        public override void Use() // shoot
        {
            if (!isCurrentlyUsed)
            {
                this.durability--;
                // ici mettre le truc pour créer bullet
                this.isCurrentlyUsed = true;
            }
        }

        protected override void StopUsing()
        {
            this.isCurrentlyUsed = false;
        }

        public abstract void Aim();
    }

    public class DefaultGun : Weapon
    {

        public DefaultGun(int durability)
        {
            this.durability = durability;
        }

        public override void Aim()
        {
            throw new System.NotImplementedException();
        }
    }

    public class MiniGun : Weapon
    {

        public MiniGun(int durability)
        {
            this.durability = durability;
        }

        public override void Aim()
        {
            throw new System.NotImplementedException();
        }
    }
}