using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PlayerController : NetworkBehaviour
{
    // change local player sprite
    public Sprite spriteLocal;

    // mouse movement
    private Vector2 targetPosition;
    enum Direction { TopLeft, Top, TopRight, Right, BottomRight, Bottom, BottomLeft, Left, Center };
    Direction facing;
    float timePerStep = 0.1f;

    // direction 
    private Vector2 heading;
    private Vector2 direction;

    //[SyncVar(hook = "OnSpriteFlip")]
    //float localScale;
    // turn ON fire
    public GameObject spriteObj;

    // prevent change child scale (on flip)
    public GameObject hpBarObj;


    public bool spawnProj;

    public GameObject projectilePrefab;
    public Transform projectileSpawn;
    private GameObject obj2Bdestroyed;

    // Use this for initialization
    void Start()
    {

        if (!isServer)
        {
            CmdSendClientName("client-" + DateTime.Now);
        }
        else
        {
            CmdSendClientName("server-" + DateTime.Now);
        }
        //hpBarObj.transform.SetParent(transform, false);
        transform.SetParent(hpBarObj.transform, false);
        targetPosition = new Vector2(transform.position.x, transform.position.y);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        // PLAYER MOVEMENT
        /*
 if (Input.GetKey(KeyCode.Mouse0))
 {
     targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
 }
 transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5);
 Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(targetPosition.x, targetPosition.y, Camera.main.transform.position.z), Time.deltaTime * 5);

    */
        /*
            timePerStep -= Time.deltaTime;
            if (Input.GetKey(KeyCode.Mouse0))
            {

                if (timePerStep < 0)
                {
                    Step();
                    timePerStep = 0.1f;
                }
            }

            */

        timePerStep -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            // AVOID CLICKS THROUGH INTERFACE
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            if (timePerStep < 0)
            {
                Step();
                timePerStep = 0.1f;
            }
        }
        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(targetPosition.x, targetPosition.y, Camera.main.transform.position.z), Time.deltaTime * 100);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        // SET DIRECTION

        if (mousePos.x > transform.position.x)
        {
            if (mousePos.y > transform.position.y + 0.5f)
            {
                // TOPRIGHT
                //projectileSpawn.transform.position = new Vector3(transform.position.x + 1, transform.position.y + 1, transform.position.z);
                facing = Direction.TopRight;
            }
            else
            {
                if (mousePos.y < transform.position.y - 0.5f)
                {
                    // BOTTOMRIGHT
                    //projectileSpawn.transform.position = new Vector3(transform.position.x + 1, transform.position.y - 1, transform.position.z);
                    facing = Direction.BottomRight;
                }
                else
                {
                    // RIGHT
                    //projectileSpawn.transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
                    facing = Direction.Right;
                }
            }
        }
        else
        {
            if (mousePos.x < transform.position.x - 0.5f)
            {
                if (mousePos.y > transform.position.y + 0.5f)
                {
                    //TOPLEFT
                    //projectileSpawn.transform.position = new Vector3(transform.position.x - 1, transform.position.y + 1, transform.position.z);
                    facing = Direction.TopLeft;
                }
                else
                {
                    if (mousePos.y < transform.position.y - 0.5f)
                    {
                        // BOTTOMLEFT
                        //projectileSpawn.transform.position = new Vector3(transform.position.x - 1, transform.position.y - 1, transform.position.z);
                        facing = Direction.BottomLeft;
                    }
                    else
                    {
                        // LEFT
                        //projectileSpawn.transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
                        facing = Direction.Left;
                    }
                }
            }
            else
            {
                if (mousePos.y > transform.position.y + 0.5f)
                {
                    // TOP
                    //projectileSpawn.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                    facing = Direction.Top;
                }
                else
                {
                    if (mousePos.y < transform.position.y - 0.5f)
                    {
                        //BOTTOM
                        //projectileSpawn.transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
                        facing = Direction.Bottom;
                    }
                    else
                    {
                        facing = Direction.Center;
                        //Debug.Log("dead zone");
                    }
                }
            }
        }


        // FLIP_SPRITE
        if (facing == Direction.BottomRight || facing == Direction.Right || facing == Direction.TopRight)
        {
            CmdFlipSprite(-1);
        }
        else
        {
            CmdFlipSprite(1);
        }
        // END FLIP_SPRITE

        // SPRITE FLIP
        /*
        if (facing == Direction.BottomRight || facing == Direction.Right || facing == Direction.TopRight)
        {
            //OnSpriteFlip(-1);
            CmdFlipSprite(-1);
        }
        else
        {
            //OnSpriteFlip(1);
            CmdFlipSprite(1);
        }

        */
        // PROJECTILE 
        if (Input.GetKey(KeyCode.Space))
        {
            CmdFire(Camera.main.ScreenToWorldPoint(Input.mousePosition), projectileSpawn.transform.position);
        }
        // TRASH
        // Debug.Log(mouseTarget);
    }

    public override void OnStartLocalPlayer()
    {
        //GetComponent<SpriteRenderer>().sprite = spriteLocal;
        spriteObj.GetComponent<SpriteRenderer>().sprite = spriteLocal;
    }


    /*    void CmdFire()
        {
            RpcFire();

        }
       // [ClientRpc]
        void RpcFire()
        */
    [Command]
    void CmdFire(Vector2 mouseTarget, Vector2 projectileSpawnPos)
    {
        obj2Bdestroyed = Instantiate(projectilePrefab, projectileSpawnPos, transform.rotation);
        heading = mouseTarget - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        direction = heading / heading.magnitude;
        obj2Bdestroyed.GetComponent<Rigidbody2D>().velocity = direction * 30;

        // Spawn the bullet on the Clients
        NetworkServer.Spawn(obj2Bdestroyed);

        // destroy afte 2s
        Destroy(obj2Bdestroyed, 2.0f);

    }
    void Step()
    {
        var stepTime = 100;
        var stepSize = 0.25f;
        var stepTimeD = 100;
        var stepSizeD = 0.15f;

        switch (facing)
        {
            case Direction.Top:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + stepSize, transform.position.z), Time.deltaTime * stepTime);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
                break;

            case Direction.Right:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + stepSize, transform.position.y, transform.position.z), Time.deltaTime * stepTime);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
                break;
            case Direction.Bottom:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y - stepSize, transform.position.z), Time.deltaTime * stepTime);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
                break;
            case Direction.Left:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - stepSize, transform.position.y, transform.position.z), Time.deltaTime * stepTime);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
                break;

            // DIAGONAL
            case Direction.TopRight:
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + stepSizeD, transform.position.y + stepSizeD, transform.position.z), Time.deltaTime * stepTimeD);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
                break;
            case Direction.TopLeft:
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - stepSizeD, transform.position.y + stepSizeD, transform.position.z), Time.deltaTime * stepTimeD);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
                break;
            case Direction.BottomRight:
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + stepSizeD, transform.position.y - stepSizeD, transform.position.z), Time.deltaTime * stepTimeD);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
                break;
            case Direction.BottomLeft:
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - stepSizeD, transform.position.y - stepSizeD, transform.position.z), Time.deltaTime * stepTimeD);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
                break;
        }
    }
    void OnSpriteFlip(float localScale)
    {
        CmdFlipSprite(localScale);
        spriteObj.transform.localScale = new Vector3(localScale, 1, 1);
        //transform.localScale = new Vector3(localScale, 1, 1);
    }

    [Command]
    void CmdFlipSprite(float localScale)
    {
        RpcFlipSprite(localScale);
    }
    [ClientRpc]
    void RpcFlipSprite(float localScale)
    {
        //transform.localScale = new Vector3(localScale, 1, 1);
        spriteObj.transform.localScale = new Vector3(localScale, 1, 1);
    }


    [Command]
    void CmdSendClientName(string name)
    {
        RpcSendClientName(name);
    }
    [ClientRpc]
    void RpcSendClientName(string name)
    {
        transform.name = name;
        //Debug.Log(transform.name);
    }
}
