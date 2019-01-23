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
    // private Game

    private bool isPickedUp;

    // Start is called before the first frame update
    void Start()
    {
        this.itemRaretyPairs = InitItemRaretyPairs();
        CreateNewItem(ChooseItem());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int GetRaretyInt(Rarety rarety, int y)
    {
        return (int)rarety + (int)rarety / 10 * y;
    }

    private Dictionary<ItemType, int> InitItemRaretyPairs()
    {
        int y = (int)this.transform.position.y;

        // Add here all game's items
        itemRaretyPairs = new Dictionary<ItemType, int>();
        itemRaretyPairs.Add(ItemType.SpeedBoost, GetRaretyInt(Rarety.Common, y));
        itemRaretyPairs.Add(ItemType.Rifle, GetRaretyInt(Rarety.Common, y)); // Fusil, arme un peu naze
        itemRaretyPairs.Add(ItemType.MachineGun, GetRaretyInt(Rarety.Rare, y)); // Mitraillette, arme bien
        itemRaretyPairs.Add(ItemType.AssaultRifle, GetRaretyInt(Rarety.Special, y)); // Fusil d'assaut, qu'elle est bien cette arme

        return itemRaretyPairs;
    }

    private void CreateNewItem(ItemType type)
    {
        ItemController.Item newItem = null;
        GameObject newObject = null;
        switch (type)
        {
            case ItemType.SpeedBoost:
                newItem = new ItemController.SpeedBoost(30000, null);
                newObject = Instantiate(SpeedBoostPrefab, this.transform, false);
                //newObject = Instantiate(SpeedBoostPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), this.transform);
                break;
        }
    }

    private ItemType ChooseItem()
    {
        // return ItemType.JumpBoost;
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
}
