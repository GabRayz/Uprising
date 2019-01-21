using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Item controller.
/// Declares classes for collected item, currently in tha player's inventory.
/// </summary>

public class ItemController : MonoBehaviour
{
    private class Item
    {
        private int durability;

        public int GetDurability()
        {
            return durability;
        }
    }

    private interface IBonus
    {

    }
}
