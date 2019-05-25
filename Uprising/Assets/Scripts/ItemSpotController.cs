using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq.Expressions;
using Uprising.Items;
using Photon.Pun;

public class ItemSpotController : MonoBehaviour
{
    public int averageCoolDown;
    public Dictionary<ItemType, int> itemRaretyPairs;
    private int cooldown;
    private UnityEngine.Random random;

    private bool isPickedUp = true;

    // Start is called before the first frame update
    void Start()
    {
        random = new UnityEngine.Random();
        this.itemRaretyPairs = InitItemRaretyPairs();
        if (PhotonNetwork.IsMasterClient) // Only the master client instatiates the items
        {
            // CreateNewItem(ChooseItem());
            cooldown = averageCoolDown;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isPickedUp && PhotonNetwork.IsMasterClient)
        {
            cooldown -= (int)(Time.deltaTime * 1000);
            if (cooldown <= 0)
            {
                CreateNewItem(ChooseItem());
                int randomRange = UnityEngine.Random.Range((int)-0.2 * averageCoolDown, (int)0.2 * averageCoolDown);
                cooldown = averageCoolDown + randomRange;
                isPickedUp = false;
            }
        }
    }

    private int GetRaretyInt(Rarety rarety, int y)
    {
        return (int)rarety + (1 / (int)rarety) * y;
    }

    private Dictionary<ItemType, int> InitItemRaretyPairs()
    {
        int y = (int)this.transform.position.y;

        // Add here all game's items
        itemRaretyPairs = new Dictionary<ItemType, int>();
        itemRaretyPairs.Add(ItemType.SpeedBoost, GetRaretyInt(Rarety.Common, y));
        //itemRaretyPairs.Add(ItemType.DefaultGun, GetRaretyInt(Rarety.Common, y)); USELESS
        itemRaretyPairs.Add(ItemType.Minigun, GetRaretyInt(Rarety.Common, y));
        itemRaretyPairs.Add(ItemType.Drugs, GetRaretyInt(Rarety.Common, y));
        itemRaretyPairs.Add(ItemType.Grapnel, GetRaretyInt(Rarety.Common, y));
        itemRaretyPairs.Add(ItemType.RocketLauncher, GetRaretyInt(Rarety.Common, y));
        itemRaretyPairs.Add(ItemType.Shield, GetRaretyInt(Rarety.Common, y));
        itemRaretyPairs.Add(ItemType.Rifle, GetRaretyInt(Rarety.Common, y));
        itemRaretyPairs.Add(ItemType.BearTrap, GetRaretyInt(Rarety.Common, y));
        //itemRaretyPairs.Add(ItemType.SlimeGun, GetRaretyInt(Rarety.Common, y));

        itemRaretyPairs.Add(ItemType.JumpBoost, GetRaretyInt(Rarety.Rare, y));
        itemRaretyPairs.Add(ItemType.ForceField, GetRaretyInt(Rarety.Rare, y));
        //itemRaretyPairs.Add(ItemType.PortalGun, GetRaretyInt(Rarety.Rare, y));
        itemRaretyPairs.Add(ItemType.ShotGun, GetRaretyInt(Rarety.Rare, y));
        itemRaretyPairs.Add(ItemType.MachineGun, GetRaretyInt(Rarety.Rare, y));
        itemRaretyPairs.Add(ItemType.Mine, GetRaretyInt(Rarety.Rare, y));

        itemRaretyPairs.Add(ItemType.Dash, GetRaretyInt(Rarety.Special, y));
        itemRaretyPairs.Add(ItemType.Sniper, GetRaretyInt(Rarety.Special, y));
        itemRaretyPairs.Add(ItemType.Invisibility, GetRaretyInt(Rarety.Special, y));
        //itemRaretyPairs.Add(ItemType.GuidedMissile, GetRaretyInt(Rarety.Special, y));
        itemRaretyPairs.Add(ItemType.AssaultRifle, GetRaretyInt(Rarety.Special, y));
        //itemRaretyPairs.Add(ItemType.Blackout, GetRaretyInt(Rarety.Special, y));



        return itemRaretyPairs;
    }

    private void CreateNewItem(ItemType type)
    {
        GameObject newItem;
        try
        {
            newItem = PhotonNetwork.InstantiateSceneObject(type.ToString(), this.transform.position + this.transform.up/2, this.transform.rotation);

            newItem.SendMessage("SetSpot", this.gameObject);
        }catch(Exception e)
        {
            Debug.LogError(e);
        }

    }

    private ItemType ChooseItem()
    {
        int total = 0; // Get the total value of all rarety int
        foreach(var item in itemRaretyPairs)
        {
            total += item.Value;
        }
        // Choose one
        int score = UnityEngine.Random.Range(1, total);
        ItemType chosenType = ItemType.DefaultGun; // = null
        int stack = 0;
        foreach(var item in itemRaretyPairs)
        {
            if (score <= stack + item.Value && score > stack)
            {
                chosenType = item.Key;
            }
            stack += item.Value;
        }
        return chosenType;
    }

    public void PickUp()
    {
        isPickedUp = true;
    }
}
