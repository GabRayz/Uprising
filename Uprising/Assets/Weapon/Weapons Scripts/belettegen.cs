using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Uprising.Items;

public class belettegen : MonoBehaviour
{
    public GameObject belette;
    public Item item;

    public void shoot(int durability,Vector3 direction)
    {
        // GameObject NewBelette = Instantiate(belette, this.gameObject.transform.position, this.gameObject.transform.rotation);
        GameObject NewBelette = PhotonNetwork.Instantiate("belette_" + item.type.ToString(), gameObject.transform.position, gameObject.transform.rotation);
        NewBelette.GetComponent<Rigidbody>().AddForce((this.gameObject.transform.forward + direction) * 10000);
    }
}
