using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour {

    Text txt;
	// Use this for initialization
	void Start ()
    {
        txt = GetComponent<Text>();
        SetText();
    }

    public void ChangeGridActive()
    {
        Control.gridTurnedOn = !Control.gridTurnedOn;
        SetText();
    }

    private void SetText()
    {
        if (Control.gridTurnedOn) txt.text = "ON";
        else txt.text = "OFF";
    }
}
