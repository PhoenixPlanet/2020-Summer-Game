using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundSoundManager : MonoBehaviour
{
    private static BackgroundSoundManager instance = null;

    private string current_scene;

    private AudioSource audioSource;
    public AudioClip[] bgm;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            current_scene = SceneManager.GetActiveScene().name;
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = bgm[0];
            audioSource.Play();
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string new_scene = SceneManager.GetActiveScene().name;
        if (current_scene != new_scene)
        {
            if (current_scene == "MainLevel")
            {
                audioSource.clip = bgm[0];
                audioSource.Play();
            } else if (new_scene == "MainLevel")
            {
                audioSource.clip = bgm[1];
                audioSource.Play();
            }
            current_scene = SceneManager.GetActiveScene().name;
        }
    }
}
