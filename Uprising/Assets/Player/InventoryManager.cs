using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Uprising.Items;
using UnityEngine.UI;

namespace Uprising.Players
{

    [RequireComponent(typeof(PlayerControl))]
    public class InventoryManager : MonoBehaviour
    {
        public Item[] items; // 0: Primary Weapon, 1: Secondary Weapon, 2: Bonus 1, 3: Bonus 2
        public int selectedItem;
        private List<Item> appliedEffects;
        private PlayerControl playerControl;

        // Start is called before the first frame update
        void Start()
        {
            appliedEffects = new List<Item>();
            items = new Item[4];
            playerControl = GetComponent<PlayerControl>();

            // Give the default weapon
            Item defaultGun = new DefaultGun(999, 100, 10, 20, this.gameObject);
            GiveItem(defaultGun);
            SelectItem(0);
        }

        void FixedUpdate()
        {
            // Update all bonuses timer
            foreach (Item effect in appliedEffects.ToList())
            {
                (effect as Effect).Update();
            }

            if(items[selectedItem] is Weapon)
            {
                items[selectedItem].Reload();
            }
        }

        public int GetSelectedItem()
        {
            return selectedItem;
        }

        // Inventory Management
        public void GiveItem(Item item)
        {
            Debug.Log("Give item : " + item.type);
            // Add item to inventory
            int index = 0;
            if (item is Weapon)
            {
                if(item.type == ItemType.DefaultGun)
                {
                    if (items[0] != null)
                        ClearItem(items[0]); // Clear a slot
                    items[0] = item;
                    playerControl.hudWeapon1.transform.Find(item.type.ToString()).gameObject.SetActive(true);
                    index = 0;
                }
                else
                {
                    if (items[1] != null)
                        ClearItem(items[1]); // Clear a slot
                    items[1] = item;
                    playerControl.hudWeapon2.transform.Find(item.type.ToString()).gameObject.SetActive(true);
                    index = 1;
                }

            }
            else
            {
                if(items[3] != null && selectedItem != 3)
                {
                    items[2] = item;
                    playerControl.hudBonus1.transform.Find(item.type.ToString()).gameObject.SetActive(true);
                    index = 2;
                }
                else
                {
                    items[3] = item;
                    playerControl.hudBonus2.transform.Find(item.type.ToString()).gameObject.SetActive(true);
                    index = 3;
                }
            }
            item.player = playerControl.gameObject;
            if (index == selectedItem) SelectItem(index);
        }

        public void SelectItem(int index)
        {
            if (playerControl.aim)
            {
                playerControl.aim = false;
                playerControl.toggleaim();
            }
            // Limit selecting range
            if (index < 0) index = 3;
            if (index > 3) index = 0;

            Debug.Log("Select item : " + items[index]);

            // Unselect previous item, then select new one
            if (items[selectedItem] != null)
            {
                items[selectedItem].Unselect();
            }
            ChangeSlotColor(selectedItem, Color.white);

            selectedItem = index;
            ChangeSlotColor(selectedItem, Color.blue);

            if (items[index] != null) items[index].Select();
            Debug.Log(items[index] != null);
        }

        public void ChangeSlotColor(int slot, Color color)
        {
            switch (slot)
            {
                case 0:
                    playerControl.hudWeapon1.GetComponent<RawImage>().color = color;
                    break;
                case 1:
                    playerControl.hudWeapon2.GetComponent<RawImage>().color = color;
                    break;
                case 2:
                    playerControl.hudBonus1.GetComponent<RawImage>().color = color;
                    break;
                case 3:
                    playerControl.hudBonus2.GetComponent<RawImage>().color = color;
                    break;
            }
        }

        public void UseSelectedItem()
        {
            if(items[selectedItem] != null)
                items[selectedItem].Use();
        }

        public void ApplyEffect(Item effectToApply)
        {
            Debug.Log("Apply Effect : " + effectToApply.type);
            Item applied = appliedEffects.Find(x => x.type == effectToApply.type);
            if (applied == null) // If the effect is not already applied
            {
                appliedEffects.Add(effectToApply);
                switch (effectToApply.type)
                {
                    case ItemType.SpeedBoost:
                        playerControl.ModifySpeed(5);
                        break;
                    case ItemType.Invisibility:
                        this.playerControl.photonView.RPC("ToggleInvisibility", RpcTarget.Others);
                        break;
                    case ItemType.JumpBoost:
                        this.playerControl.ModifyJumpHeight(900);
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
                    case ItemType.Invisibility:
                        this.playerControl.photonView.RPC("ToggleInvisibility", RpcTarget.Others);
                        break;
                    case ItemType.JumpBoost:
                        this.playerControl.ModifyJumpHeight(-900);
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
                    if (i == 1) playerControl.hudWeapon2.transform.Find(item.type.ToString()).gameObject.SetActive(false);
                    if (i == 2) playerControl.hudBonus1.transform.Find(item.type.ToString()).gameObject.SetActive(false);
                    if (i == 3) playerControl.hudBonus2.transform.Find(item.type.ToString()).gameObject.SetActive(false);
                    if (i == selectedItem) items[i].Unselect();
                    items[i] = null;
                    return;
                }
            }
        }
    }
}