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
    public GameObject heldItemPrefab;

    // Use this for initialization
    void Start () {
        // TODO
        spot = GetSpot();

        collectible = GetComponent<Rigidbody>();
        rotation = new Vector3(60, 60, 60);

        switch (type)
        {
            case ItemType.SpeedBoost:
                this.item = new SpeedBoost(2000, null);
                break;
            case ItemType.DefaultGun:
                this.item = new DefaultGun(100,100,1000,100,1000);
                break;
            default:
                Debug.LogError("This item type is not related to a class");
                break;
        }
    }

    private GameObject GetSpot()
    {
        try
        {
            GameObject spotGet = this.transform.parent.gameObject.transform.parent.gameObject;
            return spotGet;
        }catch(Exception e) {
            Debug.LogError(e);
            return null;
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
}
