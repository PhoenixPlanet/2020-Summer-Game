using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToNextLevel : MonoBehaviour
{
  
   int nextScene;
  
    void Start()
    {
        nextScene = SceneManager.GetActiveScene().buildIndex + 1;
    }
    // Update is called once per frame
    public void LoadNextLevel()
    {
    
        SceneManager.LoadScene(nextScene);
    }
}
