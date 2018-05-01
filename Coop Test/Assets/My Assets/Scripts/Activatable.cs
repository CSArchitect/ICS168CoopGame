using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class Activatable : NetworkBehaviour {

    public bool pressurePlate;
    public bool spawner;
    public bool door;
    public bool exit;

    public GameObject[] targets;

    public bool activated = false;
    private bool origRendered;
    private bool origInteract;
    private bool serverVisible;
    private bool clientVisible;
    private bool serverInteractable;
    private bool clientInteractable;
    private int exitCounter = 0;


    /*
    string[] supportedNetworkLevels = new[] { "1", "3", "2" };
    string disconnectedLevel = "loader";
    int lastLevelPrefix = 0;
    NetworkView networkView;

    void Awake() {
        DontDestroyOnLoad(this);
        networkView = new NetworkView();
        networkView.group = 1;
        Application.LoadLevel(disconnectedLevel);
    }

    void OnGUI() {
        if (Network.peerType != NetworkPeerType.Disconnected) {
            GUILayout.BeginArea(new Rect(0, Screen.height - 30, Screen.width, 30));
            GUILayout.BeginHorizontal();
  
            foreach (var level in supportedNetworkLevels) {
                if (GUILayout.Button(level)) {
                Network.RemoveRPCsInGroup(0);
                Network.RemoveRPCsInGroup(1);
                networkView.RPC("loadLevel", RPCMode.AllBuffered, level, lastLevelPrefix + 1)
                }
            }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        }
    }

    [RPC]
    IEnumerator LoadLevel(string level, int levelPrefix)


    */

    // Use this for initialization
    void Start() {
        origRendered = gameObject.GetComponentInChildren<SpriteRenderer>().enabled;
        origInteract = gameObject.GetComponentInChildren<BoxCollider2D>().enabled;
        serverVisible = gameObject.GetComponent<PlatformVisibility>().serverVisible;
        clientVisible = gameObject.GetComponent<PlatformVisibility>().clientVisible;
        serverInteractable = gameObject.GetComponent<PlatformVisibility>().serverInteractable;
        clientInteractable = gameObject.GetComponent<PlatformVisibility>().clientInteractable;

        if (spawner) {
            foreach (GameObject T in targets) {
                T.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (door) {
            //doorLogic();
        }
        if (exit) {
            exitLogic();
        }

    }

    void exitLogic() {
        if (exitCounter >= 4) {
            if (SceneManager.GetActiveScene().name == "1") {
                SceneManager.LoadScene("3");
            }
            else if (SceneManager.GetActiveScene().name == "3") {
                SceneManager.LoadScene("2");
            }
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
                //T.GetComponent<Activatable>().activated = true;
                if (T.activeInHierarchy) {
                    T.SetActive(false);
                }
                else {
                    T.SetActive(true);
                }
            }
        }
        if (exit) {
            exitCounter++;
            Debug.Log(exitCounter);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (pressurePlate) {
            foreach (GameObject T in targets) {
                //T.GetComponent<Activatable>().activated = false;
                if (!T.activeInHierarchy) {
                    T.SetActive(true);
                }
                else {
                    T.SetActive(false);
                }
            }
        }
        if (exit) {
            exitCounter--;
            Debug.Log(exitCounter);
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
