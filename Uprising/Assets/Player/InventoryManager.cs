using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Uprising.Items;

namespace Uprising.Players
{

    [RequireComponent(typeof(PlayerControl))]
    public class InventoryManager : MonoBehaviour
    {
        private Item Weapon1;
        private Item Weapon2;
        private Item Bonus1 = null;
        private Item Bonus2;
        private Item[] items; // 0: Primary Weapon, 1: Secondary Weapon, 2: Bonus 1, 3: Bonus 2
        private int selectedItem;
        private List<Item> appliedEffects;
        private PlayerControl playerControl;

        // Start is called before the first frame update
        void Start()
        {
            appliedEffects = new List<Item>();
            items = new Item[4];
            playerControl = GetComponent<PlayerControl>();
        }

        void FixedUpdate()
        {
            //if (true || PlayerControl.photonView.IsMine)
            //{
            //    // GetInput();
            //}

            // Update all bonuses timer
            foreach (Item effect in appliedEffects.ToList())
            {
                (effect as Effect).Update();
            }
        }

        public int GetSelectedItem()
        {
            return selectedItem;
        }

        // Inventory Management
        public void GiveItem(Item item)
        {
            // Add item to inventory
            if (item is Weapon)
            {
                if (items[0] == null) items[0] = item;
                else if (items[1] == null) items[1] = item;
                else items[(selectedItem > 1) ? 0 : selectedItem] = item;
            }
            else
            {
                if (items[2] == null) items[2] = item;
                else if (items[3] == null) items[3] = item;
                else items[(selectedItem < 2) ? 2 : selectedItem] = item;
            }
            item.player = this.gameObject;

            // To delete
            item.Use(); // This line is for testing
        }

        public void SelectItem(int index)
        {
            if (index < 0) index = 3;
            if (index > 3) index = 0;
            selectedItem = index;
            Debug.Log("Select item " + index);
            if (items[index] == null) return;
            items[selectedItem].Unselect();
            items[index].Select();
        }

        public void UseSelectedItem()
        {
            items[selectedItem].Use();
        }

        public void ApplyEffect(Item effectToApply)
        {
            Item applied = appliedEffects.Find(x => x.type == effectToApply.type);
            if (applied == null)
            {
                appliedEffects.Add(effectToApply);
                switch (effectToApply.type)
                {
                    case ItemType.SpeedBoost:
                        playerControl.ModifySpeed(5);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                applied.durability += effectToApply.durability;
                effectToApply = null;
            }
        }

        public void UnApplyEffect(Item effectToDisable)
        {
            Item applied = appliedEffects.Find(x => x.type == effectToDisable.type);
            if (applied != null)
            {
                appliedEffects.Remove(applied);
                switch (effectToDisable.type)
                {
                    case ItemType.SpeedBoost:
                        playerControl.ModifySpeed(-5);
                        break;
                    default:
                        break;
                }
            }
        }

        public void ClearItem(Item item)
        {
            for (var i = 0; i < items.Length; i++)
            {
                if (items[i] == item)
                {
                    if (i == selectedItem) items[i].Unselect();
                    items[i] = null;
                    return;
                }
            }
        }
    }
}