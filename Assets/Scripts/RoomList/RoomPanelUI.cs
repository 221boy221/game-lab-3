using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Boy Voesten
    // TODO: Improve Join function

public class RoomPanelUI : MonoBehaviour {

    private RoomInfo _roomInfo;
    private RoomList _roomList;
    [SerializeField] private Text _roomNumber;
    [SerializeField] private Text _roomName;
    [SerializeField] private Text _playerCount;

    void Awake() {
        _roomInfo = GetComponent<RoomInfo>();
        _roomList = GameObject.FindGameObjectWithTag("RoomList").GetComponent<RoomList>();
    }

    public void UpdatePanel() {
        Debug.Log("Updating room panel UI...");
        _roomNumber.text    = _roomInfo.roomNumber.ToString();
        _roomName.text      = _roomInfo.roomName;
        _playerCount.text   = _roomInfo.amountOfPlayers + " / " + _roomInfo.maxAmountOfPlayers;
    }

    public void Join() {
        Debug.Log("Joining room #" + _roomInfo.roomNumber.ToString());
        _roomList.Join(_roomInfo.roomNumber);
    }
}
