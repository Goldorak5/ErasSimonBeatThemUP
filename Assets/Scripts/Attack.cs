using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int Damage;
    public int counterDamage;
    public GameObject bloodEffect;
    public AudioClip punchSound;
    private AudioSource audioSource;
    public EnemyController enemyController;
    private Vector3 bloodSpawning;
    private bool targetHit = false;
    public float counterStrengh = 5;


    private CameraShake cameraShake;
    public NoiseSettings cameraShakeProfil;

    public float shakeDuration, shakeAmplitude, shakeFrequency;

    public void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
        enemyController = GetComponentInParent<EnemyController>();
        cameraShake = FindAnyObjectByType<CameraShake>();
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
                cameraShake.TriggerShake(shakeDuration, cameraShakeProfil,shakeAmplitude, shakeFrequency);
            }

            if (player != null && !gameObject.CompareTag("Player"))
            {
                if (player.characterState != CharacterState.Counter)
                {
                //regular Hit
                targetHit = true;
                audioSource.clip = punchSound;
                audioSource.Play();
                if (player.facingRight)
                {
                    Instantiate(bloodEffect, player.gameObject.transform.position, transform.rotation);
                }
                else
                {
                    Instantiate(bloodEffect, player.gameObject.transform.position, transform.rotation);
                }
                
                player.HandleDamage(Damage);
                cameraShake.TriggerShake(shakeDuration, player.punchCameraShakeProfil, shakeAmplitude, shakeFrequency);

                }
                else if (!targetHit)
                {
                    //Counter
                    cameraShake.TriggerShake(shakeDuration, player.counterCameraShakeProfil, shakeAmplitude, shakeFrequency);
                    player.CounterAttack();
                        if (player.facingRight)
                        {
                            player.rigidBody.velocity = new Vector3(player.rigidBody.velocity.x + counterStrengh, player.rigidBody.velocity.y,player.rigidBody.velocity.z);
                            Instantiate(bloodEffect, enemyController.gameObject.transform.position, transform.rotation);
                        }
                        else
                        {
                            player.rigidBody.velocity = new Vector3(player.rigidBody.velocity.x - counterStrengh, player.rigidBody.velocity.y, player.rigidBody.velocity.z);
                            Instantiate(bloodEffect, enemyController.gameObject.transform.position, transform.rotation);
                        }
                    enemyController.Countered();
                    audioSource.clip = punchSound;
                    audioSource.Play();
                    
                    enemyController.HandleDamage(counterDamage);
                }
                 targetHit = false;
            }
        
    }

}
