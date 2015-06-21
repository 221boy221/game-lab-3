using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Boy Voesten
    // TODO: Make the panels spawn, refresh and remove
    // TODO: Make it grab the _userInfo from the player joining the game.

public class WaitingRoom : MonoBehaviour {

    /*
    [SerializeField] private GameObject _playerPanelPref;
    [SerializeField] private Transform _playerList;
    private UserInfo _userInfo;
    private Dictionary<string, string> _allUserInfo = new Dictionary<string, string>();
    private Dictionary<string, GameObject> _allUserPanels = new Dictionary<string, GameObject>();
    */
    private loader _loader;

    void Awake() {
        _loader = GameObject.FindGameObjectWithTag("SceneSwitcher").GetComponent<loader>();
    }

    void OnServerInitialized() {
        Debug.Log("Server initialized and ready");
    }
 
    void OnPlayerConnected(NetworkPlayer player) {
        Debug.Log("Player " + " connected from " + player.ipAddress);
        Debug.Log("Player slots: " + Network.connections.Length + "/" + Network.maxConnections);
        
    }

    public void OnJoin(HostData roomData) {
        Debug.Log("OnJoin");
        StartCoroutine("Game");
    }

    IEnumerator Game() {
        Debug.Log("Waiting...");

        yield return new WaitForSeconds(2f);
        if (Network.isServer) {
            Debug.Log("Server");
        } else if (Network.isClient) {
            Debug.Log("Client");
        }

        Debug.Log("Done");

        if (Network.connections.Length >= Network.maxConnections) {
            Debug.Log("Player Limit reached, ready to start game.");
            _loader.load();
        }
        
    }
    
    public void Disconnect() {
        Network.Disconnect();
        //RefreshList();
    }
    


    /// (CURRENTLY DISABLED, NOT WORKING YET)
    /// Advanced: player panels that contain all connected users their usernames and lvls ///
    /// 


    /*
    public IEnumerator Join(float f) {
        yield return new WaitForSeconds(f);
        _userInfo = GameObject.FindGameObjectWithTag("UserData").GetComponent<UserInfo>();
        _allUserInfo.Add(_userInfo.guid, _userInfo.username);
        foreach (NetworkPlayer player in Network.connections) {
            Debug.Log("Lolyep " + player.guid + " , ");
            CreatePlayerPanel(player.guid);
        }
        for (int i = 0; i < Network.connections.Length -1; i++) {
            Debug.Log("lolnope");
        }
            //CreatePlayerPanel(_userInfo.username);
            //RefreshList();
    }
    
    private void CreatePlayerPanel(string name) {
        GameObject panelPref;
        PlayerPanelUI panelUI;

        
        panelPref = (GameObject)Network.Instantiate(_playerPanelPref, transform.position, transform.rotation, 0);//(GameObject)Instantiate(_playerPanelPref);
        panelPref.transform.SetParent(_playerList, false);

        panelUI = panelPref.GetComponent<PlayerPanelUI>();
        //panelUI.playerLvl = playerInfo.lvl;
        panelUI.playerName = name;
    }
    
    
    public void RefreshList() {
        List<string> temp = new List<string>();
        List<string> temp2 = new List<string>();

        foreach (KeyValuePair<string, string> entry in _allUserInfo) {
            foreach (NetworkPlayer player in Network.connections) {
                if (player.guid == entry.Key) {
                    temp.Add(entry.Key);
                }
            }
        }
        if (temp.Count > 0) {
            foreach (KeyValuePair<string, string> entry in _allUserInfo) {
                if (!temp.Contains(entry.Key)) {
                    temp2.Add(entry.Key);
                }
            }
        }

        for (int i = 0; i < temp2.Count - 1; i++) {
            Destroy(_allUserPanels[temp2[i]].gameObject);
            if (_allUserPanels.ContainsKey(temp2[i])) {
            }
            _allUserInfo.Remove(temp2[i]);
            _allUserPanels.Remove(temp2[i]);
        }
    }
    */
}
