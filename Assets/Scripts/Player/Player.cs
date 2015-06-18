using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    int moveSpeed = 8;
    float horiz = 0;
    public bool haveControl = false;
    private float speed;
    private Animator anim;
    public int health;
    public Player enemy;
    private bool attacked;
    private int lowAttack;
    private int midAttack;
    private int lowBlock;
    private int midBlock;
    public AnimatorStateInfo currentBaseState;
    private Rigidbody2D body;
    private NetworkView _networkView;
    private bool inRange;
    public matchScript match;

    void Start()
    {
        anim = GetComponent<Animator>();
        speed = 5;
        health = 100;
        attacked = false;
        midAttack = Animator.StringToHash("AttackMid");
        lowAttack = Animator.StringToHash("AttackLow");
        lowBlock = Animator.StringToHash("BlockMid");
        midBlock = Animator.StringToHash("BlockLow");
        body = GetComponent<Rigidbody2D>();
        _networkView = GetComponent<NetworkView>();
        enemy = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        match = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<matchScript>();

    }
    void FixedUpdate()
    {
        if (haveControl)
        {
            horiz = Input.GetAxis("Horizontal");
            Vector2 newVelocity = (transform.right * horiz * moveSpeed);
            Vector2 myVelocity = body.velocity;
            myVelocity.x = newVelocity.x;

            if (myVelocity != body.velocity)
            {
                    movePlayer(myVelocity);
                    anim.SetBool("Running", true);
            }
            else
            {
                anim.SetBool("Running", false);
            }
        }
    }
    void Update()
    {
        if (haveControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetTrigger("AttackLow");
                if (attacked == false && inRange)
                {
                    DoDamag(0);
                }
                attacked = true;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                anim.SetTrigger("AttackMid");
                if (attacked == false && inRange)
                {
                    DoDamag(1);
                }
                attacked = true;
            }
            currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
            if (currentBaseState.fullPathHash != lowAttack && currentBaseState.fullPathHash != midAttack)
            {
                attacked = false;
            }
        }
    }
    [RPC]
    private void DoDamag(int midLow)
    {
        enemy.receivingDamag(midLow);
    }
    public void receivingDamag(int midLow)
    {
        if (midLow == 0 && currentBaseState.fullPathHash != lowBlock)
        {
            health -= 10;
        }
        else if (midLow == 1 && currentBaseState.fullPathHash != midAttack)
        {
            health -= 20;
        }
        match.UpdateHealthBar(health);
        //Debug.Log(health + ":" + enemy.health);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inRange = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
        }
    }
    [RPC]
    void movePlayer(Vector3 playerVelocity)
    {
        body.velocity = playerVelocity;
        GetComponent<NetworkView>().RPC("updatePlayer", RPCMode.OthersBuffered, transform.position);
    }
    [RPC]
    void updatePlayer(Vector3 playerPos)
    {
        transform.position = playerPos;
    }
}