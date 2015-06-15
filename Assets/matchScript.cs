using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class matchScript : MonoBehaviour {

    bool countdown;
    public Slider playerOneHealth;
    public Slider playerTwoHealth;
    public Text countdowntText;
    public Transform playerOne;
    public Transform playerTwo;
    private float timeLeft;
    public Transform playerPrefab;
    public NetworkPlayer myPlayer;
    GameObject[] players;
	void Start () 
    {
        countdown = true;
        timeLeft = 3;
        MasterServer.ipAddress = ServerInfo.serverIP;
        MasterServer.port = 23466;
        Network.natFacilitatorIP = ServerInfo.serverIP;
        Network.proxyPort = 50005;
	}
    void OnServerInitialized()
    {
        if (Network.isServer)
        {
            myPlayer = Network.player;
            makePlayer(myPlayer);
        }
    }

    void OnConnectedToServer()
    {
        myPlayer = Network.player;
        GetComponent<NetworkView>().RPC("makePlayer", RPCMode.Server, myPlayer);
    }
	void Update () 
    {
	    if(countdown)
        {
            timeLeft -= Time.deltaTime;
            if(timeLeft > 0)
            {
                countdowntText.text = Mathf.RoundToInt(timeLeft).ToString();
            }
            else if(timeLeft >-0.5f)
            {
                countdowntText.text = "Start";
            }
            else
            {
                StartGame();
            }
        }
	}
    [RPC]
    void makePlayer(NetworkPlayer thisPlayer)
    {
        if (Network.isServer)
        {
            Vector3 startPos = new Vector3(-7, -3, 0);
            playerOne = Network.Instantiate(playerPrefab, startPos, transform.rotation, 0) as Transform;
        }
        else
        {
            Vector3 startPos = new Vector3(7, -3, 0);
            playerTwo = Network.Instantiate(playerPrefab, startPos, transform.rotation, 0) as Transform;
            playerTwo.localScale = new Vector3(-playerTwo.localScale.x, playerTwo.localScale.y, playerTwo.localScale.z);
        }
        
        
    }
    void StartGame()
    {
        countdowntText.gameObject.SetActive(false);
        countdown = false;
        if (Network.isServer)
        {
            playerOne.GetComponent<Player>().haveControl = true;
            playerOne.GetComponent<Player>().enemy = playerOne.GetComponent<Player>();
        }
        else 
        {
            playerTwo.GetComponent<Player>().haveControl = true;
            playerTwo.GetComponent<Player>().enemy = playerTwo.GetComponent<Player>();
        }
        playerOne.GetComponent<Player>().enemy = playerTwo.GetComponent<Player>();
        playerTwo.GetComponent<Player>().enemy = playerOne.GetComponent<Player>();
    }
    [RPC]
    public void UpdateHealthBar()
    {
        playerOneHealth.value = playerOne.GetComponent<Player>().health;
        playerTwoHealth.value = playerTwo.GetComponent<Player>().health;
    }
}
