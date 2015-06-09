using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    int moveSpeed = 8;
    float horiz = 0;
    public bool haveControl = false;
    private float speed;
    private Animator anim;
    public int health;
    private Player enemy;
    private bool attacked;
    private int lowAttack;
    private int midAttack;
    private int lowBlock;
    private int midBlock;
    public AnimatorStateInfo currentBaseState;
    private Rigidbody2D body;
    private NetworkView _networkView;
    void FixedUpdate() {
        if (haveControl) {
            horiz = Input.GetAxis("Horizontal");
            Vector2 newVelocity = (transform.right * horiz * moveSpeed);
            Vector2 myVelocity = GetComponent<Rigidbody2D>().velocity;
            myVelocity.x = newVelocity.x;

            if (myVelocity != GetComponent<Rigidbody2D>().velocity) {
                if (Network.isServer) {
                    movePlayer(myVelocity);
                } else {
                    GetComponent<NetworkView>().RPC("movePlayer", RPCMode.Server, (Vector3)myVelocity);
                }
            }
        }
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