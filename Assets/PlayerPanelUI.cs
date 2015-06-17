using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Boy Voesten

public class PlayerPanelUI : MonoBehaviour {

    [SerializeField] private Text _playerLvlTxt;
    [SerializeField] private Text _playerNameTxt;
    private int _playerLvl;
    private string _playerName;

    void Awake() {
        _playerLvlTxt.text = "Lvl " + _playerLvl;
        _playerNameTxt.text = _playerName;
    }


    // GETTERS & SETTERS

    public int playerLvl {
        get {
            return _playerLvl;
        }
        set {
            _playerLvl = value;
        }
    }

    public string playerName {
        get {
            return _playerName;
        }
        set {
            _playerName = value;
        }
    }

}
