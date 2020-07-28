using UnityEngine;
using System.Collections;

public class BOTTON : MonoBehaviour
{
    public GameObject ObjectOn;
    public GameObject ObjectOff;
    public short Escape = 0;

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


