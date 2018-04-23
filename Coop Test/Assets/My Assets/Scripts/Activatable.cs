using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Activatable : NetworkBehaviour {

    public bool pressurePlate;
    public bool door;

    public GameObject[] targets;

    public bool activated = false;
    private bool origRendered;
    private bool origInteract;
    private bool serverVisible;
    private bool clientVisible;
    private bool serverInteractable;
    private bool clientInteractable;
    // Use this for initialization
    void Start() {
        origRendered = gameObject.GetComponentInChildren<SpriteRenderer>().enabled;
        origInteract = gameObject.GetComponentInChildren<BoxCollider2D>().enabled;
        serverVisible = gameObject.GetComponent<PlatformVisibility>().serverVisible;
        clientVisible = gameObject.GetComponent<PlatformVisibility>().clientVisible;
        serverInteractable = gameObject.GetComponent<PlatformVisibility>().serverInteractable;
        clientInteractable = gameObject.GetComponent<PlatformVisibility>().clientInteractable;
    }

    // Update is called once per frame
    void Update() {
        if (door) {
            doorLogic();
        }
    }

    void doorLogic() {
        Component[] renderers;
        Component[] colliders;
        if (activated) {
            renderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
            colliders = gameObject.GetComponentsInChildren<BoxCollider2D>();
            foreach (SpriteRenderer SP in renderers) {
                if ((isServer && serverVisible) || (!isServer && clientVisible)) {
                    SP.enabled = !origRendered;
                }
            }
            foreach (BoxCollider2D BC in colliders) {
                if ((isServer && serverInteractable) || (!isServer && clientInteractable)) {
                    BC.enabled = !origInteract;
                }
            }
        }
        else {
            renderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
            colliders = gameObject.GetComponentsInChildren<BoxCollider2D>();
            foreach (SpriteRenderer SP in renderers) {
                if ((isServer && serverVisible) || (!isServer && clientVisible)) {
                    SP.enabled = origRendered;
                }
            }
            foreach (BoxCollider2D BC in colliders) {
                if ((isServer && serverInteractable) || (!isServer && clientInteractable)) {
                    BC.enabled = origInteract;
                }
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D other){
        if (pressurePlate) {
            foreach (GameObject T in targets) {
                T.GetComponent<Activatable>().activated = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (pressurePlate) {
            foreach (GameObject T in targets) {
                T.GetComponent<Activatable>().activated = false;
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
}
