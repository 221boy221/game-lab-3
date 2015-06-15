using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// Boy Voesten
    // TODO: Improve or recreate the removal of the panelUI

public class RoomList : MonoBehaviour {

    private HostData[] _hostData;
    private float _refreshRequestLength = 2f;
    private Dictionary<int, GameObject> _roomList = new Dictionary<int,GameObject>();
    public GameObject roomPanelPref;
    public Transform roomBrowser;
    private Text _refreshTxt;


    void Start() {
        _refreshTxt = GetComponentInChildren<Text>();
    }

    // For the UI to access
    public void RefreshList() {
        // Get the HostData from the server you're in
        MasterServer.RequestHostList(ServerInfo.serverName);

        StartCoroutine(RefreshHostData());
    }

    // Refreshes the hostData
    private IEnumerator RefreshHostData() {
        // Make it so it refreshes for a few seconds
        float timeEnd = Time.time + _refreshRequestLength;
        _refreshTxt.text = "Refreshing...";
        while (Time.time < timeEnd) {
            _hostData = MasterServer.PollHostList();
            yield return new WaitForEndOfFrame();
        }
        _refreshTxt.text = "Refresh";
        Debug.Log("hostData length (amount of rooms): " + _hostData.Length);

        // Check if room needs a panel or not
        if (_hostData.Length > 0) {
            int hostDataLength = _hostData.Length;
            for (int i = 0; i < hostDataLength; i++) {
                if (_roomList.ContainsKey(i)) {
                    Debug.Log("Room #" + i + " already exists");
                } else {
                    Debug.Log("Adding room #" + i);
                    CreateRoomPanel(i);
                }
            }
            CheckForRemove();
        } else {
            Debug.Log("No rooms found - clearing roomList...");

            foreach (KeyValuePair<int, GameObject> entry in _roomList) {
                Destroy(_roomList[entry.Key].gameObject);
            }
            _roomList.Clear();
            CheckForRemove();
        }
    }

    // This creates the panel for the room in the lobby
    private void CreateRoomPanel(int i) {
        GameObject panelPref;
        RoomInfo info;
        RoomPanelUI panelUI;

        panelPref = (GameObject)Instantiate(roomPanelPref);
        panelPref.transform.SetParent(roomBrowser, false);

        info = panelPref.gameObject.GetComponent<RoomInfo>();
        info.roomNumber         = i;
        info.roomName           = _hostData[i].gameName;
        info.maxAmountOfPlayers = _hostData[i].playerLimit;
        info.amountOfPlayers    = _hostData[i].connectedPlayers;
        info.guid               = _hostData[i].guid;

        panelUI = panelPref.gameObject.GetComponent<RoomPanelUI>();
        //panelUI.UpdatePanel();

        _roomList.Add(i, panelPref);
    }

    private void CheckForRemove() {
        Debug.Log("Checking for rooms to remove...");

        List<int> temp = new List<int>();
        List<int> temp2 = new List<int>();

        for (int i = 0; i < _hostData.Length - 1; i++) {
            foreach (KeyValuePair<int, GameObject> entry in _roomList) {
                if (_hostData[i].guid == _roomList[entry.Key].GetComponent<RoomInfo>().guid) {
                    Debug.Log("Temp.Add: " + entry.Key);
                    temp.Add(entry.Key);
                    break;
                }
            }
        }
        if (temp.Count > 0) {
            foreach (KeyValuePair<int, GameObject> entry in _roomList) {
                if (!temp.Contains(entry.Key)) {
                    temp2.Add(entry.Key);
                    Debug.Log("Temp2.Add: " + entry.Key);
                }
            }
        }

        for (int i = 0; i < temp2.Count - 1; i++) {
            Debug.Log("Removing non-existing rooms");
            Destroy(_roomList[temp2[i]].gameObject);
            _roomList.Remove(temp2[i]);
        }

        foreach (KeyValuePair<int, GameObject> entry in _roomList) {
            Debug.Log("Updating room #" + entry.Key);
            _roomList[entry.Key].GetComponent<RoomInfo>().amountOfPlayers = _hostData[entry.Key].connectedPlayers;
            _roomList[entry.Key].GetComponent<RoomPanelUI>().UpdatePanel();
        }
    }

    public void Join(int i) {
        Debug.Log("Connecting to host...");
        Network.Connect(_hostData[i]);
        // TODO: Replace with room's map
        Application.LoadLevel("new");
    }

}
