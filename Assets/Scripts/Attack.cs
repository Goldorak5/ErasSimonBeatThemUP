using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int Damage;
    public GameObject bloodEffect;
    private float bloodTimerDestroy;
    private float maxBloodTime = 2f;
    public List<GameObject> bloodEffectsList = new List<GameObject>();
    private bool bloodEnabled = true;
    void Start()
    {
        
    }

    void Update()
    {
        if (bloodEnabled)
        {
         bloodTimerDestroy += Time.deltaTime;
        }

        if (bloodEffectsList.Count > 0)
        {
            if (bloodTimerDestroy > maxBloodTime)
            {
                Destroy(bloodEffectsList[0]);
                bloodEffectsList.RemoveAt(0);
                bloodTimerDestroy = 0f;
            }
        }
        //if there is no blood on the field not try to erase blood
        else bloodEnabled = false;

    }
    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        PlayerContoller player = other.GetComponent<PlayerContoller>();

        if (enemy != null && !gameObject.CompareTag("Enemy"))
        {
            bloodEnabled = true;
            bloodEffectsList.Add(Instantiate(bloodEffect, enemy.gameObject.transform.position, transform.rotation));
            enemy.HandleDamage(Damage);
        }
        if (player != null && !gameObject.CompareTag("Player"))
        {
            bloodEnabled = true;
            bloodEffectsList.Add(Instantiate(bloodEffect, player.gameObject.transform.position, transform.rotation));
            player.HandleDamage(Damage);
        }
    }

}
