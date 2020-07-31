using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveData : MonoBehaviour
{
    int nextScene;
    int resetGame = 0;

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
        resetGame++;
        if (resetGame >= 5)
        {
            PlayerPrefs.DeleteAll();
            UnityEngine.Debug.Log("Reset");
        }
    }
}
