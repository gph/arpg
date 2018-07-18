using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class _player_movement : NetworkBehaviour
{

    private Vector2 mousePosition;
    private Vector2 heading;
    private Vector2 direction;
    private float cameraSpeed;
    private float movementSpeed;
    private float timeToWalk;

    void Start()
    {
        if (!isServer)
        {
            CmdSendClientName("client-" + netId);
        }
        else
        {
            CmdSendClientName("server-" + netId);
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
        // START_MOVEMENT
        timeToWalk -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            /* SMOOTH ONE
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(mousePosition.x, mousePosition.y, transform.position.z), Time.deltaTime * movementSpeed * 10);
            */
            if (timeToWalk < 0)
            {
                Step();
                timeToWalk = 0.05f;
                
            }          
        }
        // END_MOVEMENT
        if (Input.GetKey(KeyCode.Mouse0))
        {

        }

        // START_FLIP
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

        }
        // END_FLIP
        
        // START_CAMERA_MOVEMENT
        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), Time.deltaTime * cameraSpeed);
        // END_CAMERA_MOVEMENT
    }

    void Step()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        heading = mousePosition - new Vector2(transform.position.x, transform.position.y);
        direction = heading / heading.magnitude;
        transform.Translate(direction.x * movementSpeed, direction.y * movementSpeed, 0);
    }

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
}
