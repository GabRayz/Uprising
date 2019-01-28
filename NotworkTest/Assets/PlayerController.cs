using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<PhotonView>().IsMine)
        {
            if (Input.GetKey(KeyCode.Z))
                transform.position += transform.forward * Time.deltaTime * 5;
            if (Input.GetKey(KeyCode.Q))
                transform.position -= transform.right * Time.deltaTime * 5;
            if (Input.GetKey(KeyCode.S))
                transform.position -= transform.forward * Time.deltaTime * 5;
            if (Input.GetKey(KeyCode.D))
                transform.position += transform.right * Time.deltaTime * 5;
        }
    }
}
