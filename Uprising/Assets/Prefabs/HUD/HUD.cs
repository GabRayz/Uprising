using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text Ammo;
    public Text Remain;
    public List<GameObject> bonuses;

    public void ChangeAmmo(int durability)
    {
        Ammo.text = durability.ToString();
    }

    public void ChangeRemain(int players)
    {
        Remain.text = players.ToString();
    }

    public void DisplayBonus(string name, int durability)
    {
        GameObject bonus = bonuses.Find(e => e.name == name);
        if (bonus == null)
        {
            Debug.LogWarning("This bonus cannot be found in the HUD");
            return;
        }
        bonus.transform.Find("RawImage").GetComponent<RawImage>().color = new Color(255, 255, 255, 1);
        bonus.transform.Find("Text").GetComponent<Text>().text = durability.ToString();
    }

    public void HideBonus(string name)
    {
        GameObject bonus = bonuses.Find(e => e.name == name);
        if (bonus == null)
        {
            Debug.LogWarning("This bonus cannot be found in the HUD");
            return;
        }
        bonus.transform.Find("RawImage").GetComponent<RawImage>().color = new Color(255, 255, 255, .4f);
        bonus.transform.Find("Text").GetComponent<Text>().text = "";
    }

    public void ChangeDurability(string name, int durability)
    {
        GameObject bonus = bonuses.Find(e => e.name == name);
        if (bonus == null)
        {
            Debug.LogWarning("This bonus cannot be found in the HUD");
            return;
        }
        bonus.transform.Find("Text").GetComponent<Text>().text = durability.ToString();
    }
}
