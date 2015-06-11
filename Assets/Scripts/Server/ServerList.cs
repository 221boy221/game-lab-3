using UnityEngine;
using System.Collections;

// Boy Voesten
    // TODO: Make it update the list every ... amount of time;
    // TODO: Make it so that it adds a panelUI for each created server (Automate the process)

public class ServerList : MonoBehaviour {

    // Use 'ServerInfo.serverIP' if you want to actually switch server, instead of just the channel.
    // for example, having EU and NA servers that use different hosts to keep the ping to a minimum.
    public void SetServer(int server) {
        switch (server) {
            case 0:
                ServerInfo.serverName = "Server 0";
                //ServerInfo.serverIP = "IP_HERE";
                Debug.Log("Server set to 0");
                break;
            case 1:
                ServerInfo.serverName = "Server 1";
                //ServerInfo.serverIP = "IP_HERE";
                Debug.Log("Server set to 1");
                break;
        }
    }

    public void JoinServer() {
        Application.LoadLevel("Lobby");
    }
}
