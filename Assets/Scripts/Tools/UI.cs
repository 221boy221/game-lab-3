using UnityEngine;
using System.Collections;

// Boy Voesten
    // Contains usefull UI functions

public class UI : MonoBehaviour {

    public void ToggleActive(GameObject obj) {
        obj.SetActive(!obj.activeSelf);
    }

    public void LoadScene(string scene) {
        Application.LoadLevel(scene);
    }

    public void ReloadScene() {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void SetUsername(string name) {
        GameObject.FindGameObjectWithTag("UserData").GetComponent<UserInfo>().username = name;
    }
}
