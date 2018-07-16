using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    // change local player sprite
    public Sprite spriteLocal;

    // mouse movement
    private Vector2 targetPosition;
    enum Direction {TopLeft, Top, TopRight, Right, BottomRight, Bottom, BottomLeft, Left};
    Direction facing;
    float timePerStep = 0.1f;

    // direction 
    private Vector2 heading;
    private Vector2 direction;

    // healthbar direction

    public Canvas healthbar;
    // turn ON fire
    public bool spawnProj;

    public GameObject projectilePrefab;
    public Transform projectileSpawn;
    private GameObject obj2Bdestroyed;

    // Use this for initialization
    void Start()
    {
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
        timePerStep -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            
            if (timePerStep < 0)
            {
                Cmdstep();
                timePerStep = 0.1f;
            }

            /*
            var stepTime = 100;
            if (facing == Direction.Top)
            { 
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Time.deltaTime * stepTime);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
            }
            if (facing == Direction.Right)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), Time.deltaTime * stepTime);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
            }
            if (facing == Direction.Bottom)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Time.deltaTime * stepTime);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
            }
            if (facing == Direction.Left)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), Time.deltaTime * stepTime);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
            }
            */
        }

        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(targetPosition.x, targetPosition.y, Camera.main.transform.position.z), Time.deltaTime * 100);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // SET DIRECTION
        if (mousePos.x > transform.position.x + 0.5) {
            if (mousePos.y > transform.position.y + 0.5)
            {
                // TOPRIGHT
                projectileSpawn.transform.position = new Vector3(transform.position.x + 1, transform.position.y + 1, transform.position.z);
                facing = Direction.TopRight;
            }
            else
            {
                if (mousePos.y < transform.position.y - 0.5)
                {
                    // BOTTOMRIGHT
                    projectileSpawn.transform.position = new Vector3(transform.position.x + 1, transform.position.y -1, transform.position.z);
                    facing = Direction.BottomRight;
                }
                else
                {
                    // RIGHT
                    projectileSpawn.transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
                    facing = Direction.Right;
                }
            }
        }
        else
        {
            if (mousePos.x < transform.position.x - 0.5)
            {
                if (mousePos.y > transform.position.y + 0.5)
                {
                    //TOPLEFT
                    projectileSpawn.transform.position = new Vector3(transform.position.x - 1, transform.position.y + 1, transform.position.z);
                    facing = Direction.TopLeft;
                }
                else
                {
                    if (mousePos.y < transform.position.y - 0.5)
                    {
                        // BOTTOMLEFT
                        projectileSpawn.transform.position = new Vector3(transform.position.x - 1, transform.position.y - 1, transform.position.z);
                        facing = Direction.BottomLeft;
                    }
                    else
                    {
                        // LEFT
                        projectileSpawn.transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
                        facing = Direction.Left;
                    }
                }
            }
            else
            {
                if (mousePos.y > transform.position.y + 0.5)
                {
                    // TOP
                    projectileSpawn.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                    facing = Direction.Top;
                }
                else
                {
                    //BOTTOM
                    projectileSpawn.transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
                    facing = Direction.Bottom;
                }
            }
        }

        if (facing == Direction.BottomRight || facing == Direction.Right || facing == Direction.TopRight)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // PROJECTILE 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire(Camera.main.ScreenToWorldPoint(Input.mousePosition), projectileSpawn.transform.position);
        }

        // TRASH
        // Debug.Log(mouseTarget);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<SpriteRenderer>().sprite = spriteLocal;
    }

    [Command]
    /*    void CmdFire()
        {
            RpcFire();

        }
       // [ClientRpc]
        void RpcFire()
        */
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


    void Cmdstep()
    {
        var stepTime = 50;
        var stepTime2 = 80;

        switch (facing)
        {
            case Direction.Top:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Time.deltaTime * stepTime);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
                break;

            case Direction.Right:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), Time.deltaTime * stepTime);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
                break;
            case Direction.Bottom:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Time.deltaTime * stepTime);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
                break;
            case Direction.Left:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), Time.deltaTime * stepTime);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
                break;

            // DIAGONAL
            case Direction.TopRight:
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + 1, transform.position.y + 1, transform.position.z), Time.deltaTime * stepTime2);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
                break;
            case Direction.TopLeft:
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - 1, transform.position.y + 1, transform.position.z), Time.deltaTime * stepTime2);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
                break;
            case Direction.BottomRight:
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + 1, transform.position.y - 1, transform.position.z), Time.deltaTime * stepTime2);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
                break;
            case Direction.BottomLeft:
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - 1, transform.position.y - 1, transform.position.z), Time.deltaTime * stepTime2);
                targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
                break;
        }
    }
}
