using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<LevelManager>();
            }

            return m_instance;
        }
    }
    private static LevelManager m_instance;

    public AudioClip clearClip;
    private AudioSource audioSource;

    public GameObject ingameUI;
    public GameObject clearUI;

    private int currentLevel;

    void clearLevel()
    {
        audioSource.clip = clearClip;
        audioSource.Play();

        clearUI.SetActive(true);
    }

    public void refreshLevel()
    {
        clearUI.SetActive(false);
        GameLogic.instance.refreshLevel(currentLevel);
    }

    public void restartLevel()
    {
        clearUI.SetActive(false);
        GameLogic.instance.startLevel(currentLevel);
    }

    public void nextLevel()
    {
        ingameUI.SetActive(true);
        clearUI.SetActive(false);
        currentLevel++;
        if (currentLevel > PlayerPrefs.GetInt("levelAt", 1))
        {
            PlayerPrefs.SetInt("levelAt", currentLevel);
        }
        GameLogic.instance.startLevel(currentLevel);
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        GameLogic.instance.OnMarbleClear += clearLevel;

        ingameUI.SetActive(true);
        clearUI.SetActive(false);
        //currentLevel = 10;
        currentLevel = PlayerPrefs.GetInt("current_level");
        Debug.Log(currentLevel);
        GameLogic.instance.startLevel(currentLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
