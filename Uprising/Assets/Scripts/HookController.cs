using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    public float maxDist;
    public GrapnelController grapnel;
    public int speed;
    Rigidbody rb;
    public bool isFlying;
    public bool isAttached;
    LineRenderer lineRenderer;

    public void Init(GameObject grapnel, Vector3 direction)
    {
        this.grapnel = grapnel.GetComponent<GrapnelController>();
        rb = GetComponent<Rigidbody>();
        rb.AddForce(direction * speed, ForceMode.Force);
        isFlying = true;
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isFlying || isAttached)
        {
            lineRenderer.SetPosition(0, this.transform.position);
            lineRenderer.SetPosition(1, grapnel.transform.position);
        }

        if (isFlying)
        {
            float dist = (grapnel.transform.position - this.transform.position).magnitude;
            if (dist > maxDist)
                grapnel.grapnel.Detach();
        }

        if(isAttached)
        {
            float dist = (grapnel.transform.position - this.transform.position).magnitude;
            if (dist < 2)
            {
                grapnel.grapnel.Detach();
            }

            Vector3 dir = (transform.position - grapnel.player.transform.position).normalized * 25;
            grapnel.player.GetComponent<Rigidbody>().velocity = dir;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("platform") && !isAttached)
        {
            Debug.Log("hit");
            isFlying = false;
            rb.isKinematic = true;
            isAttached = true;
            rb.ResetInertiaTensor();
        }
    }
}
