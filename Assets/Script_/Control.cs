using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Control : MonoBehaviour {

    public Text sideACount;
    public Text sideBCount;
    public GameObject sideA;
    public GameObject sideB;
    public GameObject character;

	public GameObject shot;
    public GameObject hitResult;

    Camera mainCam = new Camera();

    int mousePressed = 0;
    const int charactersTotal = 10;

	bool canShoot;
    bool canAShoot;

    int wind = 0;

	// Use this for initialization
	void Start () {
        mainCam = Camera.main;

        sideACount.text = (charactersTotal/2).ToString();
        sideBCount.text = (charactersTotal/2).ToString();

        ShowSideA(true);
	}

    // Update is called once per frame
    void Update()
    {
        //print(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (mousePressed < charactersTotal)
            {
				mousePressed++;
				SpawnCharacter();
				if (mousePressed == 5) {
					StartCoroutine(ShowSideA(false));
				}else if (mousePressed == 10)
                {
                    StartCoroutine(ShowSideA(true));
                    canAShoot = true;
                    canShoot = true;
                    SetLivingCount();
                }
            }
            else if (canShoot)
            {
                SpawnShot();
            }
        }


    }

    private void SetLivingCount()
    {
        sideACount.text = sideB.transform.childCount.ToString();
        sideBCount.text = sideA.transform.childCount.ToString();
    }

    IEnumerator ShowSideA(bool showA, int seconds = 3)
    {
        var timeStarted = DateTime.Now;
        if(showA)
        {
            sideB.SetActive(false);
            yield return new WaitForSeconds(seconds);
            sideA.SetActive(true);

        }
        else 
        {
            sideA.SetActive(false);
            yield return new WaitForSeconds(seconds);
            sideB.SetActive(true);
        }
    }

    private void SpawnCharacter()
    {
        var pos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        if (mousePressed <= 5 && (mainCam.ScreenToViewportPoint(Input.mousePosition).x < 0.5))
        {
            var stickman = Instantiate(character, pos, Quaternion.identity);
            stickman.transform.parent = sideA.transform;
            sideACount.text = (charactersTotal / 2 - mousePressed).ToString();

        }
        else if (5 < mousePressed && mousePressed <= 10 && (mainCam.ScreenToViewportPoint(Input.mousePosition).x > 0.5))
        {
            var stickman = Instantiate(character, pos, Quaternion.identity);
            stickman.transform.parent = sideB.transform;
            sideBCount.text = (charactersTotal - mousePressed).ToString();
        }
        else mousePressed--;
    }

    private void SpawnShot()
    {
        var pos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        if (canAShoot && (mainCam.ScreenToViewportPoint(Input.mousePosition).x < 0.5))
        {
            canAShoot = false;
			var fire = Instantiate(shot, pos, Quaternion.identity);
            var posFire = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0, 0));
            posFire.y = wind;
            posFire.z = 0;

            var fireEnemy = Instantiate(hitResult, posFire, Quaternion.identity);
            fireEnemy.transform.parent = fire.transform;

            StartCoroutine(ShowSideA(false));



        }
        else if (!canAShoot && (mainCam.ScreenToViewportPoint(Input.mousePosition).x > 0.5))
        {
            canAShoot = true;
            var fire = Instantiate(shot, pos, Quaternion.identity);
            var posFire = mainCam.ViewportToWorldPoint(new Vector3(-0.5f, 0, 0));
            posFire.y = wind;
            posFire.z = 0;

            var fireEnemy = Instantiate(hitResult, posFire, Quaternion.identity);
            fireEnemy.transform.parent = fire.transform;

            StartCoroutine(ShowSideA(true));
        }

    }
}
