using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    DefaultGun,
    SpeedBoost,
    JumpBoost,
    Shield,
    ForceField,
    Invisibility,
    Minigun,
    Rifle,
    AssaultRifle,
    Sniper,
    MachineGun,
    RocketLauncher
}

public class CollectibleController : MonoBehaviour {

    //private Collectible item;
    public Rigidbody collectible;
    private Vector3 rotation;
    public ItemType type;
    public ItemController.Item item;

    //public CollectibleController(ItemType type, int durability)
    //{
    //    switch (type)
    //    {
    //        case ItemType.SpeedBoost:
    //            item = new ItemController.SpeedBoost(durability, null);
    //            break;
    //    }
    //}

    // Use this for initialization
    void Start () {
        collectible = GetComponent<Rigidbody>();
        rotation = new Vector3(60, 60, 60);

        switch (type)
        {
            case ItemType.SpeedBoost:
                item = new ItemController.SpeedBoost(3000, null);
                Debug.Log(item);
                break;
            default:
                Debug.LogError("This item type is not related to a class");
                break;
        }
    }
	
    private void FixedUpdate()
    {
        Quaternion deltaRot = Quaternion.Euler(rotation * Time.deltaTime);
        collectible.MoveRotation(collectible.rotation * deltaRot);
    }

    public void Collect(GameObject player)
    {
        Debug.Log("Collecting");
        collectible.gameObject.SetActive(false);
        player.SendMessage("GiveItem", item);
    }



}
