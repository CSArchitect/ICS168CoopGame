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
        if (isLocalPlayer) {
            lights.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
        setColor();
    }

    void onConnected() {

    }

    void setColor() {
        if (isServer) {
            if (isLocalPlayer) {
                gameObject.GetComponent<SpriteRenderer>().material = blue;
                lights.GetComponent<Light>().color = blue.color;
            }
            else {
                gameObject.GetComponent<SpriteRenderer>().material = red;
                lights.GetComponent<Light>().color = red.color;

            }
        }
        else {
            if (isLocalPlayer) {
                gameObject.GetComponent<SpriteRenderer>().material = red;
                lights.GetComponent<Light>().color = red.color;

            }
            else {
                gameObject.GetComponent<SpriteRenderer>().material = blue;
                lights.GetComponent<Light>().color = blue.color;

            }
        }
    }
}
