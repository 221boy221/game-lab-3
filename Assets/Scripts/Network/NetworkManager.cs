using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[ExecuteInEditMode]

public class NetworkManager : MonoBehaviour {

    private string _serverIP = "172.17.57.28";
    private string _serverName = "Server 1";
    private string _roomName = "";

    private float _refreshRequestLength = 3.0f;
    private HostData[] _hostData;

    public Transform playerPrefab;
    public NetworkPlayer myPlayer;
    public Transform cameraPrefab;

    public GameObject lobbyPanel;
    public Transform lobbyBrowser;


    void Start() {
        MasterServer.ipAddress = _serverIP;
        MasterServer.port = 23466;
        Network.natFacilitatorIP = _serverIP;
        Network.proxyPort = 50005;
    }

    private void StartServer() {
        Network.InitializeServer(16, Random.Range(2000, 2500), !Network.HavePublicAddress());
        MasterServer.RegisterHost(_serverName, _roomName);
    }

    void OnServerInitialized() {
        if (Network.isServer) {
            myPlayer = Network.player;
            makePlayer(myPlayer);
        }
    }

    void OnConnectedToServer() {
        myPlayer = Network.player;
        GetComponent<NetworkView>().RPC("makePlayer", RPCMode.Server, myPlayer);
    }

    [RPC]
    void makePlayer(NetworkPlayer thisPlayer) {
        Transform newPlayer = Network.Instantiate(playerPrefab, transform.position, transform.rotation, 0) as Transform;
        Transform newCam = Instantiate(cameraPrefab, new Vector3(0,0,-10), transform.rotation) as Transform;
        if (thisPlayer != myPlayer) {
            GetComponent<NetworkView>().RPC("enableCamera", thisPlayer, newPlayer.GetComponent<NetworkView>().viewID);
        } else {
            enableCamera(newPlayer.GetComponent<NetworkView>().viewID);
        }
    }

    [RPC]
    void enableCamera(NetworkViewID playerID) {
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject thisPlayer in players) {
            if (thisPlayer.GetComponent<NetworkView>().viewID == playerID) {
                thisPlayer.GetComponent<Player>().haveControl = true;
                /*Transform myCamera = thisPlayer.transform.Find("Camera");
                myCamera.GetComponent<Camera>().enabled = true;
                myCamera.GetComponent<Camera>().GetComponent<AudioListener>().enabled = true;*/
                break;
            }
        }

    }

    //public IEnumerator RefreshHostList() {
    //    MasterServer.RequestHostList(_serverName);
    //    float timeEnd = Time.time + _refreshRequestLength;
    //    while (Time.time < timeEnd) {
    //        _hostData = MasterServer.PollHostList();
    //        yield return new WaitForEndOfFrame();
    //    }
    //}

    public void RefreshBrowser() {
        MasterServer.RequestHostList(_serverName);
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

    private void CreateRoomPanel(int i) {
        GameObject temp;
        RoomInfo info;
        RoomPanelUI panelUI;

        temp = (GameObject)Instantiate(lobbyPanel);
        temp.transform.SetParent(lobbyBrowser, false);

        info = temp.gameObject.GetComponent<RoomInfo>();
        info.roomNumber = i;
        info.roomName = _hostData[i].gameName;
        info.maxAmountOfPlayers = _hostData[i].playerLimit;
        info.amountOfPlayers = _hostData[i].connectedPlayers;

        panelUI = temp.gameObject.GetComponent<RoomPanelUI>();
        panelUI.UpdatePanel();
    }

    public void ConnectToRoom(HostData room) {
        Network.Connect(room);
    }

    public void OnGUI() {
        if (Network.isClient || Network.isServer) {
            return;
        }
        if (_roomName == "") {
            GUI.Label(new Rect(Screen.width / 2 - Screen.width / 10, Screen.height / 2 - Screen.height / 20, Screen.width / 5, Screen.height / 20), "Room Name");
        }

        _roomName = GUI.TextField(new Rect(Screen.width / 2 - Screen.width / 10, Screen.height / 2 - Screen.height / 20, Screen.width / 5, Screen.height / 20), _roomName, 25);

        if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 10, Screen.height / 2, Screen.width / 5, Screen.height / 10), "Start New Server")) {
            StartServer();
        }
        if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 10, Screen.height / 2 + Screen.height / 10, Screen.width / 5, Screen.height / 10), "Find Servers")) {
            //StartCoroutine(RefreshHostList());
            RefreshBrowser();
        }
        if (_hostData != null) {
            for (int i = 0; i < _hostData.Length; i++) {
                if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 10, Screen.height / 2 + ((Screen.height / 20) * (i + 4)), Screen.width / 5, Screen.height / 20), _hostData[i].gameName)) {
                    Network.Connect(_hostData[i]);
                }
            }
        }
    }
}