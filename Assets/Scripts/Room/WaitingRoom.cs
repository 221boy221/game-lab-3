using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Boy Voesten
    // TODO: Make the panels spawn, refresh and remove
    // TODO: Make it grab the _userInfo from the player joining the game.

public class WaitingRoom : MonoBehaviour {

    [SerializeField] private GameObject _playerPanelPref;
    [SerializeField] private Transform _playerList;
    private UserInfo _userInfo;
    private Dictionary<string, string> _allUserInfo = new Dictionary<string, string>();
    private Dictionary<string, GameObject> _allUserPanels = new Dictionary<string, GameObject>();

    public void Disconnect() {
        Network.Disconnect();
    }

    public void OnJoin() {
        _userInfo = GameObject.FindGameObjectWithTag("UserData").GetComponent<UserInfo>();
        _allUserInfo.Add(_userInfo.guid, _userInfo.name);
        RefreshList();
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

    private void CreatePlayerPanel(string username) {
        GameObject panelPref;
        PlayerPanelUI panelUI;

        panelPref = (GameObject)Instantiate(_playerPanelPref);
        panelPref.transform.SetParent(_playerList, false);

        panelUI = panelPref.GetComponent<PlayerPanelUI>();
        //panelUI.playerLvl = playerInfo.lvl;
        panelUI.playerName = username;
    }
    
}
