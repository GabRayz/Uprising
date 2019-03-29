using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Players;

public class BearTrapController : MonoBehaviour
{
    private float activationCoolDown = 2;
    private bool isReady = false;
    private float trapDuration = 5;
    private GameObject target;

    // Update is called once per frame
    void Update()
    {
        if (activationCoolDown > 0)
            activationCoolDown -= Time.deltaTime;
        else if (!isReady)
            isReady = true;

        if(target != null)
        {
            trapDuration -= Time.deltaTime;
            if(trapDuration <= 0)
            {
                Debug.Log("Player released");
                target.SendMessage("ModifySpeed", 100);
                target.SendMessage("ModifyJumpHeight", 1000);
                Destroy(this.gameObject);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(isReady && other.CompareTag("player"))
        {
            Debug.Log("Player trapped !");
            target = other.gameObject;
            target.SendMessage("ModifySpeed", -100);
            target.SendMessage("ModifyJumpHeight", -1000);
            isReady = false;
        }
    }
}
