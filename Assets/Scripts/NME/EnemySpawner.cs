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

        // Calculate the world positions of the camera's left and right edges
        float leftBoundary = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane)).x + (enemyWidth / 2);
        float rightBoundary = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane)).x - (enemyWidth / 2);

        // Generate a random x position within the camera's view bounds
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
