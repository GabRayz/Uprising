using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpotController : MonoBehaviour
{
    // public int raretyIndex;
    public int averageCoolDown;
    public Dictionary<ItemType, int> itemRaretyPairs;
    // private Game

    private bool isPickedUp;

    // Start is called before the first frame update
    void Start()
    {
        this.itemRaretyPairs = InitItemRaretyPairs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Dictionary<ItemType, int> InitItemRaretyPairs()
    {
        itemRaretyPairs = new Dictionary<ItemType, int>();
        itemRaretyPairs.Add(ItemType.SpeedBoost, (int)Rarety.Common);
        itemRaretyPairs.Add(ItemType.Rifle, (int)Rarety.Common); // Fusil, arme un peu naze
        itemRaretyPairs.Add(ItemType.MachineGun, (int)Rarety.Rare); // Mitraillette, arme bien
        itemRaretyPairs.Add(ItemType.AssaultRifle, (int)Rarety.Special); // Fusil d'assaut, qu'elle est bien cette arme

        int y = (int)this.transform.position.y;
        for(var i = 0; i < itemRaretyPairs.Count; i++) // A foreach doesnt work
        {
            itemRaretyPairs[(ItemType)i] += (itemRaretyPairs[(ItemType)i] / 10) * y;
        }

        return itemRaretyPairs;
    }

    //private void CreateAndPlaceItem()
    //{
    //    ItemController.Item item = CreateNewItem(ChooseItem());

    //}

    private ItemController.Item CreateNewItem(ItemType type)
    {
        ItemController.Item newItem = null;
        switch (type)
        {
            case ItemType.SpeedBoost:
                newItem = new ItemController.SpeedBoost(30000, null);
                break;
        }
        return newItem;
    }

    private ItemType ChooseItem()
    {
        int total = 0;
        foreach(var item in itemRaretyPairs)
        {
            total += item.Value;
        }
        int score = Random.Range(1, total);
        ItemType chosenType = ItemType.DefaultGun; // = null
        int stack = 0;
        foreach(var item in itemRaretyPairs)
        {
            if (score <= item.Value && score > stack - item.Value)
            {
                chosenType = item.Key;
            }
            stack += item.Value;
        }
        return chosenType;
    }
}
