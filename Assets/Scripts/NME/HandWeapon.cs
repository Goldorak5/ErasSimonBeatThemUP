using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandWeapon : MonoBehaviour
{
    public int direction = 1;
    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody> ();
       if (direction == 1)
       {
         spriteRenderer.flipX = true;
       }else
         spriteRenderer.flipX = false;
    }
        

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(6 * direction, 0, 0);
    }


}
