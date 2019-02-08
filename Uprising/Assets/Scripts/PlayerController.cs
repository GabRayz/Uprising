using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : SessionManager
{
    // This script is to be attached to the player.
    public Rigidbody rb;
    private GameObject model;
    public float speed = 2;
    public float speedModifier = 1;
    public GameObject head;
    public PhotonView photonView;

    // Inventory slots, to be modified
    private ItemController.Item Weapon1;
    private ItemController.Item Weapon2;
    private ItemController.Item Bonus1 = null;
    private ItemController.Item Bonus2;
    private ItemController.Item[] items; // 0: Primary Weapon, 1: Secondary Weapon, 2: Bonus 1, 3: Bonus 2
    private int selectedItem;
    private List<ItemController.Item> appliedEffects;

    public float GetSpeed()
    {
        return speed * speedModifier;
    }

    // Use this for initialization
    void Start()
    {
        appliedEffects = new List<ItemController.Item>();
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        model = rb.gameObject;
        speed = 2;
        speedModifier = 1;
        items = new ItemController.Item[4];
        SelectItem(0);
    }

    void FixedUpdate()
    {
        if(true || photonView.IsMine)
        {
            GetInput();
        }

        // Update all bonuses timer
        foreach(ItemController.Item effect in appliedEffects.ToList())
        {
            (effect as ItemController.Effect).Update();
        }
    }

    // Inventory Management
    public void GiveItem(ItemController.Item item)
    {
        // Add item to inventory
        if(item is ItemController.Weapon)
        {
            if (items[0] == null) items[0] = item;
            else if (items[1] == null) items[1] = item;
            else items[(selectedItem > 1) ? 0 : selectedItem] = item;
        }
        else
        {
            if (items[2] == null) items[2] = item;
            else if (items[3] == null) items[3] = item;
            else items[(selectedItem < 2) ? 2 : selectedItem] = item;
        }
        item.player = this.gameObject;

        // To delete
        item.Use(); // This line is for testing
    }

    public void SelectItem(int index)
    {
        if (index < 0) index = 3;
        if (index > 3) index = 0;
        selectedItem = index;
        Debug.Log("Select item " + index);
        if (items[index] == null) return;
        items[selectedItem].Unselect();
        items[index].Select();
    }

    public void UseSelectedItem()
    {
        items[selectedItem].Use();
    }

    private void GetInput()
    {
        if (Input.GetButtonDown("Select 1")) SelectItem(0);
        if (Input.GetButtonDown("Select 2")) SelectItem(1);
        if (Input.GetButtonDown("Select 3")) SelectItem(2);
        if (Input.GetButtonDown("Select 4")) SelectItem(3);
        //if (inputManager.GetButtonDown("select1")) SelectItem(0);
        //if (inputManager.GetButtonDown("select2")) SelectItem(1);
        //if (inputManager.GetButtonDown("select3")) SelectItem(2);
        //if (inputManager.GetButtonDown("select4")) SelectItem(3);

        if (Input.GetAxis("Mouse ScrollWheel") > 0) SelectItem((selectedItem + 1) % 4);
        if (Input.GetAxis("Mouse ScrollWheel") < 0) SelectItem((selectedItem - 1));
        if(Input.inputString != "") Debug.Log(Input.inputString);

        // Player's movement, to be improved
        if (Input.GetKey(KeyCode.Z))
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

        if (Input.GetKey(KeyCode.A))
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

    public void ApplyEffect(ItemController.Item effectToApply)
    {
        ItemController.Item applied = appliedEffects.Find(x => x.type == effectToApply.type);
        if(applied == null)
        {
            appliedEffects.Add(effectToApply);
            switch(effectToApply.type)
            {
                case ItemType.SpeedBoost:
                    ModifySpeed(1);
                    break;
                default:
                    break;
            }
        }
        else
        {
            applied.durability += effectToApply.durability;
            effectToApply = null;
        }
    }

    public void UnApplyEffect(ItemController.Item effectToDisable)
    {
        ItemController.Item applied = appliedEffects.Find(x => x.type == effectToDisable.type);
        if(applied != null)
        {
            appliedEffects.Remove(applied);
            switch (effectToDisable.type)
            {
                case ItemType.SpeedBoost:
                    ModifySpeed(-1);
                    break;
                default:
                    break;
            }
        }
    }

    public void ClearItem(ItemController.Item item)
    {
        for(var i = 0; i < items.Length; i++)
        {
            if(items[i] == item)
            {
                if (i == selectedItem) items[i].Unselect();
                items[i] = null;
                return;
            }
        }
    }
}