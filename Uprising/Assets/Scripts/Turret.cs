using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Items;

public class Turret : MonoBehaviour
{
    public GameObject belette;
    DefaultGun gun;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Timer");
        gun = new DefaultGun(100, 60, 20, 20, null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Timer()
    {
        float time = 1f;
        while(time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        Shoot();
        StartCoroutine("Timer");
    }

    void Shoot()
    {
        GameObject bel = Instantiate(belette, transform.position, transform.rotation);
        bel.GetComponent<Rigidbody>().AddForce(transform.forward * 500);
        bel.GetComponent<Belette>().InitBelette(gun.knockback);
    }
}
