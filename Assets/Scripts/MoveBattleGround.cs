using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBattleGround : MonoBehaviour
{
    public GameObject nextLevel;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void ActivateGo()
    {
        GetComponent<Animator>().SetTrigger("Go");
    }

    public void AdvanceLevel()
    {
       // FindObjectOfType<CinemachineConfiner2D>().m_BoundingShape2D = nextLevel.GetComponent<PolygonCollider2D>();
        // Get the CinemachineConfiner2D component
        CinemachineConfiner2D confiner = FindObjectOfType<CinemachineConfiner2D>();

        // Assign the new bounding shape
        PolygonCollider2D newBoundingShape = nextLevel.GetComponent<PolygonCollider2D>();

        if (confiner != null && newBoundingShape != null)
        {
            // Assign the new PolygonCollider2D to the confiner
            confiner.m_BoundingShape2D = newBoundingShape;

            // Force the confiner to recalculate the bounding area
            confiner.enabled = false;
            confiner.enabled =true;
        }
        else
        {
            Debug.LogError("CinemachineConfiner2D or PolygonCollider2D not found!");
        }
    }

    public void PlaySound()
    {
        audioSource.Play();
    }
}
