using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Items;
using Photon.Pun;

public class MissileGen : MonoBehaviour
{
    public GameObject rocket;
    public GameObject rocketPrefab;
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
            rocket = PhotonNetwork.Instantiate("Rocket", this.transform.position + transform.forward, this.transform.rotation);
            rocket.transform.rotation = launcher.target.transform.rotation;
        }
        else
        {
            rocket = Instantiate(rocketPrefab, transform.position + transform.forward, transform.rotation);
            rocket.transform.rotation = launcher.target.transform.rotation;
        }
    }
}
