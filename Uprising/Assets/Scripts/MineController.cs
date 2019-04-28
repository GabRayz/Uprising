using UnityEngine;
using System.Collections;
using Photon.Pun;

public class MineController : MonoBehaviour
{
    public GameObject explosionEffect;
    public PhotonView photonView;

    // Use this for initialization
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            Explode();
        }
    }

    void Explode()
    {
        explosionEffect = Instantiate(explosionEffect, this.transform.position, this.transform.rotation);
        StartCoroutine("ExplosionTimer");

        Collider[] colliders = Physics.OverlapSphere(this.transform.position, 5f);
        foreach (Collider c in colliders)
        {
            if(c.CompareTag("player"))
            {
                Rigidbody rb = c.GetComponent<Rigidbody>();
                rb.AddExplosionForce(1000f, this.transform.position, 5f);
            }
        }
    }

    IEnumerator ExplosionTimer()
    {
        float time = 1.5f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        Destroy(explosionEffect);

        if (photonView != null)
            PhotonNetwork.Destroy(this.GetComponent<PhotonView>());
        else
            Destroy(this);
    }
}
