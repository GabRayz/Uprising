using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour {

    //private Collectible item;
    public Rigidbody collectible;
    private Vector3 rotation;
    public string name;
    public ItemController.Item item;

    public CollectibleController(string itemName, int durability)
    {
        switch (itemName)
        {
            case "SpeedBoost":
                item = new ItemController.SpeedBoost(durability, null);
                break;
        }

        this.name = itemName;
    }

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

    public void Collect(GameObject player)
    {
        collectible.gameObject.SetActive(false);
        player.SendMessage("GiveItem", item);
    }



}
