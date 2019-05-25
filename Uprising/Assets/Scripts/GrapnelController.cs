using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Items;
using Uprising.Players;

public class GrapnelController : MonoBehaviour
{
    public GameObject hook;
    public GameObject flyingHook;
    public GameObject player;
    public Grapnel grapnel;

    public void Shoot(Grapnel grapnel)
    {
        this.grapnel = grapnel;
        flyingHook = Instantiate(hook, transform.position, transform.rotation);
        Vector3 direction = grapnel.target.transform.position - transform.position;
        flyingHook.GetComponent<HookController>().Init(gameObject, direction.normalized);

        player.GetComponent<AudioManager>().PlaySound("Grapnel");
    }

    public void Detach()
    {
        Destroy(flyingHook);
        flyingHook = null;
    }
}
