using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq.Expressions;

public class ItemSpotController : MonoBehaviour
{
    public GameObject SpeedBoostPrefab;
    public int averageCoolDown;
    public Dictionary<ItemType, int> itemRaretyPairs;
    private int cooldown;

    private bool isPickedUp;

    // Start is called before the first frame update
    void Start()
    {
        this.itemRaretyPairs = InitItemRaretyPairs();
        CreateNewItem(ChooseItem());
        cooldown = averageCoolDown;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPickedUp)
        {
            cooldown -= (int)(Time.deltaTime * 1000);
            if (cooldown <= 0)
            {
                CreateNewItem(ChooseItem());
                cooldown = averageCoolDown;
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
        //itemRaretyPairs.Add(ItemType.Drugs, GetRaretyInt(Rarety.Common, y));
        //itemRaretyPairs.Add(ItemType.Grapnel, GetRaretyInt(Rarety.Common, y));
        //itemRaretyPairs.Add(ItemType.RocketLauncher, GetRaretyInt(Rarety.Common, y));
        //itemRaretyPairs.Add(ItemType.Shield, GetRaretyInt(Rarety.Common, y));
        //itemRaretyPairs.Add(ItemType.Rifle, GetRaretyInt(Rarety.Common, y));
        //itemRaretyPairs.Add(ItemType.BearTrap, GetRaretyInt(Rarety.Common, y));
        //itemRaretyPairs.Add(ItemType.SlimeGun, GetRaretyInt(Rarety.Common, y));

        //itemRaretyPairs.Add(ItemType.JumpBoost, GetRaretyInt(Rarety.Rare, y));
        //itemRaretyPairs.Add(ItemType.ForceField, GetRaretyInt(Rarety.Rare, y));
        //itemRaretyPairs.Add(ItemType.PortalGun, GetRaretyInt(Rarety.Rare, y));
        //itemRaretyPairs.Add(ItemType.ShotGun, GetRaretyInt(Rarety.Rare, y));
        //itemRaretyPairs.Add(ItemType.MachineGun, GetRaretyInt(Rarety.Rare, y));
        //itemRaretyPairs.Add(ItemType.Mine, GetRaretyInt(Rarety.Rare, y));

        //itemRaretyPairs.Add(ItemType.DoubleJump, GetRaretyInt(Rarety.Special, y));
        //itemRaretyPairs.Add(ItemType.Invisibility, GetRaretyInt(Rarety.Special, y));
        //itemRaretyPairs.Add(ItemType.GuidedMissile, GetRaretyInt(Rarety.Special, y));
        //itemRaretyPairs.Add(ItemType.AssaultRifle, GetRaretyInt(Rarety.Special, y));
        //itemRaretyPairs.Add(ItemType.Blackout, GetRaretyInt(Rarety.Special, y));



        return itemRaretyPairs;
    }

    private void CreateNewItem(ItemType type)
    {
        ItemController.Item newItem = null;
        switch (type)
        {
            case ItemType.SpeedBoost:
                newItem = new ItemController.SpeedBoost(30000, null);
                Instantiate(SpeedBoostPrefab, this.transform, false);
                break;
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
