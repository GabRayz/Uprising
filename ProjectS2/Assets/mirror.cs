using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour {
    public Transform PlayerCam;
    public Transform MirrorCam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 direction = (PlayerCam.position - MirrorCam.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);

        rotation.eulerAngles = transform.eulerAngles - rotation.eulerAngles;
        MirrorCam.localRotation = rotation;
        //MirrorCam.Rotate(new Vector3(0, (float)0.1, 0));
	}
}
