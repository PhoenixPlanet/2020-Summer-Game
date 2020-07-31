using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    public Button[] lvlButtons;
    public int activeLevels;

    void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt", 1);

        for(int i=0; i<activeLevels; i++)
        {
            if (i + 1 > levelAt)
                lvlButtons[i].interactable = false;
        }

        for(int i = activeLevels; i < lvlButtons.Length; i++)
        {
            lvlButtons[i].interactable = false;
        }
    }
}
