using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ServerPanelUI : MonoBehaviour {

    [SerializeField] private Text _serverStatus;
    [SerializeField] private string _server;
    [SerializeField] private string _ip;
    private bool _isOnline = true;
    private ServerList _serverList;

    void Awake() {
        _serverList = GameObject.FindGameObjectWithTag("ServerList").GetComponent<ServerList>();
        //UpdatePanel();
    }
    /*
    public void UpdatePanel() {
        _serverList.TestConnectionFunc(_server, _ip);
        Debug.Log("UPDATE FUCKIN PANEL");
    }
    */
    private void FixedUpdate() {
        if (_isOnline) {
            _serverStatus.text = "Online";
        } else {
            _serverStatus.text = "Offline";
        }
    }

    public void OnClick() {
        //UpdatePanel();
        if (_isOnline)
            _serverList.SetServer(_server, _ip);
    }
}
