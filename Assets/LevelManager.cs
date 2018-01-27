using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    float splashTime = 2f;
	// Use this for initialization
	void Start () {
        StartCoroutine(SplashScreen());
	}
	
    IEnumerator SplashScreen()
    {
        yield return new WaitForSeconds(splashTime);
        LoadLevel("Game");
    }

    public void LoadLevel(string name)
    {
        print("Load level : " + name);
        SceneManager.LoadScene(name);
    }

}
