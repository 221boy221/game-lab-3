using UnityEngine;
using System.Collections;

public class UserInfo : MonoBehaviour {

	private string _username;
    private string _guid;
    private static bool spawned = false;

    void Awake() {
        // We do not allow 2 of those to be spawned
        if (spawned == false) {
            spawned = true;
            DontDestroyOnLoad(transform.gameObject);
        } else {
            DestroyImmediate(gameObject);
        }
    }

    void Start() {
        _guid = Network.player.guid;
    }

    // GETTERS & SETTERS //

    public string username {
        get {
            return _username;
        }
        set {
            _username = value;
            Debug.Log("Setting user to: " + value);
        }
    }

    public string guid {
        get {
            return _guid;
        }
        set {
            _guid = value;
        }
    }
}
