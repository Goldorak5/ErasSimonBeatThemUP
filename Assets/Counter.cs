using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public int Damage;
    public GameObject bloodEffect;
    public AudioClip punchSound;
    private AudioSource audioSource;
    public EnemyController enemyController;
    BoxCollider boxCollider;
    Vector3 boxCenter;
    Vector3 boxHalfExtent;
    Quaternion boxOrientation;
    Vector3 boxDirection;
    float maxDistance;


    
    void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
        boxCollider = GetComponentInParent<BoxCollider>();

    }

   
    void Update()
    {
        if (gameObject.activeSelf == true)
        {
            boxCenter = boxCollider.center;
            boxHalfExtent = boxCollider.size / 2;
            boxOrientation = boxCollider.transform.rotation;
            maxDistance = boxHalfExtent.magnitude * 2;
            boxDirection = enemyController.transform.forward;
            // Perform the BoxCast
            RaycastHit hitInfo;
            if (Physics.BoxCast(boxCenter, boxHalfExtent, boxDirection, out hitInfo, boxOrientation, maxDistance))
            {
                Debug.Log("BoxCast hit 1 ");
                Debug.Log("BoxCast hit: " + hitInfo.collider.name);
                CounterHit(hitInfo);
            }
        }
    }

    private void CounterHit(RaycastHit other)
    {
        PlayerContoller player = other.collider.transform.GetComponent<PlayerContoller>();

        if (player != null && player.characterState == CharacterState.Counter)
        {
            Debug.Log("Is countered");
            //audioSource.clip = punchSound;
            //audioSource.Play();
            Instantiate(bloodEffect, enemyController.gameObject.transform.position, transform.rotation);
            enemyController.HandleDamage(Damage);
            enemyController.Countered();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        PlayerContoller player = other.GetComponent<PlayerContoller>();

        if (player != null && player.characterState == CharacterState.Counter)
        {
            Debug.Log("Is countered");
            //audioSource.clip = punchSound;
            //audioSource.Play();
            Instantiate(bloodEffect, enemyController.gameObject.transform.position, transform.rotation);
            enemyController.HandleDamage(Damage);
            enemyController.Countered();
        }

    }
}
