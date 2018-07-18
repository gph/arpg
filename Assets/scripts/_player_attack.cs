using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class _player_attack : NetworkBehaviour {

    public GameObject weaponPrefab;
    public Transform weaponPosition;
    private GameObject []obj2Bdestroyed;
    // Use this for initialization
    void Start ()
    {
        obj2Bdestroyed = new GameObject[5];
        for (int i = 0; i < obj2Bdestroyed.Length; i++)
        {
            switch (i)
            {
                case 0:
                    obj2Bdestroyed[i] = Instantiate(weaponPrefab, new Vector3(weaponPosition.position.x - 1, weaponPosition.position.y, weaponPosition.position.z), transform.rotation);
                    break;
                case 1:
                    obj2Bdestroyed[i] = Instantiate(weaponPrefab, new Vector3(weaponPosition.position.x - 0.8f, weaponPosition.position.y, weaponPosition.position.z), transform.rotation);
                    break;
                case 2:
                    obj2Bdestroyed[i] = Instantiate(weaponPrefab, new Vector3(weaponPosition.position.x - 0.6f, weaponPosition.position.y, weaponPosition.position.z), transform.rotation);
                    break;
                case 3:
                    obj2Bdestroyed[i] = Instantiate(weaponPrefab, new Vector3(weaponPosition.position.x - 0.4f, weaponPosition.position.y, weaponPosition.position.z), transform.rotation);
                    break;
                case 4:
                    obj2Bdestroyed[i] = Instantiate(weaponPrefab, new Vector3(weaponPosition.position.x - 0.2f, weaponPosition.position.y, weaponPosition.position.z), transform.rotation);
                    break;
            }
            obj2Bdestroyed[i].transform.parent = gameObject.transform;
            obj2Bdestroyed[i].SetActive(false);
            NetworkServer.Spawn(obj2Bdestroyed[i]);
            
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        CmdAttack();
    }
    [Command]
    void CmdAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < obj2Bdestroyed.Length; i++)
            {
                obj2Bdestroyed[i].SetActive(true);
            }
            

            // destroy afte 2s
            // Destroy(obj2Bdestroyed, 0.5f);
        }
        else
        {
            for (int i = 0; i < obj2Bdestroyed.Length; i++)
            {
                obj2Bdestroyed[i].SetActive(false);
            }
        }
    }
}
