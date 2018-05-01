using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class SpawnPoint : NetworkBehaviour {

    //public GameObject player1;
    //public GameObject player2;
    public Camera cam;
    public GameObject spawnloc;
    public GameObject camloc;
    public GameObject winScreen;
    public bool grantAblities;
    public bool exit;

    private int counter;
    private bool setServer = false;
    private bool setClient = false;
    private bool setCam = false;

    private float lerpTime = 2.5f;
    private bool islerping = false;
    private Vector3 startPos;
    private float lerpTimeStart;

	// Use this for initialization
	void Start () {
        counter = 0;
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (counter >= 2) {
            player1.GetComponent<Player>().lastPos = spawnloc.transform.position;
            player2.GetComponent<Player>().lastPos = spawnloc.transform.position;
        }
         */
        if (setClient && setServer && !setCam) {
            cam.GetComponent<Camera>().orthographicSize = -camloc.transform.position.z;
            lerpPos();
            //cam.transform.position = camloc.transform.position;
            Debug.Log("setting camera size");
            Debug.Log(-camloc.transform.position.z);
            //cam.GetComponent<SmashCamera>().FocusLevel = camloc;
            setCam = true;
        }
        if (islerping) {
            float deltat = Time.time - lerpTimeStart;
            float percent = deltat / lerpTime;
            cam.transform.position = Vector3.Lerp(startPos, camloc.transform.position, percent);
            if (percent >= 1.0f) {
                islerping = false;
            }
        }
	}

    void lerpPos() {
        islerping = true;
        lerpTimeStart = Time.time;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            counter++;
            Debug.Log(counter);
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player") {
            if (counter >= 2) {
                if (exit) {
                    Time.timeScale = 0;
                    winScreen.SetActive(true);
                }
                if (!setServer) {
                    if (other.gameObject.layer == 8) {
                        other.gameObject.GetComponent<Player>().lastPos = spawnloc.transform.position;
                        if(grantAblities){
                            other.gameObject.GetComponent<Player>().canTripleJump = true;
                        }
                        Debug.Log("set new spawn");
                        setServer = true;
                    }
                }
                if (!setClient) {
                    if (other.gameObject.layer == 9) {
                        other.gameObject.GetComponent<Player>().lastPos = spawnloc.transform.position;
                        if (grantAblities) {
                            other.gameObject.GetComponent<Player>().canStickToWalls = true;
                        }
                        Debug.Log("set new spawn");
                        setClient = true;
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            counter--;
            Debug.Log(counter);
        }
    }
}
