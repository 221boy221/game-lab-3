using UnityEngine;
using System.Collections;

// Boy Voesten
    // TODO: Remove panelUI when room is deleted

public class RoomList : MonoBehaviour {

    private HostData[] _hostData;
    public GameObject roomPanelUI;
    public Transform roomList;

    // Refreshes the list of rooms
    public void RefreshRoomList() {
        MasterServer.RequestHostList(ServerInfo.serverName);
        _hostData = MasterServer.PollHostList();

        // Loop through all rooms
        if (_hostData.Length > 0) {
            int hostDataLength = _hostData.Length;
            for (int i = 0; i < hostDataLength; i++) {
                CreateRoomPanel(i);
            }
        } else {
            Debug.Log("No rooms found");
        }
    }

    // This creates the panel for the room in the lobby
    private void CreateRoomPanel(int i) {
        GameObject temp;
        RoomInfo info;
        RoomPanelUI panelUI;

        temp = (GameObject)Instantiate(roomPanelUI);
        temp.transform.SetParent(roomList, false);

        info = temp.gameObject.GetComponent<RoomInfo>();
        info.roomNumber = i;
        info.roomName = _hostData[i].gameName;
        info.maxAmountOfPlayers = _hostData[i].playerLimit;
        info.amountOfPlayers = _hostData[i].connectedPlayers;

        panelUI = temp.gameObject.GetComponent<RoomPanelUI>();
        panelUI.UpdatePanel();
    }
    /*
    public IEnumerator RefreshHostList() {
        MasterServer.RequestHostList(_serverName);
        float timeEnd = Time.time + _refreshRequestLength;
        while (Time.time < timeEnd) {
            _hostData = MasterServer.PollHostList();
            yield return new WaitForEndOfFrame();
        }
    }
    */


    public void Join(int i) {
        Network.Connect(_hostData[i]);
    }

}
