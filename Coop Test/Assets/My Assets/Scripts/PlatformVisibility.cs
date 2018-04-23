using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlatformVisibility : NetworkBehaviour {

    //public bool serverSide = false;
    //public bool clientSide = false;
    public bool serverVisible = false;
    public bool clientVisible = false;
    public bool serverInteractable = false;
    public bool clientInteractable = false;
    //private bool server = false;
    //private bool client = false;

    public Material blue;
    public Material red;

	// Use this for initialization
	void Start () {
        if ((isServer && !serverVisible) || (!isServer && !clientVisible)) {
            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
        if (serverInteractable && !clientInteractable) {
            gameObject.GetComponentInChildren<SpriteRenderer>().material = blue;
            SetLayerR(gameObject, 10);
        }
        else if (!serverInteractable && clientInteractable) {
            gameObject.GetComponentInChildren<SpriteRenderer>().material = red;
            SetLayerR(gameObject, 11);
        }
        else if (serverInteractable && clientInteractable) {
            SetLayerR(gameObject, 12);
        }
	}
	
	// Update is called once per frame
	void Update () {
   
	}
    /*
    void isWho() {
        if (isServer && isClient) {
            server = true;
            client = false;
        }
        else {
            server = false;
            client = true;
        }
    }

    void exists() {
        if ((server && serverSide) || (client && clientSide)) {
            gameObject.SetActive(true);
        }
        else {
            gameObject.SetActive(false);
        }
    }
    */
    void SetLayerR(GameObject obj, int newLayer) {
        obj.layer = newLayer;
        foreach(Transform child in obj.transform){
            if (null == child) {
                continue;
            }
            SetLayerR(child.gameObject, newLayer);
        }
    }
}
