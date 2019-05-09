using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapnelController : MonoBehaviour
{
    public GameObject hook;
    public GameObject flyingHook;
    public GameObject player;

    public void Shoot()
    {
        Debug.Log("Hook shot");
        flyingHook = Instantiate(hook, transform.position, transform.rotation);
        flyingHook.GetComponent<HookController>().Init(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            Shoot();
    }
}
