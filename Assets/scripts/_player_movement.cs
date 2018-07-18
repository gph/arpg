using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class _player_movement : NetworkBehaviour
{

    private Vector2 mousePosition;
    private int cameraSpeed;
    private int movementSpeed;




    // Use this for initialization
    void Start()
    {
        mousePosition = Vector3.zero;
        cameraSpeed = 100;
        movementSpeed = 10;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!isLocalPlayer)
        {
            return;
        }
        
        if (Input.GetKey(KeyCode.Mouse0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(mousePosition.x, mousePosition.y, transform.position.z), Time.deltaTime * movementSpeed);
        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), Time.deltaTime * cameraSpeed);
    }
}
