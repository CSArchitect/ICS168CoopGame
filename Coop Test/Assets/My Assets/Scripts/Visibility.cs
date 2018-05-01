using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Visibility : NetworkBehaviour {

    public GameObject lights;
    public Material blue;
    public Material red;
    private Material mat;

	// Use this for initialization
	void Start () {
        if (true){//isLocalPlayer) {
            lights.SetActive(false);
        }
        setColor();
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    void onConnected() {

    }

    void setColor() {
        if (isServer) {
            if (isLocalPlayer) {
                gameObject.GetComponentInChildren<SpriteRenderer>().material = blue;
                lights.GetComponent<Light>().color = blue.color;
                //gameObject.GetComponent<Player>().canTripleJump = true;
                SetLayerR(gameObject, 8);
            }
            else {
                gameObject.GetComponentInChildren<SpriteRenderer>().material = red;
                lights.GetComponent<Light>().color = red.color;
                //gameObject.GetComponent<Player>().canStickToWalls = true;
                SetLayerR(gameObject, 9);
            }
        }
        else {
            if (isLocalPlayer) {
                gameObject.GetComponentInChildren<SpriteRenderer>().material = red;
                lights.GetComponent<Light>().color = red.color;
                //gameObject.GetComponent<Player>().canStickToWalls = true;
                SetLayerR(gameObject, 9);
            }
            else {
                gameObject.GetComponentInChildren<SpriteRenderer>().material = blue;
                lights.GetComponent<Light>().color = blue.color;
                //gameObject.GetComponent<Player>().canTripleJump = true;
                SetLayerR(gameObject, 8);
            }
        }
    }

    void SetLayerR(GameObject obj, int newLayer) {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform) {
            if (null == child) {
                continue;
            }
            SetLayerR(child.gameObject, newLayer);
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            transform.parent = other.transform;
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            transform.parent = null;
        }
    }
}
