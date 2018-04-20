using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlatformVisibility : NetworkBehaviour {

    public bool server = false;
    public bool client = false;
    public bool serverVisible = false;
    public bool clientVisible = false;
    public bool blueOnly = false;
    public bool redOnly = false;

    public Material blue;
    public Material red;

	// Use this for initialization
	void Start () {
        if (!((server && isServer) || (client && !isServer))) {
            gameObject.SetActive(false);
        }
        if ((isServer && !serverVisible) || (!isServer && !clientVisible)) {
            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
        if (blueOnly) {
            gameObject.GetComponentInChildren<SpriteRenderer>().material = blue;
            SetLayerR(gameObject, 10);
        }
        if (redOnly) {
            gameObject.GetComponentInChildren<SpriteRenderer>().material = red;
            SetLayerR(gameObject, 11);
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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
