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

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "belette")
            life--;
    }
    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
            Destroy(this.transform.gameObject);
    }
}
