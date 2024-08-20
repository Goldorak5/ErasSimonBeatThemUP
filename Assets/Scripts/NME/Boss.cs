using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : EnemyController
{

    public GameObject handProjectile;
    public float minShootTime;
    public float maxShootTime;
    private MusicController musicController;

    private void Awake()
    {
        Invoke("ThrowAbilities", Random.Range(minShootTime, maxShootTime));
        musicController = FindAnyObjectByType<MusicController>();
        musicController.PlaySong(musicController.BossSong);
    }

    void ThrowAbilities()
    {
        if (!IsDead())
        {
            if (facingRight)
            {
                handProjectile.GetComponent<HandWeapon>().direction = 1;
            }
            else
            {
                handProjectile.GetComponent<HandWeapon>().direction = -1;
            }
            Instantiate(handProjectile, transform.position, transform.rotation);
            Invoke("ThrowAbilities", Random.Range(minShootTime, maxShootTime));
        }
    }

    void BossDefeated()
    {
        musicController.PlaySong(musicController.levelClearSong);    
        Invoke("LoadScene", 6f);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   
    }
}
