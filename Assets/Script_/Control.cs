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
    public GameObject blockA;
    public GameObject blockB;


    public GameObject shootCollection;

    public Text winStatementA;
    public Text winStatementB;

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

        blockA.SetActive(false);
        blockB.SetActive(false);

        winStatementA.text = "";
        winStatementB.text = "";
	}

    // Update is called once per frame
    void Update()
    {
        if (canShoot) {
            if (sideA.transform.childCount <= 0) {
                winStatementB.text = "Player B wins!";
                canShoot = false;
            }
            else if (sideB.transform.childCount <= 0)
            {
                winStatementA.text = "Player A wins!";
                canShoot = false;
            }
            SetLivingCount();
        }
        //print(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (mousePressed < charactersTotal)
            {
				mousePressed++;
				SpawnCharacter();
				if (mousePressed == 5) {
                    blockA.SetActive(true);
                    print("reach five");
				}else if (mousePressed == 10)
                {
                    StartCoroutine(ShowSideA(true));
                    print("reach ten");
                    canShoot = true;
					canAShoot = true;
                    SetLivingCount();
                }
            }
            else if (canShoot)
            {
                SpawnShot();
            }
        }


    }

    public void SetLivingCount()
    {
        sideACount.text = sideB.transform.childCount.ToString();
        sideBCount.text = sideA.transform.childCount.ToString();
    }

    IEnumerator ShowSideA(bool showA, int seconds = 2)
    {
        if(showA)
        {
            blockB.SetActive(true);
            yield return new WaitForSeconds(seconds);
            blockA.SetActive(false);

        }
        else 
        {
            blockA.SetActive(true);
            yield return new WaitForSeconds(seconds);
            blockB.SetActive(false);
        }
    }

    private void SpawnCharacter()
    {
        var pos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        print("mouse pressed at " + pos);

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
			var shootCollect = Instantiate(shootCollection, new Vector3(0,0,0), Quaternion.identity);
            canAShoot = false;
			//var fire = Instantiate(shot, pos, Quaternion.identity);
            var fire = shootCollect.transform.GetChild(0);
            fire.transform.position = pos;

            var distToLine = (0 - fire.transform.position.x);
            var posFire = new Vector3(fire.transform.position.x + distToLine * 2, fire.transform.position.y + wind, 0);
            //var fireEnemy = Instantiate(hitResult, posFire, Quaternion.identity);
            var fireEnemy = shootCollect.transform.GetChild(1);
            fireEnemy.transform.position = posFire;

            //fire.transform.parent = shootCollect.transform;
            //fireEnemy.transform.parent = shootCollect.transform;

            StartCoroutine(ShowSideA(false));
            print("A hide");

        }
        else if (!canAShoot && (mainCam.ScreenToViewportPoint(Input.mousePosition).x > 0.5))
        {
            var shootCollect = Instantiate(shootCollection, new Vector3(0, 0, 0), Quaternion.identity);
            canAShoot = true;
            //var fire = Instantiate(shot, pos, Quaternion.identity);
            var fire = shootCollect.transform.GetChild(0);
            fire.transform.position = pos;

            var distToLine = (fire.transform.position.x - 0);
            var posFire = new Vector3(fire.transform.position.x-distToLine * 2, fire.transform.position.y + wind, 0);
            //var fireEnemy = Instantiate(hitResult, posFire, Quaternion.identity);
            var fireEnemy = shootCollect.transform.GetChild(1);
            fireEnemy.transform.position = posFire;
            //fire.transform.parent = shootCollect.transform;
            //fireEnemy.transform.parent = shootCollect.transform;

            StartCoroutine(ShowSideA(true));
            print("A show");
        }

    }
}
