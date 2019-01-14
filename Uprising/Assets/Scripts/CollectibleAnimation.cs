using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleAnimation : MonoBehaviour {

    public Rigidbody collectible;
    private Vector3 rotation;

	// Use this for initialization
	void Start () {
        collectible = GetComponent<Rigidbody>();
        rotation = new Vector3(60, 60, 60);
	}
	
    private void FixedUpdate()
    {
        Quaternion deltaRot = Quaternion.Euler(rotation * Time.deltaTime);
        collectible.MoveRotation(collectible.rotation * deltaRot);
    }
}
