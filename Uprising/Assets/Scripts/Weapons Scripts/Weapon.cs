
using UnityEngine;
using Uprising.Item;

public abstract class Weapon : Item
{
    public GameObject belette;
    public GameObject weapon;

    public float accuracy;
    public float range;
    public float firerate;
    public float knockback;
    public int durability;
    public bool isCurrentlyUsed = false;
    public GameObject player;

    public abstract void Use(); //shoot
    public abstract void Aim(); //Aim
    public abstract void Select(); // Display item, and apply passif effect
    public abstract void Unselect();


    public Weapon()
    {

    }

    void Update()
    {
        if (this.durability <= 0)
        {
            this.StopUsing();
        }

        if(Input.GetKeyDown(KeyCode.Mouse0)) //Left-Click
        {
            Use();
            durability--;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1)) //Right-Click
        {
            Aim();
        }
    }
    
    public int GetDurability()
    {
        return durability;
    }

    protected void StopUsing()
    {
        // Destroy(this);
    }
}
