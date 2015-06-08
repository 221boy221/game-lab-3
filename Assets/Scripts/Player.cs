using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    private float speed;
    private Animator anim;
    public int health;
    bool inRange;
    private Player enemy;
    private bool attacked;
    private int lowAttack;
    private int midAttack;
    private int lowBlock;
    private int midBlock;
    public AnimatorStateInfo currentBaseState;
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
	}
	
	void Update () 
    {
	    if(Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * Time.deltaTime * speed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * Time.deltaTime * 5;
        }
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
        enemy = other.GetComponent<Player>();
    }
    void OnTriggerExit2D()
    {
        enemy = null;
    }
}
