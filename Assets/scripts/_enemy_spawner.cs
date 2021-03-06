﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class _enemy_spawner : NetworkBehaviour
{

    public GameObject enemyPrefab;
    public int numberOfEnemies;

    public Transform [] spawnablePosition;

    public override void OnStartServer()
    {
        //alreadySpawned = new Vector3[numberOfEnemies];
        //alreadySpawned[0] = new Vector3();

        for (int x = 0; x < spawnablePosition.Length; x++)
        {
            int temp_nEnemies = numberOfEnemies;
            temp_nEnemies = Random.Range(0, numberOfEnemies + 1);
            //Debug.Log(temp_nEnemies);
            for (int i = 0; i < temp_nEnemies; i++)
            {
                var spawnPosition = Vector3.zero;

                switch (i)
                {
                    case 0:
                        spawnPosition = new Vector2(1, 0);
                        break;
                    case 1:
                        spawnPosition = new Vector2(1, 1);
                        break;
                    case 2:
                        spawnPosition = new Vector2(0, 0.5f);
                        break;
                    case 3:
                        spawnPosition = new Vector2(1, 0.5f);
                        break;
                    case 4:
                        spawnPosition = new Vector2(2, 0.5f);
                        break;
                    case 5:
                        spawnPosition = new Vector2(2, 1);
                        break;
                    case 6:
                        spawnPosition = new Vector2(0, 1);
                        break;
                    case 7:
                        spawnPosition = new Vector2(2, 0);
                        break;
                    case 8:
                        spawnPosition = new Vector2(0, 0);
                        break;
                }

                spawnPosition = spawnablePosition[x].position + spawnPosition;
                var spawnRotation = Quaternion.Euler(
                    0.0f,
                    0.0f,
                    0.0f);

                var enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, spawnRotation);
                NetworkServer.Spawn(enemy);
            }
        }

    }
}

