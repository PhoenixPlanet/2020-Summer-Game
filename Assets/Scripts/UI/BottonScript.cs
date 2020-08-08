using UnityEngine;
using System.Collections;

public class BottonScript : MonoBehaviour
{
    public GameObject ObjectOn;
    public GameObject ObjectOff;
    
    public void objon()
    {
        if (ObjectOff != null)
        {
            ObjectOff.SetActive(false);
        }
        if (ObjectOn != null)
        {
            ObjectOn.SetActive(true);
        }
        
    }
    public void QUIT()
    {
        Application.Quit();
    }

}


