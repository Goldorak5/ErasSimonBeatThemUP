using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private PlayerContoller player;
    public int enemyKill;
    private UIManager hud;
    private EnemySpawner spawner;
    public MoveBattleGround moveBattleGround;
    public Boss boss;
    private bool bossSpawned = false;
    private bool levelAdvanced = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        player = FindObjectOfType<PlayerContoller>();
        hud = FindObjectOfType<UIManager>();
        spawner = FindObjectOfType<EnemySpawner>();
    }

    void Update()
    {
        if (player.currentLives < 0)
        {
            ReplayLevel();
        }
        if (!levelAdvanced)
        {
            AdvanceLevel();
        }
        
        if (!bossSpawned)
        {
           SpawnBoss(boss);
        }
        
    }
    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        player.currentLives = player.maxLives;
        FindObjectOfType<UIManager>().UpdateUIPlayerLives();
    }
    public void EnemyCount()
    {
        enemyKill++;
        hud.EnemykilledCount();
    }
    private void AdvanceLevel()
    {
        if (enemyKill == 5)
        {
            moveBattleGround.AdvanceLevel();
            moveBattleGround.ActivateGo();
            levelAdvanced = true;
        }
        
    }
    private void SpawnBoss(Boss boss)
    {
        if (enemyKill == 10)
        {
            spawner.SpawnBoss();
            bossSpawned = true;
        }
       
    }
}
