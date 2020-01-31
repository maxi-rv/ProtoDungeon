using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    //COMPONENTS
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private CheckHit checkHit;
    private Collider2D hurtBox;
    private Collider2D pushBox;
    private Material matWhite;
    private Material matDefault;
    
    //VARIABLES
    public int life;
    private float moveSpeed;
    public bool facingUp;
    public bool facingDown;
    public bool facingLeft;
    public bool facingRight;
    

    // Initializes all needed variables and gets references from all the Components and Children Components.
    void Awake()
    {
        // CHILDREN
        GameObject PushBox = gameObject.transform.Find("PushBox").gameObject;
        GameObject HurtBox = gameObject.transform.Find("HurtBox").gameObject;

        // COMPONENTS
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        pushBox = PushBox.GetComponent<Collider2D>();
        hurtBox = HurtBox.GetComponent<Collider2D>();
        checkHit = HurtBox.GetComponent<CheckHit>();

        matWhite = Resources.Load("WhiteSprite", typeof(Material)) as Material;
        matDefault = spriteRenderer.material;

        // VARIABLES
        life = 4;
        moveSpeed = 7f;

        facingDown = true;
        facingUp = false;
        facingLeft = false;
        facingRight = false;

        // ANIMATOR VARIABLES
        animator.SetBool("FacingDown", true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // FixedUpdate is called multiple times per frame.
    void FixedUpdate()
    {
        CheckHit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Checks if the HurtBox has collided with a HitBox from another Object.
    private void CheckHit()
    {
        if(checkHit.isHurt)
        {
            GetHurt();
        }
    }

    // Sets required variables to execute the hurting state.
    private void GetHurt()
    {
        checkHit.SetFalse();
        hurtBox.enabled = false;
        life--;

        // Make a knockback or some hurting effect!!!
        spriteRenderer.material = matWhite;

        if(life<=0)
        {
            KillItself();
        }
        else
        {
            Invoke("StopHurt", 0.1f);
        } 
    }

    // Sets required variables to stop the hurting state.
    public void StopHurt()
    {
        hurtBox.enabled = true;

        spriteRenderer.material = matDefault;
    }


    // On the animator...
    // Sets ONE boolean variable to indicate wich side the player is looking.
    // SetFacingSide should be called first, because this one depends on the facing side.
    private void SetFacingSideAnimator()
    {
        if(facingDown)
        {
            animator.SetBool("FacingDown", true);
            animator.SetBool("FacingUp", false);
            animator.SetBool("FacingSide", false);
        }

        if(facingUp)
        {
            animator.SetBool("FacingDown", false);
            animator.SetBool("FacingUp", true);
            animator.SetBool("FacingSide", false);
        }

        if(facingRight)
        {
            animator.SetBool("FacingDown", false);
            animator.SetBool("FacingUp", false);
            animator.SetBool("FacingSide", true);
        }

        if(facingLeft)
        {
            animator.SetBool("FacingDown", false);
            animator.SetBool("FacingUp", false);
            animator.SetBool("FacingSide", true);
        }
    }

    // On this script...
    // Sets ONE boolean variable to indicate wich side the player is looking.
    private void SetFacingSide(bool down, bool up, bool left, bool right)
    {
        facingDown = down;
        facingUp = up;
        facingLeft = left;
        facingRight = right;
    }

    void KillItself()
    {
        Destroy(gameObject);
    }


}
