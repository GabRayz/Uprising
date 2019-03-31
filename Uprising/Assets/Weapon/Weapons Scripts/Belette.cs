using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Items;

public class Belette : MonoBehaviour
{
    public float distance;
    public float range;
    public float firerate;
    public float fireratetime;

    public GameObject belette;
    public GameObject player;
    public Weapon weapon;

    public void InitBelette(Weapon weapon)
    {
        this.weapon = weapon;
    }

    // Update is called once per frame
    void Update()
    {
        distance++;
        
        if (distance > range)
        {
            Destroy(belette);
        }

        if (fireratetime < firerate)
        {
            fireratetime++;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "player")
        {
            //other.SendMessage("Hit", this); //We also need to send the direction and the force for the propel.
            //Destroy(this);
        }
    }
}
