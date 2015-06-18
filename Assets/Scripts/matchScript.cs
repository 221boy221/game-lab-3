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
        timeLeft = 30;
        myPlayer = Network.player;
        makePlayer(myPlayer);
        
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
        Vector3 startPos;
        Vector3 scale;
        Transform player;
        if (Network.isServer)
        {
            startPos = new Vector3(-7, -3, 0);
            scale = new Vector3(.4f, .4f, 1);
        }
        else
        {
            startPos = new Vector3(7, -3, 0);
            scale = new Vector3(-.4f, .4f, 1);
        }

        player = Network.Instantiate(playerPrefab, startPos, transform.rotation, 0) as Transform;
        player.localScale = scale;
        Debug.Log(player.GetComponent<NetworkView>());
        player.GetComponent<Player>().haveControl = true;

    }
    [RPC]
    void StartGame()
    {
        countdowntText.gameObject.SetActive(false);
        countdown = false;

        //playerOne.GetComponent<Player>().enemy = playerTwo.GetComponent<Player>();
       //playerTwo.GetComponent<Player>().enemy = playerOne.GetComponent<Player>();
    }
    [RPC]
    public void UpdateHealthBar()
    {
        playerOneHealth.value = playerOne.GetComponent<Player>().health;
        playerTwoHealth.value = playerTwo.GetComponent<Player>().health;
    }
}
