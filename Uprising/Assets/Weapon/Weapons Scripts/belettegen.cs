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

    public void shoot(Item item)
    {
        player.GetComponent<AudioManager>().PlaySound(item.type.ToString());
        this.item = item;

        GameObject NewBelette;
        Vector3 dir = GetDirection(item, (item as Weapon).accuracy);

        if(PhotonNetwork.IsConnected && player.GetComponent<PhotonView>().IsMine)
        {
            NewBelette = PhotonNetwork.Instantiate("belette_" + item.type.ToString(), gameObject.transform.position + gameObject.transform.forward, gameObject.transform.rotation);
            NewBelette.GetComponent<Belette>().photonView.RPC("InitBelette", RpcTarget.All, (item as Weapon).knockback);
        }
        else
        {
            NewBelette = Instantiate(belette, this.gameObject.transform.position + gameObject.transform.forward, this.gameObject.transform.rotation);
            NewBelette.GetComponent<Belette>().InitBelette((item as Weapon).knockback);
        }
        NewBelette.GetComponent<Rigidbody>().AddForce(dir * 3000);
    }

    Vector3 GetDirection(Item item, float accuracy)
    {
        Vector3 dir = (item.target.transform.position - transform.position).normalized;
        Vector3 deviation_y = transform.up * (Random.Range(-1f, 1f) / accuracy);
        Vector3 deviation_x = transform.right * (Random.Range(-1f, 1f) / accuracy);
        dir = (dir + deviation_x + deviation_y).normalized;
        return dir;
    }
}
