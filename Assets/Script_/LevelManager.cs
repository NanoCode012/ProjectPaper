using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    float splashTime = 2f;
	void Start () {
        StartCoroutine(SplashScreen());
	}
	
    IEnumerator SplashScreen()
    {
        yield return new WaitForSeconds(splashTime);
        LoadLevel("Start");
    }

    public void LoadLevel(string name)
    {
        print("Load level : " + name);
        SceneManager.LoadScene(name);
    }

    public void PlaySound(AudioClip audio)
    {
        AudioSource.PlayClipAtPoint(audio, transform.position);
    }
}
