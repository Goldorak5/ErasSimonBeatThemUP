using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CharacterState
{
    Idle,
    Walk,
    Attack,
    Jump,
    Dead,
    Immortal
}

public class PlayerContoller : MonoBehaviour
{
    //Caracteristic
    public int maxHealth = 5;
    public int currentHealth = 5;
    public string playerName = "Bruno";
    public Sprite playerIcon;
    public int numLives;
    public int enemyKillCount = 0;
    public int immortalityTime = 3;
    int flashCount = 20;
    float flashDuration = 0.1f;
    Transform deathTransform;

    private Rigidbody rigidBody;
    private SpriteRenderer spriteRenderer;
    public CharacterState characterState = CharacterState.Idle;

    //HitBoxes
    public GameObject hitBoxPunch;
    public GameObject hitBoxKick;
    public GameObject hitBoxJumpKick;

    //jump
    private bool isOnGround;
    private bool canJump;
    public float jumpForce;
    public Transform groundChecker;

    //movement
    public float currentSpeed;
    public float maxSpeed;
    public float deceleration;
    private float inputValueH;
    private float inputValueV;
    private bool facingRight = true;

    //For limit of the player mouvement
    public float minHeight, maxHeight;

    //Animation
    private Animator animator;

    //audio
    public AudioClip[] hurtSound, jumpSound, healthPickUps, deathSound;
    private AudioSource audioSource;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        currentSpeed = maxSpeed;
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        // Ignore collisions between player and enemies
        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("NME");

        Physics.IgnoreLayerCollision(playerLayer, enemyLayer, true);
    }

    void Update()
    {
        //check if player is on the ground
        isOnGround = Physics.Linecast(transform.position, groundChecker.position, 1 << LayerMask.NameToLayer("Ground"));

        if (!IsDead())
        {
            if(Input.GetButtonDown("Jump") && isOnGround)
            {
            canJump = true;
            }
        //get value of h by pressing left and right
        inputValueH = Input.GetAxis("Horizontal");

        //get value of z by pressing up and down
        inputValueV = Input.GetAxis("Vertical");

        //jumping animation
        animator.SetBool("IsOnGround", isOnGround);

            if(Input.GetButtonDown("Fire1"))
            {
            animator.SetTrigger("Attack");
            }
        }else GetComponent<SpriteRenderer>().enabled = false;

    }

    private void FixedUpdate()
    {
        if(!IsDead())
        {

        // player body will move depending of the value horizontal and vertical
        rigidBody.velocity = new Vector3(inputValueH * currentSpeed, 
                                        rigidBody.velocity.y, 
                                        inputValueV * currentSpeed);


        //Play animation walking depending on velocity
        if (isOnGround)
        {
            animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.magnitude));
        }

        //jump
        if (canJump )
        {
            canJump = false;
            rigidBody.AddForce(Vector2.up * jumpForce);
                PlaySFX(jumpSound);
        }

        //flipping the sprite
        if (!facingRight && inputValueH > 0)
        {
            Flip();
        }
        else if (facingRight && inputValueH < 0)
        {
            Flip();
        }

        //limit the movement of the player in the screen
        float minwidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
        float maxwidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;
        rigidBody.position = new Vector3(Mathf.Clamp(rigidBody.position.x, minwidth + 1, maxwidth - 1),
                                         rigidBody.position.y,
                                         Mathf.Clamp(rigidBody.position.z, minHeight, maxHeight));
        }


    }

    void Flip()
    {
        //become the opposite of what it is
        facingRight = !facingRight;
        InvertHitBoxesX();

        if (spriteRenderer.flipX == true )
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    private void InvertHitBoxesX()
    {
        Vector3 thePunchBoxScale = hitBoxPunch.transform.localPosition;
        Vector3 theKickBoxScale = hitBoxKick.transform.localPosition;
        Vector3 theJumpKickBoxScale = hitBoxJumpKick.transform.localPosition;

        thePunchBoxScale.x *= -1;
        theKickBoxScale.x *= -1;
        theJumpKickBoxScale.x *= -1;

        hitBoxPunch.transform.localPosition = thePunchBoxScale;
        hitBoxKick.transform.localPosition = theKickBoxScale;
        hitBoxJumpKick.transform.localPosition = theJumpKickBoxScale;
    }

    void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    //call in idle animation
    void ResetSpeed()
    {
        currentSpeed = maxSpeed;
    }

    public void HandleDamage(int damageAmount)
    {
       if (!IsDead() && characterState != CharacterState.Immortal)
        {
            currentHealth -= damageAmount;
            animator.SetTrigger("GetHit");
            FindObjectOfType<UIManager>().UpdateUIPlayerHealth(currentHealth);
            PlaySFX(hurtSound);
            
            if (currentHealth <= 0)
            {
                characterState = CharacterState.Dead;
                animator.SetBool("IsDead",true);
                PlaySFX(deathSound);
                deathTransform = transform;
                if (numLives > 0)
                {
                    FindObjectOfType<UIManager>().UpdateUIPlayerLives(1);
                }
                else
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                    //if the game is build
                    Application.Quit();
                }
            }
        }
    }

    private void RespawnPlayer()
    {
        //reset the healt
        currentHealth = maxHealth;
        FindObjectOfType<UIManager>().UpdateUIPlayerHealth(currentHealth);

        animator.SetBool("IsDead", false);
        animator.SetTrigger("IsRevived");
        //revive from higher
        gameObject.transform.position = new Vector3(deathTransform.position.x,deathTransform.position.y + 5,deathTransform.position.z);
        StartCoroutine(FlashingSprite());
    }


    IEnumerator FlashingSprite()
    {
        characterState = CharacterState.Immortal;
        for (int i = 0; i <= flashCount; i++)
        {
            //spriteRenderer.enabled = false;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashDuration);

            //spriteRenderer.enabled = true;
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashDuration);
        }
        characterState = CharacterState.Idle;
        spriteRenderer.color = Color.white;
    }
    
    public bool IsDead()
    {
        return characterState == CharacterState.Dead;
    }


    public void PlaySFX(AudioClip[] clip)
    {
        int indexNumber = Random.Range(0, clip.Length - 1);


        audioSource.clip = clip[indexNumber];
        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Healing"))
        {
            currentHealth = maxHealth;
            Destroy(other.gameObject);
            PlaySFX(healthPickUps);
            FindObjectOfType<UIManager>().UpdateUIPlayerHealth(currentHealth);
        }
    }
}
