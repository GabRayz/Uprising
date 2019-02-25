using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

namespace UnityStandardAssets.Characters.ThirdPerson
{

    [RequireComponent(typeof(PlayerController))]
    public class InventoryManager : MonoBehaviour
    {
        private ItemController.Item Weapon1;
        private ItemController.Item Weapon2;
        private ItemController.Item Bonus1 = null;
        private ItemController.Item Bonus2;
        private ItemController.Item[] items; // 0: Primary Weapon, 1: Secondary Weapon, 2: Bonus 1, 3: Bonus 2
        private int selectedItem;
        private List<ItemController.Item> appliedEffects;
        private ThirdPersonUserControl UserControl;

        // Start is called before the first frame update
        void Start()
        {
            appliedEffects = new List<ItemController.Item>();
            items = new ItemController.Item[4];
            UserControl = GetComponent<ThirdPersonUserControl>();
        }

        void FixedUpdate()
        {
            if (true || UserControl.photonView.IsMine)
            {
                // GetInput();
            }

            // Update all bonuses timer
            foreach (ItemController.Item effect in appliedEffects.ToList())
            {
                (effect as ItemController.Effect).Update();
            }
        }

        // Inventory Management
        public void GiveItem(ItemController.Item item)
        {
            // Add item to inventory
            if (item is ItemController.Weapon)
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

        public void ApplyEffect(ItemController.Item effectToApply)
        {
            ItemController.Item applied = appliedEffects.Find(x => x.type == effectToApply.type);
            if (applied == null)
            {
                appliedEffects.Add(effectToApply);
                switch (effectToApply.type)
                {
                    case ItemType.SpeedBoost:
                        UserControl.ModifySpeed(1);
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

        public void UnApplyEffect(ItemController.Item effectToDisable)
        {
            ItemController.Item applied = appliedEffects.Find(x => x.type == effectToDisable.type);
            if (applied != null)
            {
                appliedEffects.Remove(applied);
                switch (effectToDisable.type)
                {
                    case ItemType.SpeedBoost:
                        UserControl.ModifySpeed(-1);
                        break;
                    default:
                        break;
                }
            }
        }

        public void ClearItem(ItemController.Item item)
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