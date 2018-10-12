using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movements : MonoBehaviour {

    private Transform motion;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Déclaration des déclancheurs mouvements
        bool forward = Input.GetButton("Forward");
        bool backward = Input.GetButton("Backward");
        bool left = Input.GetButton("Left");
        bool right = Input.GetButton("Right");
        bool turnLeft = Input.GetButton("TurnLeft");
        bool turnRight = Input.GetButton("TurnRight");

        if (forward) transform.Translate(new Vector3((float)0.1, 0, 0));
        if (backward) transform.Translate(new Vector3((float)-0.1, 0, 0));
        if (left) transform.Translate(new Vector3(0, 0, (float)0.1));
        if (right) transform.Translate(new Vector3(0, 0, (float)-0.1));
        if (turnLeft) transform.Rotate(new Vector3(0, (float)0.5, 0));
        if (turnRight) transform.Rotate(new Vector3(0, (float)-0.5, 0));
    }
}
