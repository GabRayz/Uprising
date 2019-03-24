using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class belettegen : MonoBehaviour
{
    public GameObject belette;
    public void shoot(int durability,Vector3 direction)
    {
        GameObject NewBelette = Instantiate(belette, this.gameObject.transform.position, this.gameObject.transform.rotation);
        NewBelette.GetComponent<Rigidbody>().AddForce((this.gameObject.transform.forward + direction) * 10000);
    }
}
