using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Items;

public class Belette : MonoBehaviour
{
    public float distance;
    public float range;

    public GameObject belette;
    public GameObject player;
    public Weapon weapon;

    public void InitBelette(float range, GameObject belette, Weapon weapon)
    {
        this.weapon = weapon;
        this.range = range;

        //GameObject NewBelette = Instantiate(belette, weapon.gameObject.transform);
        //NewBelette.GetComponent<Rigidbody>().AddForce(weapon.gameObject.transform.forward * 100);

        distance = 0f;
        Debug.Log("Distance 0");
    }

    // Update is called once per frame
    void Update()
    {
        distance++;
        if (distance > range)
        {
            Destroy(belette);
        }
        Debug.Log("Disance" + distance);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "player")
        {
            Debug.Log("touché");
            other.SendMessage("Hit", this); //We also need to send the direction and the force for the propel.
            Destroy(this);
        }
    }
}
