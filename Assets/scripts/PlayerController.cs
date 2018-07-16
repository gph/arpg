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
    enum Direction {Top,Right,Bottom, Left};
    float stepsPerTime = 0.1f;
    Direction facing; 
    // direction 
    private Vector2 heading;
    private Vector2 direction;

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

        
        if (Input.GetKey(KeyCode.Mouse0))
        {
            stepsPerTime -= Time.deltaTime;
            if (stepsPerTime < 0)
            {
                Cmdstep();
                stepsPerTime = 0.5f;
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

        // MOUSE DIRECTION

        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
        {
            //transform.localScale = new Vector3(-1, 1, 1);
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > transform.position.y + 2)
            {
                projectileSpawn.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                facing = Direction.Top;
            }
            else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y < transform.position.y - 2)
            {
                projectileSpawn.transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
                facing = Direction.Bottom;
            }
            else
            {
                projectileSpawn.transform.position = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
                facing = Direction.Right;
            }

        }
        else
        {
            //transform.localScale = new Vector3(1, 1, 1);
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > transform.position.y + 2)
            {
                projectileSpawn.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                facing = Direction.Top;
            }
            else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y < transform.position.y - 2)
            {
                projectileSpawn.transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
                facing = Direction.Bottom;
            }
            else
            {
                projectileSpawn.transform.position = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
                facing = Direction.Left;
            }
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
        obj2Bdestroyed.GetComponent<Rigidbody2D>().velocity = direction * 10;

        // Spawn the bullet on the Clients
        NetworkServer.Spawn(obj2Bdestroyed);

        // destroy afte 2s
        Destroy(obj2Bdestroyed, 2.0f);

    }


    void Cmdstep()
    {
        var stepTime = 50;
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
    }
}
