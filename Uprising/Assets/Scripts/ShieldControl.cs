using UnityEngine;
using Uprising.Items;
using Uprising.Players;

public class ShieldControl : MonoBehaviour
{
    int durability;
    Shield item;
    PlayerControl playerControl;
    bool isUsed = false;

    public void Init(int durability, Shield item)
    {
        this.durability = durability;
        this.item = item;
        playerControl = item.playerControl;
        isUsed = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("belette"))
        {
            durability -= (int)other.GetComponent<Belette>().power;

            other.gameObject.SetActive(false);
            Destroy(other.gameObject);

            if (durability <= 0)
                item.Break();
        }
    }
}
