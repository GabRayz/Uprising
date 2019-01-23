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
    public GameObject head;

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

        float Vrot = Input.GetAxis("Vertical") * 10;
        float Hrot = Input.GetAxis("Horizontal") * 10;
        Hrot *= Time.deltaTime;
        Vrot *= Time.deltaTime;

        // Right / Left robot's rotation
        this.transform.Rotate(transform.up * Hrot);
        // Up / Down Head's rotation

        head.transform.rotation.Set(head.transform.rotation.x + Vrot, head.transform.rotation.y, head.transform.rotation.z, head.transform.rotation.w);
        // head.transform.Rotate(head.transform.rotation.x);
        // transform.rotation += this.transform.right;

        // Update all bonuses timer
        if (Bonus1 != null)
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
        item.Use(); // This line is for testing
    }

    public void ClearItem(ItemController.Item item)
    {
        // TODO
    }
}