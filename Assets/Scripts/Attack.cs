using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int Damage;
    public GameObject bloodEffect;
    public AudioClip punchSound;
    private AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        PlayerContoller player = other.GetComponent<PlayerContoller>();

        if (enemy != null && !gameObject.CompareTag("Enemy"))
        {
            audioSource.clip = punchSound;
            audioSource.Play();
            Instantiate(bloodEffect, enemy.gameObject.transform.position, transform.rotation);
            enemy.HandleDamage(Damage);
        }
        if (player != null && !gameObject.CompareTag("Player"))
        {
            audioSource.clip = punchSound;
            audioSource.Play();
            Instantiate(bloodEffect, player.gameObject.transform.position, transform.rotation);
            player.HandleDamage(Damage);
        }
    }

}
