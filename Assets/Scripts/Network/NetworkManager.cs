using UnityEngine;
using System.Collections;
[ExecuteInEditMode]

// Boy Voesten & Beau Arthurs
    // TODO: Put the spawning of things in a different script
    // TODO: Maybe merge with ServerInfo.cs

public class NetworkManager : MonoBehaviour {

    //private float _refreshRequestLength = 3.0f;
    public Transform playerPrefab;
    public NetworkPlayer myPlayer;

    void Start()
    {
        MasterServer.ipAddress = ServerInfo.serverIP;
        MasterServer.port = 23466;
        Network.natFacilitatorIP = ServerInfo.serverIP;
        Network.proxyPort = 50005;
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
               

                /*Transform myCamera = thisPlayer.transform.Find("Camera");
                myCamera.GetComponent<Camera>().enabled = true;
                myCamera.GetComponent<Camera>().GetComponent<AudioListener>().enabled = true;*/
                break;
            }
        }
    }

}