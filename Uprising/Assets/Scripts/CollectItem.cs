using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItem : MonoBehaviour {
    // This script is to be attached to the player. It tests the collision with a item.
    public Rigidbody player;

	// Use this for initialization
	void Start () {
        player = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("item"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
