using UnityEngine;
using System.Collections;
[ExecuteInEditMode]

public class NetworkManager : MonoBehaviour {

    private string _serverIP = "172.17.57.28";
    private string _gameName = "CritterCombat";
    private string _lobbyName = "";

    private float _refreshRequestLength = 3.0f;
    private HostData[] _hostData;

    public Transform playerPrefab;
    public NetworkPlayer myPlayer;


    void Start() {
        MasterServer.ipAddress = _serverIP;
        MasterServer.port = 23466;
        Network.natFacilitatorIP = _serverIP;
        Network.proxyPort = 50005;
    }

    private void StartServer() {
        Network.InitializeServer(16, Random.Range(2000, 2500), !Network.HavePublicAddress());
        MasterServer.RegisterHost(_gameName, _lobbyName);
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
                Transform myCamera = thisPlayer.transform.Find("Camera");
                myCamera.GetComponent<Camera>().enabled = true;
                myCamera.GetComponent<Camera>().GetComponent<AudioListener>().enabled = true;
                break;
            }
        }

    }

    public IEnumerator RefreshHostList() {
        MasterServer.RequestHostList(_gameName);
        float timeEnd = Time.time + _refreshRequestLength;
        while (Time.time < timeEnd) {
            _hostData = MasterServer.PollHostList();
            yield return new WaitForEndOfFrame();
        }
    }

    public void OnGUI() {
        if (Network.isClient || Network.isServer) {
            return;
        }
        if (_lobbyName == "") {
            GUI.Label(new Rect(Screen.width / 2 - Screen.width / 10, Screen.height / 2 - Screen.height / 20, Screen.width / 5, Screen.height / 20), "Lobby Name");
        }

        _lobbyName = GUI.TextField(new Rect(Screen.width / 2 - Screen.width / 10, Screen.height / 2 - Screen.height / 20, Screen.width / 5, Screen.height / 20), _lobbyName, 25);

        if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 10, Screen.height / 2, Screen.width / 5, Screen.height / 10), "Start New Server")) {
            StartServer();
        }
        if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 10, Screen.height / 2 + Screen.height / 10, Screen.width / 5, Screen.height / 10), "Find Servers")) {
            StartCoroutine(RefreshHostList());
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