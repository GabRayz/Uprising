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
        public GameObject hud;
        public HUD hudControl;
        public GameObject hudWeapon1;
        public GameObject hudWeapon2;
        public GameObject hudBonus1;
        public GameObject hudBonus2;

        // Start is called before the first frame update
        void Start()
        {
            appliedEffects = new List<Item>();
            items = new Item[4];
            playerControl = GetComponent<PlayerControl>();

            if (playerControl.debugMode || playerControl.photonView.IsMine)
            {
                hud = Instantiate(hud);
                hudControl = hud.GetComponent<HUD>();
                hudWeapon1 = hud.transform.Find("Canvas").Find("HUD right").Find("Slot1 Weapon").gameObject;
                hudWeapon2 = hud.transform.Find("Canvas").Find("HUD right").Find("Slot2 Weapon").gameObject;
                hudBonus1 = hud.transform.Find("Canvas").Find("HUD right").Find("Slot3 Item").gameObject;
                hudBonus2 = hud.transform.Find("Canvas").Find("HUD right").Find("Slot4 Item").gameObject;
                SelectItem(0);
            }

            if(!playerControl.debugMode && playerControl.photonView.IsMine)
            {
                hud.GetComponent<HUD>().ChangeRemain(playerControl.gameManager.playersCount);
            }
        }

        void FixedUpdate()
        {
            // Update all bonuses timer
            foreach (Item effect in appliedEffects.ToList())
            {
                (effect as Effect).Update();
                hudControl.ChangeDurability(effect.type.ToString(), (int)(effect.durability/1000));
            }

            if(items[selectedItem] is Weapon)
            {
                items[selectedItem].Reload();
            }
        }

        public int GetSelectedItemIndex()
        {
            return selectedItem;
        }

        public Item GetSelectedItem()
        {
            return items[selectedItem];
        }

        // Inventory Management
        public void GiveItem(Item item)
        {

            // Add item to inventory
            int index = 0;
            if (item is Weapon)
            {
                if(item.type == ItemType.DefaultGun)
                {
                    if (items[0] != null)
                        ClearItem(items[0]); // Clear a slot
                    items[0] = item;
                    if(playerControl.debugMode || playerControl.photonView.IsMine)
                        hudWeapon1.transform.Find(item.type.ToString()).gameObject.SetActive(true);
                    index = 0;
                }
                else
                {
                    if (items[1] != null)
                        ClearItem(items[1]); // Clear a slot
                    items[1] = item;
                    if (playerControl.debugMode || playerControl.photonView.IsMine)
                        hudWeapon2.transform.Find(item.type.ToString()).gameObject.SetActive(true);
                    index = 1;
                }

            }
            else
            {
                if((selectedItem == 2 && items[3] != null) || items[2] == null)
                {
                    if (items[2] != null)
                        ClearItem(items[2]);
                    items[2] = item;
                    if (playerControl.debugMode || playerControl.photonView.IsMine)
                        hudBonus1.transform.Find(item.type.ToString()).gameObject.SetActive(true);
                    index = 2;
                }
                else
                {
                    if (items[3] != null)
                        ClearItem(items[3]);
                    items[3] = item;
                    if (playerControl.debugMode || playerControl.photonView.IsMine)
                        hudBonus2.transform.Find(item.type.ToString()).gameObject.SetActive(true);
                    index = 3;
                }
            }
            item.player = playerControl.gameObject;
            item.SetPlayer(gameObject);
            if (index == selectedItem) SelectItem(index);
            if (items[index] != null && index == 1 && (playerControl.debugMode || playerControl.photonView.IsMine))
                hud.GetComponent<HUD>().ChangeAmmo(items[index].durability);
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


            // Unselect previous item, then select new one
            if (items[selectedItem] != null)
            {
                items[selectedItem].Unselect();
            }
            if (playerControl.debugMode || playerControl.photonView.IsMine)
                ChangeSlotColor(selectedItem, Color.white);

            selectedItem = index;
            if (playerControl.debugMode || playerControl.photonView.IsMine)
                ChangeSlotColor(selectedItem, Color.blue);

            if (items[index] != null) items[index].Select();
            if (items[index] != null) GetComponent<AudioManager>().PlaySound("Draw");
        }

        public void ChangeSlotColor(int slot, Color color)
        {
            switch (slot)
            {
                case 0:
                    hudWeapon1.GetComponent<RawImage>().color = color;
                    break;
                case 1:
                    hudWeapon2.GetComponent<RawImage>().color = color;
                    break;
                case 2:
                    hudBonus1.GetComponent<RawImage>().color = color;
                    break;
                case 3:
                    hudBonus2.GetComponent<RawImage>().color = color;
                    break;
            }
        }

        public void UseSelectedItem()
        {

            if(items[selectedItem] != null && selectedItem == 1)
            {
                items[selectedItem].Use();
                if (playerControl.debugMode || playerControl.photonView.IsMine)
                {
                    if(items[selectedItem] != null)
                        hud.GetComponent<HUD>().ChangeAmmo(items[selectedItem].durability);
                    else
                        hud.GetComponent<HUD>().ChangeAmmo(0);
                }

            }
            else if(items[selectedItem] != null)
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
                        this.playerControl.ModifyJumpHeight(500);
                        break;
                    case ItemType.Drugs:
                        playerControl.firerateModifier += .5f;
                        break;
                    default:
                        break;
                }

                hudControl.DisplayBonus(effectToApply.type.ToString(), (int)(effectToApply.durability/1000));
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
                        this.playerControl.ModifyJumpHeight(-500);
                        break;
                    case ItemType.Drugs:
                        playerControl.firerateModifier -= .5f;
                        break;
                    default:
                        break;
                }
                hudControl.HideBonus(effectToDisable.type.ToString());
            }
        }

        public void ClearItem(Item item)
        {
            for (var i = 0; i < items.Length; i++)
            {
                if (items[i] == item)
                {
                    if (playerControl.aim)
                        playerControl.toggleaim();
                    if (i == 1 && (playerControl.debugMode || playerControl.photonView.IsMine)) hudWeapon2.transform.Find(item.type.ToString()).gameObject.SetActive(false);
                    if (i == 2 && (playerControl.debugMode || playerControl.photonView.IsMine)) hudBonus1.transform.Find(item.type.ToString()).gameObject.SetActive(false);
                    if (i == 3 && (playerControl.debugMode || playerControl.photonView.IsMine)) hudBonus2.transform.Find(item.type.ToString()).gameObject.SetActive(false);
                    if (i == selectedItem) items[i].Unselect();
                    items[i] = null;
                    return;
                }
            }
        }
    }
}