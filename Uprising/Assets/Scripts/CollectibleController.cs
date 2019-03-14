using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Items;
using Uprising.Players;

public class CollectibleController : MonoBehaviour {

    //private Collectible item;
    public Rigidbody collectible;
    private Vector3 rotation;
    public ItemType type;
    public Item item = null;
    GameObject spot = null;

    // Use this for initialization
    void Start () {
        collectible = GetComponent<Rigidbody>();

        switch (type)
        {
            case ItemType.SpeedBoost:
                this.item = new SpeedBoost(2000, null);
                break;
            case ItemType.DefaultGun:
                this.item = new DefaultGun(100,100,1000,100,1000, null);
                break;
            case ItemType.Invisibility:
                this.item = new Invisibility(2000, null);
                break;
            default:
                Debug.LogError("This item type is not related to a class");
                break;
        }

        if(item is Weapon)
        {
            rotation = new Vector3(0, 60, 0);
        }
        else
        {
            rotation = new Vector3(60, 60, 60);
        }
    }

    private void FixedUpdate()
    {
        Quaternion deltaRot = Quaternion.Euler(rotation * Time.deltaTime);
        collectible.MoveRotation(collectible.rotation * deltaRot);
    }

    public void Collect(GameObject player)
    {
        collectible.gameObject.SetActive(false);

        if (item != null)
        {
            if (spot != null)
            {
                spot.SendMessage("PickUp");
            }
            player.SendMessage("GiveItem", this.item);
            Destroy(this.transform.parent.gameObject);
        }
        else
        {
            Debug.LogError("Item is not defined for this CollectibleController");
        }
    }

    public void SetSpot(GameObject spot)
    {
        this.spot = spot;
    }
}
