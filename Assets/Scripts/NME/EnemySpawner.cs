using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    //position that the zombie can walk up and down
    public float maxZ, minZ;
    public GameObject[] enemy;
    public int numberOfEnemy;
    public float spawnTime;
    private int currentEnemies = 0;
    private float enemyWidth = 1.3f;

    void Start()
    {
        
    }

    void Update()
    {


    }

    void SpawnEnemy()
    {
        Camera cam = Camera.main;
        float cameraSize = cam.orthographicSize;
        float cameraWidth = 2 * cameraSize * cam.aspect;
        //Spawn in the camera view left right and the z of max and min of the zombies
        float leftBoundary = -cameraWidth / 2 + (enemyWidth / 2);
        float rightBoundary = cameraWidth / 2 - (enemyWidth / 2);
        //         float leftBoundary = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + (enemyWidth / 2);
        //         float rightBoundary = cam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - (enemyWidth / 2);
        float randomXSpawn = Random.Range(leftBoundary, rightBoundary);

        Vector3 spawnPosition = new Vector3(randomXSpawn,
                                            5,
                                            Random.Range(minZ, maxZ));
        Debug.Log("Enemy spawn");
        Instantiate(enemy[Random.Range(0, enemy.Length)],spawnPosition , Quaternion.identity);
        currentEnemies++;
        if(currentEnemies < numberOfEnemy)
        {
            Invoke("SpawnEnemy", spawnTime);
        }
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
