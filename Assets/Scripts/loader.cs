using UnityEngine;
using System.Collections;

public class loader : MonoBehaviour
{
    string[] supportedNetworkLevels = new[] { "mylevel" };
    int lastLevelPrefix = 0;
    NetworkView networkView;

    void Awake()
    {
        // Network level loading is done in a separate channel.
        DontDestroyOnLoad(this);
        networkView = GetComponent<NetworkView>();
        networkView.group = 1;
    }
    public void load ()
    {
        networkView.RPC("LoadLevel", RPCMode.AllBuffered, lastLevelPrefix + 1);
    }
    [RPC]
    IEnumerator LoadLevel(int levelPrefix)
    {
        lastLevelPrefix = levelPrefix;
        
        // There is no reason to send any more data over the network on the default channel,
        // because we are about to load the level, thus all those objects will get deleted anyway
        Network.SetSendingEnabled(0, false);    
        
        // We need to stop receiving because first the level must be loaded first.
        // Once the level is loaded, rpc's and other state update attached to objects in the level are allowed to fire
        Network.isMessageQueueRunning = false;
        
        // All network views loaded from a level will get a prefix into their NetworkViewID.
        // This will prevent old updates from clients leaking into a newly created scene.
        Network.SetLevelPrefix(levelPrefix);
        Application.LoadLevel("new");
        yield return 0;

        // Allow receiving data again
        Network.isMessageQueueRunning = true;
        // Now the level has been loaded and we can start sending out data to clients
        Network.SetSendingEnabled(0, true);

    }
}