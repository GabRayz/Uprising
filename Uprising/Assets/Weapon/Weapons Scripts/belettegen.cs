using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Uprising.Items;
using Uprising.Players;

public class belettegen : MonoBehaviour
{
    public GameObject belette;
    public GameObject player;
    public Item item;
    public AudioSource sound;

    private void Start()
    {
        this.sound = GetComponent<AudioSource>();
    }

    public void shoot(int durability, Vector3 direction, Item item)
    {
        player.GetComponent<AudioManager>().PlaySound(item.type.ToString());

        this.item = item;
        direction = direction * 2;
        GameObject NewBelette;
        Vector3 dir = item.target.transform.position - transform.position;
        if(PhotonNetwork.IsConnected)
        {
            NewBelette = PhotonNetwork.Instantiate("belette_" + item.type.ToString(), gameObject.transform.position + gameObject.transform.forward, gameObject.transform.rotation);
            NewBelette.GetComponent<Belette>().photonView.RPC("InitBelette", RpcTarget.All, (item as Weapon).knockback);
        }
        else
        {
            NewBelette = Instantiate(belette, this.gameObject.transform.position + gameObject.transform.forward, this.gameObject.transform.rotation);
            NewBelette.GetComponent<Belette>().InitBelette((item as Weapon).knockback);
        }
        NewBelette.GetComponent<Rigidbody>().AddForce((dir.normalized) * 1000);
    }
}
