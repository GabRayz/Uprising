using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mirror : MonoBehaviour {

    public Transform PlayerCam;
    public Transform MirrorCam;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CalculRotation() {
        Vector3 direction = (PlayerCam.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);

        rotation.eulerAngles = transform.eulerAngles - rotation.eulerAngles;
        MirrorCam.localRotation = rotation;
    }
}
