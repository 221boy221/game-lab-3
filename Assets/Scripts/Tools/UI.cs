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

    public void QuitGame() {
        Application.Quit();
    }
}
