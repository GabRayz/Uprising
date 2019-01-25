using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{

    public AudioMixer audioMixer; 

    public void LoadScene(int scene)
    {
        // Application.LoadLevel(scene);
        SceneManager.LoadScene(scene);

        // GameObject newBonus = Instantiate(BonusPrefab, new Vector3(0, 5, 0), BonusPrefab.transform.rotation);
    }

    public void CreateObject(GameObject prefab)
    {
        GameObject newBonus = Instantiate(prefab, new Vector3(0, 5, 0), prefab.transform.rotation);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", volume);
    }
}
