using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class _player_movement : NetworkBehaviour
{

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

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(mousePos);
        }

    }

}
