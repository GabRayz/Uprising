using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Items;
using Uprising.Players;

public class CollectibleController : MonoBehaviour {

    //private Collectible item;
    private Vector3 rotation;
    public ItemType type;
    public Item item = null;
    GameObject spot = null;
    public GameObject model;
    public Rigidbody modelRB;

    // Use this for initialization
    void Start () {

        switch (type)
        {
            case ItemType.SpeedBoost:
                this.item = new SpeedBoost(5000, null);
                break;
            case ItemType.ShotGun:
                this.item = new ShotGun(100, 100, 100, 1000, null);
                break;
            case ItemType.DefaultGun:
                this.item = new DefaultGun(100,100,100,1000, null);
                break;
            case ItemType.Invisibility:
                this.item = new Invisibility(10000, null);
                break;
            case ItemType.Dash:
                this.item = new Dash(5, null);
                break;
            case ItemType.JumpBoost:
                this.item = new JumpBoost(10000, null);
                break;
            case ItemType.BearTrap:
                this.item = new BearTrap(null);
                break;
            default:
                Debug.LogError("This item type is not related to a class");
                break;
        }

        if(!(item is Effect))
        {
            rotation = new Vector3(0, 60, 0);
        }
        else
        {
            rotation = new Vector3(60, 60, 60);
        }
        modelRB = model.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Quaternion deltaRot = Quaternion.Euler(rotation * Time.deltaTime);
        modelRB.MoveRotation(modelRB.rotation * deltaRot);
    }

    public void Collect(GameObject player)
    {
        gameObject.SetActive(false);

        if (item != null)
        {
            if (spot != null)
            {
                spot.SendMessage("PickUp");
            }
            player.SendMessage("GiveItem", this.item);
            Destroy(this.gameObject);
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
