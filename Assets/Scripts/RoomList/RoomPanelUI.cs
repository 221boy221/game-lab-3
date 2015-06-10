using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Boy Voesten
    // TODO: Fix the join function

public class RoomPanelUI : MonoBehaviour {

    private RoomInfo _roomInfo;
    [SerializeField] private Text _roomNumber;
    [SerializeField] private Text _roomName;
    [SerializeField] private Text _playerCount;

    void Awake() {
        _roomInfo = GetComponent<RoomInfo>();
    }

    public void UpdatePanel() {
        _roomNumber.text    = _roomInfo.roomNumber.ToString();
        _roomName.text      = _roomInfo.roomName;
        _playerCount.text   = _roomInfo.amountOfPlayers + " / " + _roomInfo.maxAmountOfPlayers;
    }

    public void Join() {
        //RoomList.Join(_roomNumber);
    }
}
