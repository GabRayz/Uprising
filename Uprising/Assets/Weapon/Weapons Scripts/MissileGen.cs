using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Items;
using Photon.Pun;

public class MissileGen : MonoBehaviour
{
    public GameObject missile;
    public GameObject missilePrefab;
    public GameObject player;
    public Item item;
    public AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    public void Shoot(Item launcher)
    {
        Vector3 direction = launcher.target.transform.forward;
        if (PhotonNetwork.IsConnected)
        {
            missile = PhotonNetwork.Instantiate("Missile", this.transform.position + transform.forward, this.transform.rotation);
            missile.transform.rotation = launcher.target.transform.rotation;
        }
        else
        {
            missile = Instantiate(missilePrefab, transform.position + transform.forward, transform.rotation);
            missile.transform.rotation = launcher.target.transform.rotation;
        }
    }
}
