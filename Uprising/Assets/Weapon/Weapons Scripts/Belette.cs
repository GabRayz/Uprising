using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Players;
using Photon.Pun;

namespace Uprising.Items
{
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
            // this.player = player;
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
            Debug.Log("enter");
            if (other.CompareTag("player") && GetComponent<PhotonView>().IsMine)
            {
                Debug.Log("Hit a player");
                other.GetComponent<PlayerControl>().photonView.RPC("Hit", RpcTarget.All, this);
            }
            if(other.CompareTag("player"))
            {
                Debug.Log("Hit a player");
                other.GetComponent<PlayerControl>().Hit(this);
            }
        }
    }

}
