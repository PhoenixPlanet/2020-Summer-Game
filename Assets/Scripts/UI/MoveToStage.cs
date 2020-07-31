using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToStage : MonoBehaviour
{
    public void LoadStage()
    {
        string objectName = gameObject.name;
        SceneManager.LoadScene(objectName);
    }
  }
