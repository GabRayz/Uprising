using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour {

    private class Collectible
    {
        public Item type;
        public string name;
        public int durability;

        public Collectible(Item itemType, string itemName, int durability)
        {
            this.type = itemType;
            this.name = itemName;
            this.durability = durability;
        }
    }

    public enum Item
    {
        Weapon,
        Effect,
        Tool
    }

    private Collectible item;
    public Rigidbody collectible;
    private Vector3 rotation;

    // Use this for initialization
    void Start () {
        collectible = GetComponent<Rigidbody>();
        rotation = new Vector3(60, 60, 60);
	}
	
    private void FixedUpdate()
    {
        Quaternion deltaRot = Quaternion.Euler(rotation * Time.deltaTime);
        collectible.MoveRotation(collectible.rotation * deltaRot);
    }

    public void Init(Item itemType, string itemName, int durability)
    {
        item = new Collectible(itemType, itemName, durability);
    }

    public void Collect(GameObject player)
    {
        collectible.gameObject.SetActive(false);
        player.SendMessage("GiveItem", new Collectible(item.type, item.name, item.durability));
    }
}
