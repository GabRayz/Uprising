using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class belettegen : MonoBehaviour
{
    public GameObject belette;
    public void shoot(float range, int durability)
    {
        GameObject NewBelette = Instantiate(belette, this.gameObject.transform.position, this.gameObject.transform.rotation);
        NewBelette.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * 100);
    }
}
