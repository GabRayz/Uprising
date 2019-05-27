using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uprising.Players;

public class ForceFieldController : MonoBehaviour
{
    private float time = 15;
    public PlayerControl player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.activeSelf)
        {
            time -= Time.deltaTime;
            player.inventory.hudControl.ChangeDurability("ForceField", (int)time);
        }

        if (time <= 0f)
        {
            player.inventory.hudControl.HideBonus("ForceField");
            this.gameObject.SetActive(false);
            time = 15;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("belette"))
        {
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
    }
}
