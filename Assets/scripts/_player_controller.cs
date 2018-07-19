using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class _player_controller : NetworkBehaviour
{
    // START->PLAYER_MOVEMENT
    private Vector2 mousePosition;
    private Vector2 heading;
    private Vector2 direction;
    private float movementSpeed;
    private float timeToWalk;

    private bool walkable;
    public BoxCollider2D[] directions;
    // END->PLAYER_MOVEMENT

    // START->CAMERA_MOVEMENT
    private float cameraSpeed;
    // END->CAMERA_MOVEMENT

    // START->ATTACK
    public GameObject attackPrefab;
    public Transform attackPosition;
    // END->ATTACK

    void Start()
    {
        if (isServer)
        {
            CmdSendClientName("server-" + netId);
        }
        else
        {
           CmdSendClientName("client-" + netId);
        }
        mousePosition = Vector3.zero;
        movementSpeed = 0.1f;
        timeToWalk = 0.05f;

        cameraSpeed = 100;
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        // START->PLAYER_MOVEMENT
        timeToWalk -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            /* SMOOTH ONE
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(mousePosition.x, mousePosition.y, transform.position.z), Time.deltaTime * movementSpeed * 10);
            */

            //if (timeToWalk < 0 && ObjClicked("Ground"))
            if (timeToWalk < 0)
            {
                Step();
                timeToWalk = 0.05f;

            }
        }// END->PLAYER_MOVEMENT

        // START->PLAYER_FLIP
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x + 0.5f)
        {
            CmdFlipSprite(1);
        }
        else if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x - 0.5f)
        {
            CmdFlipSprite(-1);
        }
        else
        {

        }// END->PLAYER_FLIP

        // START->PLAYER_ATTACK
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdAttack(Camera.main.ScreenToWorldPoint(Input.mousePosition), attackPosition.transform.position);
        }// END->PLAYER_ATTACK

        // START->CAMERA_MOVEMENT
        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), Time.deltaTime * cameraSpeed);
        // END->CAMERA_MOVEMENT
    }
    // START->PLAYER_MOVEMENT
    void Step()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        heading = mousePosition - new Vector2(transform.position.x, transform.position.y);
        direction = heading / heading.magnitude;
        transform.Translate(direction.x * movementSpeed, direction.y * movementSpeed, 0);
    }

    
    /*
     * 
     * void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            walkable = true;
        }
    }
    */
    /* // RAYCAST check collider with mouse click
    bool ObjClicked(string tagName)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), - Vector2.up);
        if (!Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up))
        {
            return false;
        }
        if (hit.collider.transform.tag == tagName)
        {
            Debug.Log(hit.collider.transform.tag);
            return true;
        }
        else
        {
            return false;
        }
    }
    */
    // END->PLAYER_MOVEMENT

    // START->PLAYER_FLIP
    [Command]
    void CmdFlipSprite(int x)
    {
        RpcFlipSprite(x);
    }
    [ClientRpc]
    void RpcFlipSprite(int x)
    {
        transform.localScale = new Vector3(x, 1, 1);
    }
    // END->PLAYER_FLIP

    // START->SEND_PLAYER_NAME
    [Command]
    void CmdSendClientName(string name)
    {
        RpcSendClientName(name);
    }
    [ClientRpc]
    void RpcSendClientName(string name)
    {
        transform.name = name;
    }
    // END->SEND_PLAYER_NAME

    // START->PLAYER_ATTACK
    [Command]
    void CmdAttack(Vector2 mouseTarget, Vector2 attackPosition)
    {
        GameObject atkObj = null;
        atkObj = Instantiate(attackPrefab, attackPosition, transform.rotation);
        atkObj.transform.parent = gameObject.transform;
        heading = mouseTarget - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        direction = heading / heading.magnitude;
        atkObj.GetComponent<Rigidbody2D>().velocity = direction * 30;

        // Spawn the bullet on the Clients
        NetworkServer.Spawn(atkObj);
        // destroy afte 2s
        Destroy(atkObj, 2.0f);
        
    }
    // END->PLAYER_ATTACK
}
