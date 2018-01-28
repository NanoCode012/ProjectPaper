using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Control : MonoBehaviour {

    public Text sideACount;
    public Text sideBCount;
    public GameObject sideA;
    public GameObject sideB;
    public GameObject characterA;
    public GameObject characterB;

    public AudioClip placeCharacters;
    public AudioClip endSound;

    public GameObject blockA;
    public GameObject blockB;
    public GameObject grid;
    public static bool gridTurnedOn = true;

    public GameObject shootCollection;

    public Text winStatementA;
    public Text winStatementB;

    Camera mainCam = new Camera();

    int mousePressed = 0;
    const int charactersTotal = 10;

	bool canShoot;
    bool isATurn;
    bool gridAppeared = false;

    int wind = 0;

    Vector3 borderMin;
    Vector3 borderMax;


	// Use this for initialization
	void Start () {
        mainCam = Camera.main;
        sideACount.text = "";
        sideBCount.text = "";
        for (int i = 0; i < charactersTotal / 2; i++)
        {
            sideACount.text += " • ";
            sideBCount.text += " • ";
        }
        StartCoroutine(ShowSideA(true, 0.5f));

        winStatementA.text = "";
        winStatementA.transform.GetChild(0).GetComponent<Text>().text = "";
        winStatementA.transform.GetChild(0).gameObject.SetActive(false);
        winStatementB.text = "";
        winStatementB.transform.GetChild(0).GetComponent<Text>().text = "";
        winStatementB.transform.GetChild(0).gameObject.SetActive(false);

        grid.SetActive(gridTurnedOn);

        borderMin = mainCam.WorldToViewportPoint(new Vector3(-6.4f, -4.8f, 0f));
        borderMax = mainCam.WorldToViewportPoint(new Vector3(6.4f, 4.8f, 0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot) {
            if (sideA.transform.childCount <= 0) {
				winStatementA.transform.GetChild(0).gameObject.SetActive(true);
                winStatementB.text = "Player B wins!";
                winStatementB.transform.GetChild(0).GetComponent<Text>().text = "Play Again!";
				AudioSource.PlayClipAtPoint(endSound, transform.position);
                canShoot = false;
            }
            else if (sideB.transform.childCount <= 0)
            {
				winStatementA.transform.GetChild(0).gameObject.SetActive(true);
                winStatementA.text = "Player A wins!";
                winStatementA.transform.GetChild(0).GetComponent<Text>().text = "Play Again!";
				AudioSource.PlayClipAtPoint(endSound, transform.position);
                canShoot = false;
            }
            SetLivingCount();

        }
        //print(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = mainCam.ScreenToViewportPoint(Input.mousePosition);

            if ((mousePos.y > borderMin.y) && (mousePos.y < borderMax.y) && (mousePos.x > borderMin.x) && (mousePos.x < borderMax.x))
            {
                if (mousePressed < charactersTotal)
                {
                    mousePressed++;
                    SpawnCharacter(mousePos);
                    AudioSource.PlayClipAtPoint(placeCharacters, transform.position);
                    if (mousePressed == 5)
                    {
                        StartCoroutine(ShowSideA(false, 1f));
                        print("reach five");
                    }
                    else if (mousePressed == 10)
                    {
                        StartCoroutine(ShowSideA(true));
                        print("reach ten");
                        canShoot = true;
                        isATurn = true;
                        SetLivingCount();
                    }
                }
                else if (canShoot && gridAppeared)
                {
                    SpawnShot(mousePos);
                }
            }
        }


    }

    public void SetLivingCount()
    {
        sideACount.text = "";
        sideBCount.text = "";
        foreach (Transform child in sideB.transform) sideACount.text += " • ";
        foreach (Transform child in sideA.transform) sideBCount.text += " • ";
    }

    IEnumerator ShowSideA(bool showA, float seconds = 2f)
    {
        gridAppeared = false;
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
		gridAppeared = true;
    }

    private void SpawnCharacter(Vector3 mousePos)
    {
        var pos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        //print("mouse pressed at " + pos);


        if ((mousePressed <= 5) && (mousePos.x < 0.5))
        {
            var stickman = Instantiate(characterA, pos, Quaternion.identity);
            stickman.transform.parent = sideA.transform;
            sideACount.text = "";
            for (int i = 0; i < (charactersTotal / 2 - mousePressed); i++) sideACount.text += " • ";

        }
        else if ((5 < mousePressed) && (mousePressed <= 10) && (mousePos.x > 0.5))
        {
            var stickman = Instantiate(characterB, pos, Quaternion.identity);
            stickman.transform.parent = sideB.transform;
            sideBCount.text = "";
            for (int i = 0; i < charactersTotal - mousePressed; i++) sideBCount.text += " • ";
        }
        else mousePressed--;
    }

    private void SpawnShot(Vector3 mousePos)
    {
        var pos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        if (isATurn && (mousePos.x < 0.5))
        {
			var shootCollect = Instantiate(shootCollection, new Vector3(0,0,0), Quaternion.identity);
            isATurn = false;
            var fire = shootCollect.transform.GetChild(0);
            fire.transform.position = pos;

            var distToLine = (0 - fire.transform.position.x);
            var posFire = new Vector3(fire.transform.position.x + distToLine * 2, fire.transform.position.y + wind, 0);
            var fireEnemy = shootCollect.transform.GetChild(1);
            fireEnemy.transform.position = posFire;

            StartCoroutine(ShowSideA(false));
            print("A hide");

        }
        else if (!isATurn && (mainCam.ScreenToViewportPoint(Input.mousePosition).x > 0.5))
        {
            var shootCollect = Instantiate(shootCollection, new Vector3(0, 0, 0), Quaternion.identity);
            isATurn = true;
            var fire = shootCollect.transform.GetChild(0);
            fire.transform.position = pos;

            var distToLine = (fire.transform.position.x - 0);
            var posFire = new Vector3(fire.transform.position.x-distToLine * 2, fire.transform.position.y + wind, 0);
            var fireEnemy = shootCollect.transform.GetChild(1);
            fireEnemy.transform.position = posFire;

            StartCoroutine(ShowSideA(true));
            print("A show");
        }

    }
}
