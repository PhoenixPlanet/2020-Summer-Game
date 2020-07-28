using UnityEngine;
using System.Collections;

public class BottonScript : MonoBehaviour
{
    public GameObject ObjectOn;
    public GameObject ObjectOff;
    
    public void objon()
    {
        ObjectOff.SetActive(false);
        ObjectOn.SetActive(true);
    }
    public void QUIT()
    {
        Application.Quit();
    }

}


