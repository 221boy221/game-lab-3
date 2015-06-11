using UnityEngine;
using System.Collections;

// Boy Voesten

public class RoomCreate : MonoBehaviour {

    private string _roomName;
    private int _maxAmountOfPlayers;

    public void CreateRoom() {
        //Network.InitializeServer(_maxAmountOfPlayers, Random.Range(2000, 2500), !Network.HavePublicAddress());
        Network.InitializeServer(1, Random.Range(2000, 2500), !Network.HavePublicAddress());
        MasterServer.RegisterHost(ServerInfo.serverName, _roomName);
    }


    // GETTETS & SETTERS

    public string roomName {
        set {
            _roomName = value;
        }
    }

    public float maxAmountOfPlayers {
        set {
            _maxAmountOfPlayers =  (int)value;
        }
    }

}
