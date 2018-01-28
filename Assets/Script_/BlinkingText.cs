using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour {

    bool canSee = true;
    Color tempCol = Color.black;
    Text txt;
    // Use this for initialization
    void Start () {
		txt = GetComponent<Text>();
    }

	// Update is called once per frame
	void Update () {
        if (tempCol.a >= 1f) canSee = true;
        else if (tempCol.a <= 0f) canSee = false;
        if (canSee)
        {
            FadeOut();
        }
        else
        {
            FadeIn();
        }

	}

    void FadeOut(){
        tempCol.a -= Time.deltaTime;
        txt.color = tempCol;
    }

    void FadeIn(){
        tempCol.a += Time.deltaTime;
        txt.color = tempCol;
    }
}
