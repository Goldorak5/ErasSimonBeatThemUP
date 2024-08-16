using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Slider playerSlider;
    public TextMeshProUGUI nameTag;
    public TextMeshProUGUI lives;
    public TextMeshProUGUI zombieKilledCount;
    public TextMeshProUGUI playerName;

    public GameObject enemyUI;
    public Image enemyIcon;
    public Slider enemySlider;
    public TextMeshProUGUI enemyName;

    private PlayerContoller player;
    public float enemyTimer;
    public float enemyUITime;

    void Start()
    {
        player = FindObjectOfType<PlayerContoller>();
        playerSlider.maxValue = player.maxHealth;
        playerSlider.value = player.currentHealth;
        playerName.text = player.playerName;
        lives.text = "Lives: " + player.numLives.ToString();
        zombieKilledCount.text = "Kills: " + player.enemyKillCount.ToString();
    }

    void Update()
    {
        enemyTimer += Time.deltaTime;
        if(enemyTimer > enemyUITime)
        {
            enemyUI.SetActive(false);
            enemyTimer = 0;
        }
    }
    public void UpdateUIPlayer(int amount)
    {
        playerSlider.value = amount;
    }

    public void UpdateEnemyUI(int maxHealth, int currentHealth, string name, Sprite image)
    {
        enemySlider.maxValue = maxHealth;
        enemySlider.value = currentHealth;
        enemyName.text = name;
        enemyIcon.sprite = image;
        enemyTimer = 0;

        enemyUI.SetActive(true);
    }
    public void EnemykilledCount() 
    {
        zombieKilledCount.text = "Kill: " + player.enemyKillCount.ToString();
    }
}
