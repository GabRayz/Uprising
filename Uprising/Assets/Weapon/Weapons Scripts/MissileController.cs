using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Uprising.Players;
using System.Linq;

public class MissileController : MonoBehaviour
{
    public float maxTime;
    public int speed;
    Rigidbody rb;
    public bool isFlying;
    float time;
    public GameObject explosionEffect;
    public GameManager gamemanager;
    PlayerStats Topplayer;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        isFlying = true;
        rb.AddForce(transform.forward * speed, ForceMode.Force);

        
        //take the highest player
        Topplayer = gamemanager.players.First().Value;
        foreach (var players in gamemanager.players)
        {
            if (players.Value.kills > Topplayer.kills)
                Topplayer = players.Value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlying)
        {
            rb.AddForce((Topplayer.playerControl.gameObject.transform.forward-this.transform.position) * speed, ForceMode.Force);

            time += Time.deltaTime;
            if (time >= maxTime)
                Explode();
        }
    }

    void Explode()
    {
        isFlying = false;
        explosionEffect = Instantiate(explosionEffect, transform.position, transform.rotation);
        StartCoroutine("ExplosionTimer");

        Collider[] colliders = Physics.OverlapSphere(this.transform.position, 8f);
        foreach (Collider c in colliders)
        {
            if (c.CompareTag("player"))
            {
                Rigidbody rbPlayer = c.GetComponent<Rigidbody>();
                rbPlayer.AddExplosionForce(1000f, this.transform.position, 8f);
            }
        }
    }

    IEnumerator ExplosionTimer()
    {
        float delay = 1.5f;
        while (delay > 0)
        {
            delay -= Time.deltaTime;
            yield return null;
        }
        Destroy(explosionEffect);
        Destroy(gameObject);
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Destroy(GetComponent<PhotonView>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player") || other.CompareTag("platform") && isFlying)
        {
            Explode();
        }
    }
}
