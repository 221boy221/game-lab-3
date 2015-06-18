using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour {

    public string scene;

    void Start() {
        Application.LoadLevel(scene);
    }
}
