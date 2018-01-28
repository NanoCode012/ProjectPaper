using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{

    public AudioClip[] audioClips;
    public AudioClip preMain;
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        PlayAudio();

        SceneManager.sceneLoaded += OnLevelLoaded;
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        AudioSource.PlayClipAtPoint(preMain, transform.position);
    }
    void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        PlayAudio();
    }

    void PlayAudio()
    {
        var newClip = audioClips[SceneManager.GetActiveScene().buildIndex];
        if (audioSource.clip != newClip && newClip != null)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }

    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume;
    }
}

