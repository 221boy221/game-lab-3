﻿using UnityEngine;
using System.Collections;

// Boy Voesten
    // TODO: Find a way to keep this information accassible for all other scripts at any time (Don't Destroy on Load?)

public class ServerInfo : MonoBehaviour {

    public static string serverIP = "172.17.57.28";
    public static string serverName = "Server 0";

    public static HostData[] hostData;

}
