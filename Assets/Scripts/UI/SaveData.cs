using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveData : MonoBehaviour
{
    int nextScene;

    void Start()
    {
       nextScene = SceneManager.GetActiveScene().buildIndex + 1;
    }

    
    public void Save()
    {
        if(nextScene> PlayerPrefs.GetInt("levelAt"))
        {
            PlayerPrefs.SetInt("levelAt", nextScene);
        }
    }
    public void Reset()
    {
        PlayerPrefs.DeleteAll();
    }
}
