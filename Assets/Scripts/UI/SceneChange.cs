using System.Collections;

using System.Collections.Generic;

using UnityEngine;


using UnityEngine.SceneManagement;


public class SceneChange : MonoBehaviour

{
    public string Index;

    public void change()

    {
        SceneManager.LoadScene(Index);
    }

    public void refresh()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);;
    }

 

}



