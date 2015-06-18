using UnityEngine;
using System.Collections;

public class UserInfo : MonoBehaviour {

	private string _username = "test";
    private string _guid;

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        _guid = Network.player.guid;
    }

    public string username {
        get {
            return _username;
        }
        set {
            _username = value;
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
