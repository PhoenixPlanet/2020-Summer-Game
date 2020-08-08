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
    public GameObject tutoUI;
    public Image tutoImage;
    public Sprite[] tutoImageSprites;

    private int currentLevel;

    void clearLevel()
    {
        audioSource.clip = clearClip;
        audioSource.Play();

        if (currentLevel == PlayerPrefs.GetInt("levelAt", 1))
        {
            PlayerPrefs.SetInt("levelAt", currentLevel + 1);
        }

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

        GameLogic.instance.startLevel(currentLevel);
        showTutorial();
    }
    
    public void showTutorial()
    {
        if (currentLevel == PlayerPrefs.GetInt("levelAt", 1))
        {
            switch (currentLevel)
            {
                case 1:
                    tutoImage.sprite = tutoImageSprites[0];
                    tutoUI.SetActive(true);
                    break;
                case 2:
                    tutoImage.sprite = tutoImageSprites[1];
                    tutoUI.SetActive(true);
                    break;
                case 3:
                    tutoImage.sprite = tutoImageSprites[2];
                    tutoUI.SetActive(true);
                    break;
                case 4:
                    tutoImage.sprite = tutoImageSprites[3];
                    tutoUI.SetActive(true);
                    break;
                case 5:
                    tutoImage.sprite = tutoImageSprites[4];
                    tutoUI.SetActive(true);
                    break;
                case 7:
                    tutoImage.sprite = tutoImageSprites[5];
                    tutoUI.SetActive(true);
                    break;
                default:
                    break;
            }
        }
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
        showTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
