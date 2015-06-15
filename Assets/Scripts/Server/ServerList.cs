using UnityEngine;
using System.Collections;

// Boy Voesten
    // TODO: Make it update the list every ... amount of time;
    // TODO: Make it so that it adds a panelUI for each created server (Automate the process)

public class ServerList : MonoBehaviour {

    private string _testStatus = "Testing network connection capabilities.";
    private string _testMessage = "Test in progress";
    private string _shouldEnableNatMessage = "";
    public bool _doneTesting = false;
    private bool _probingPublicIP = false;
    public bool connectable = false;
    /*
    private float _timer;
    private float _time;
    private float _refreshRate = 5f;
    ConnectionTesterStatus connectionTestResult = ConnectionTesterStatus.Undetermined;
    */
    
    bool useNat = false;

    void Awake() {
        MasterServer.port = 23466;
        Network.natFacilitatorPort = 50005;
    }

    // Use 'ServerInfo.serverIP' if you want to actually switch server, instead of just the channel.
    // for example, having EU and NA servers that use different hosts to keep the ping to a minimum.
    public void SetServer(string server, string ip) {
        ServerInfo.serverName = server;
        ServerInfo.serverIP = ip;

        MasterServer.ipAddress = ip;
        Network.natFacilitatorIP = ip;

        Debug.Log("Server #" + server + ", IP #" + ip);
        Debug.Log("Masterserver IP: " + MasterServer.ipAddress);

        JoinLobby();
    }
    
    private void JoinLobby() {
        Application.LoadLevel("Lobby");
    }

    // Server connection tests // 

    void OnFailedToConnect(NetworkConnectionError error) {
        Debug.Log("Could not connect to server: " + error);
    }

    void OnFailedToConnectToMasterServer(NetworkConnectionError info) {
        Debug.Log("Could not connect to master server: " + info);
    }

    

    ///
    /// This is for the Online/Offline in the UI
    /// But it doesn't work yet

    /*
    void FixedUpdate() {
        if (Time.time > _time) {
            GetComponentInChildren<ServerPanelUI>().UpdatePanel();
            _time = _refreshRate + Time.time;
        }
    }
    */
    /*
    public void TestConnectionFunc(string server, string ip) {
        Network.GetLastPing(Network.player);
    }
    */
    /*
    public bool TestConnectionFunc(string ip) {
        MasterServer.ipAddress = ip;
        Network.natFacilitatorIP = ip;
        
        connectionTestResult = Network.TestConnection();
        bool connection = false;

        switch (connectionTestResult) {
            case ConnectionTesterStatus.Error:
                _testMessage = "Problem determining NAT capabilities";
                _doneTesting = true;
                //connection = false;
                break;
            
            case ConnectionTesterStatus.Undetermined:
                _testMessage = "Undetermined NAT capabilities";
                _doneTesting = false;
                //connection = false;
                break;
            
            case ConnectionTesterStatus.PublicIPIsConnectable:
                _testMessage = "Directly connectable public IP address.";
                useNat = false;
                _doneTesting = true;
                connection = true;
                break;

            // Check if we can circumvent the blocking by using NAT punchthrough
            case ConnectionTesterStatus.PublicIPPortBlocked:
                _testMessage = "Non-connectable public IP address (port " + Network.proxyPort + " blocked), running a server is impossible.";
                useNat = false;
                connection = false;
                // If no NAT punchthrough test has been performed on this public IP, force a test
                if (!_probingPublicIP) {
                    connectionTestResult = Network.TestConnectionNAT();
                    _probingPublicIP = true;
                    _testStatus = "Testing if blocked public IP can be circumvented";
                    _timer = Time.time + 10;
                }
                // NAT punchthrough test was performed but we still get blocked
                else if (Time.time > _timer) {
                    _probingPublicIP = false; 		// reset
                    useNat = true;
                    _doneTesting = true;
                    connection = false;
                }
                break;

            case ConnectionTesterStatus.PublicIPNoServerStarted:
                _testMessage = "Public IP address but server not initialized, " + "it must be started to check server accessibility. " + "Restart connection test when ready.";
                //connection = false;
                break;

            case ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted:
                _testMessage = "Limited NAT punchthrough capabilities. " + "Cannot connect to all types of NAT servers. " + "Running a server is ill advised as not everyone can connect.";
                useNat = true;
                _doneTesting = true;
                connection = true;
                break;

            case ConnectionTesterStatus.LimitedNATPunchthroughSymmetric:
                _testMessage = "Limited NAT punchthrough capabilities. " + "Cannot connect to all types of NAT servers. " + "Running a server is ill advised as not everyone can connect.";
                useNat = true;
                _doneTesting = true;
                connection = true;
                break;

            case ConnectionTesterStatus.NATpunchthroughAddressRestrictedCone:
            case ConnectionTesterStatus.NATpunchthroughFullCone:
                _testMessage = "NAT punchthrough capable. " + "Can connect to all servers and receive connections from all clients. " + "Enabling NAT punchthrough functionality.";
                useNat = true;
                _doneTesting = true;
                connection = true;
                break;

            default:
                _testMessage = "Error in test routine, got " + connectionTestResult;
                connection = false;
                break;
        }

        if (_doneTesting) {
            if (useNat)
                _shouldEnableNatMessage = "When starting a server the NAT " + "punchthrough feature should be enabled (useNat parameter)";
            else
                _shouldEnableNatMessage = "NAT punchthrough not needed";
            _testStatus = "Done testing";   
        }

        Debug.Log(_testStatus);
        Debug.Log(_testMessage);
        Debug.Log(connection);
        return connection;
    }
     * */

}
