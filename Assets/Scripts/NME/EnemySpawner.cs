using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    //position that the zombie can walk up and down
    public float maxZ, minZ;
    public GameObject[] enemy;
    public GameObject boss;
    public int numberOfEnemy;
    public float spawnTime;
    private int currentEnemies = 0;
    private float enemyWidth = 1.3f;

    void SpawnEnemy()
    {
        Camera cam = Camera.main;
        float cameraSize = cam.orthographicSize;
        float cameraWidth = 2 * cameraSize * cam.aspect;
        //Spawn in the camera view left right and the z of max and min of the zombies
        float leftBoundary = -cameraWidth / 2 + (enemyWidth / 2);
        float rightBoundary = cameraWidth / 2 - (enemyWidth / 2);
        float randomXSpawn = Random.Range(leftBoundary, rightBoundary);

        Vector3 spawnPosition = new Vector3(randomXSpawn,
                                            5,
                                            Random.Range(minZ, maxZ));
        Instantiate(enemy[Random.Range(0, enemy.Length)],spawnPosition , Quaternion.identity);
        currentEnemies++;
        if(currentEnemies < numberOfEnemy)
        {
            Invoke("SpawnEnemy", spawnTime);
        }
    }

    public void SpawnBoss()
    {
        Camera cam = Camera.main;
        float cameraSize = cam.orthographicSize;
        float cameraWidth = 2 * cameraSize * cam.aspect;
        float rightBoundary = cameraWidth / 2 - (enemyWidth / 2);
        Vector3 spawnPosition = new Vector3(rightBoundary - 5,
                                            5,
                                            minZ + maxZ / 2);
        Instantiate(boss, spawnPosition, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().enabled = false;
            SpawnEnemy();
        }
    }
}