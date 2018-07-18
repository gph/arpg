using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour
{
    public GameObject enemyPrefab;
    public int numberOfEnemies;
    Vector3[] alreadySpawned;

    public override void OnStartServer()
    {
        alreadySpawned = new Vector3[numberOfEnemies];
        alreadySpawned[0] = new Vector3();
        for (int i = 0; i < numberOfEnemies; i++)
        {
            var spawnPosition = new Vector3(
                Random.Range(2, 5),
                Random.Range(1, 3),
                0.0f);
            while (Contains(spawnPosition, alreadySpawned))
            {
                spawnPosition = new Vector3(
                Random.Range(3.0f, 5.0f),
                Random.Range(1.0f, 3.0f),
                0.0f);
            }
            alreadySpawned[i] = spawnPosition;
            var spawnRotation = Quaternion.Euler(
                0.0f,
                0.0f,
                0.0f);

            var enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, spawnRotation);
            NetworkServer.Spawn(enemy);
        }
    }

    bool Contains(Vector3 xyz, Vector3[] array)
    {
        foreach (Vector3 pos in array)
        {
            if (pos == xyz)
            {
                return true;
            }
        }
        return false;
    }
}
