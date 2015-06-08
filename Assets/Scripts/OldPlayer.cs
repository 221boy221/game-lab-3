using UnityEngine;
using System.Collections;

public class OldPlayer : MonoBehaviour 
{
    private float speed;
    private Animator anim;
    public int health;
    bool inRange;
    private OldPlayer enemy;
    private bool attacked;
    private int lowAttack;
    private int midAttack;
    private int lowBlock;
    private int midBlock;
    public AnimatorStateInfo currentBaseState;
    public bool haveControl = false;
    private Rigidbody2D body;
    private NetworkView _networkView;
    float horiz = 0;
	void Start () 
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
        
	}
	void FixedUpdate()
    {
        horiz = Input.GetAxis("Horizontal");
        Vector2 newVelocity = (transform.right * horiz * speed);
        Vector2 myVelocity = body.velocity;
        myVelocity.x = newVelocity.x;

        if (myVelocity != body.velocity)
        {
            if (_networkView.isMine)
            {
                movePlayer(myVelocity);
            }
            else
            {
                GetComponent<NetworkView>().RPC("movePlayer", RPCMode.Server, (Vector3)myVelocity);
            }
        }
    }
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetBool("BlockMid", true);
        }
         else if(Input.GetKeyDown(KeyCode.Q))
        {
            anim.SetBool("BlockLow", true);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            anim.SetBool("BlockLow", false);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            anim.SetBool("BlockMid", false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("AttackLow");
            attacked = true;
            if (attacked == false && enemy != null)
            {
                enemy.damage(10,0);
                Debug.Log(enemy.health);
            }
            attacked = true;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("AttackMid");
            Debug.Log(attacked + ":" + enemy);
            if(attacked == false && enemy != null)
            {
                enemy.damage(10,1);
                Debug.Log(enemy.health);
            }
            attacked = true;
        }
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);


        if (currentBaseState.fullPathHash != lowAttack && currentBaseState.fullPathHash != midAttack)
        {
            attacked = false;
        }
	}
    public void damage(int amount ,int midLow)
    {
        if(midLow == 0 && currentBaseState.fullPathHash != lowBlock)
        {
            health -= amount;
        }
        else if (midLow == 1 && currentBaseState.fullPathHash != midBlock)
        {
            health -= amount;
        }
        Debug.Log(health);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        enemy = other.GetComponent<OldPlayer>();
    }
    void OnTriggerExit2D()
    {
        enemy = null;
    }
    [RPC]
    void movePlayer(Vector3 playerVelocity) {
        GetComponent<Rigidbody2D>().velocity = playerVelocity;
        GetComponent<NetworkView>().RPC("updatePlayer", RPCMode.OthersBuffered, transform.position);
    }

    [RPC]
    void updatePlayer(Vector3 playerPos) {
        transform.position = playerPos;
    }
}
