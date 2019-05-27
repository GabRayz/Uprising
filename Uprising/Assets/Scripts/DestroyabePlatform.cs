using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyabePlatform : MonoBehaviour
{
    public int life = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("belette"))
        {
            life--;
            Destroy(other.gameObject);
        }   
    }
    // Update is called once per frame
    public void Update()
    {
        if (life <= 0)
            Destroy(this.transform.gameObject);
    }
}
