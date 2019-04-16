using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Uprising.Items;

public class belettegen : MonoBehaviour
{
    public GameObject belette;
    public Item item;

    public void shoot(int durability,Vector3 direction, Item item)
    {
        this.item = item;
        GameObject NewBelette;
        if(PhotonNetwork.IsConnected)
            NewBelette = PhotonNetwork.Instantiate("belette_" + item.type.ToString(), gameObject.transform.position, gameObject.transform.rotation);
        else
            NewBelette = Instantiate(belette, this.gameObject.transform.position, this.gameObject.transform.rotation);
        NewBelette.GetComponent<Belette>().InitBelette(item as Weapon);
        NewBelette.GetComponent<Rigidbody>().AddForce((this.gameObject.transform.forward + direction) * 1000);
    }
}
