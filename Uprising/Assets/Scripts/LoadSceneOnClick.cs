using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

    //public GameObject BonusPrefab;

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
}
