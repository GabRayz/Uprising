using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // This script is to be attached to the player.
    public Rigidbody rb;
    private GameObject model;
    public float speed = 2;
    public float speedModifier = 1;

    // Inventory slots
    private ItemController.Item Weapon1;
    private ItemController.Item Weapon2;
    private ItemController.Item Bonus1 = null;
    private ItemController.Item Bonus2;

    public float GetSpeed()
    {
        return speed * speedModifier;
    }
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        model = rb.gameObject;
        speed = 2;
        speedModifier = 1;
    }

    void FixedUpdate()
    {
        // Player's movement, to be improved
        if(Input.GetKey(KeyCode.Z))
        {
            this.transform.position += this.transform.forward * GetSpeed() * Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += -1 * transform.forward * (GetSpeed() / 2) * Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            this.transform.position += -1 * transform.right * (GetSpeed() / 2) * Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += transform.right * (GetSpeed() / 2) * Time.fixedDeltaTime;
        }

        if(Input.GetKey(KeyCode.A))
        {
            Quaternion deltaRot = Quaternion.Euler(new Vector3(0, 50, 0) * Time.deltaTime * GetSpeed());
            rb.MoveRotation(deltaRot * rb.rotation);
        }

        if (Input.GetKey(KeyCode.E))
        {
            Quaternion deltaRot = Quaternion.Euler(new Vector3(0, -50, 0) * Time.deltaTime * GetSpeed());
            rb.MoveRotation(deltaRot * rb.rotation);
        }

        // Update all bonuses timer
        if(Bonus1 != null)
        {
            (Bonus1 as ItemController.Effect).Update();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("item"))
        {
            other.gameObject.SendMessage("Collect", rb.gameObject);
        }
    }

    public void ModifySpeed(float modifier)
    {
        speedModifier += modifier;
    }

    public void GiveItem(ItemController.Item item)
    {
        // Add item to inventory
        this.Bonus1 = item;
        item.player = this.gameObject;
        item.Use();
    }

    public void ClearItem(ItemController.Item item)
    {
        // TODO
    }
}