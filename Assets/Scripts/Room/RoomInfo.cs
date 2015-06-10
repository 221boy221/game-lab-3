using UnityEngine;
using System.Collections;

// Boy Voesten

public class RoomInfo : MonoBehaviour {

    private int _roomNumber = 0;
    private string _roomName = "";
    private int _amountOfPlayers = 0;
    private int _maxAmountOfPlayers = 0;


    // GETTERS AND SETTERS

    public int roomNumber {
        get {
            return _roomNumber;
        }
        set {
            _roomNumber = value;
        }
    }

    public string roomName {
        get {
            return _roomName;
        }
        set {
            _roomName = value;
        }
    }

    public int amountOfPlayers {
        get {
            return _amountOfPlayers;
        }
        set {
            _amountOfPlayers = value;
        }
    }

    public int maxAmountOfPlayers {
        get {
            return _maxAmountOfPlayers;
        }
        set {
            _maxAmountOfPlayers = value;
        }
    }
    
}
