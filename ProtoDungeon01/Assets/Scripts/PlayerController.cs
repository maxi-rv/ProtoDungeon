using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //COMPONENTS
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private CheckHit checkHit;
    private Collider2D hurtBox;
    private Collider2D pushBox;
    private Collider2D hitBoxUp;
    private Collider2D hitBoxDown;
    private Collider2D hitBoxLeft;
    private Collider2D hitBoxRight;
    public GameObject arrowPrefab;
    private Material matWhite;
    private Material matDefault;
    
    //VARIABLES
    public int life;
    private float moveSpeed;
    private float arrowSpeed;
    private bool facingUp;
    private bool facingDown;
    private bool facingLeft;
    private bool facingRight;
    private bool meleeAttacking;
    private bool rangeAttacking;
    private bool attacking;
    

    // Initializes all needed variables and gets references from all the Components and Children Components.
    void Awake()
    {
        // CHILDREN
        GameObject PushBox = gameObject.transform.Find("PushBox").gameObject;
        GameObject HurtBox = gameObject.transform.Find("HurtBox").gameObject;
        GameObject HitBoxUp = gameObject.transform.Find("HitBoxUp").gameObject;
        GameObject HitBoxDown = gameObject.transform.Find("HitBoxDown").gameObject;
        GameObject HitBoxLeft = gameObject.transform.Find("HitBoxLeft").gameObject;
        GameObject HitBoxRight = gameObject.transform.Find("HitBoxRight").gameObject;

        // COMPONENTS
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        pushBox = PushBox.GetComponent<Collider2D>();
        hurtBox = HurtBox.GetComponent<Collider2D>();
        checkHit = HurtBox.GetComponent<CheckHit>();

        hitBoxUp = HitBoxUp.GetComponent<Collider2D>();
        hitBoxDown = HitBoxDown.GetComponent<Collider2D>();
        hitBoxLeft = HitBoxLeft.GetComponent<Collider2D>();
        hitBoxRight = HitBoxRight.GetComponent<Collider2D>();

        matWhite = Resources.Load("WhiteSprite", typeof(Material)) as Material;
        matDefault = spriteRenderer.material;

        // VARIABLES
        life = 6;
        moveSpeed = 9f;
        arrowSpeed = 21;

        facingDown = true;
        facingUp = false;
        facingLeft = false;
        facingRight = false;

        meleeAttacking = false;
        rangeAttacking = false;

        attacking = false;

        // ANIMATOR VARIABLES
        animator.SetBool("FacingDown", true);
    }

    // Start is called before the first frame update.
    void Start()
    {
    }

    // FixedUpdate is called multiple times per frame.
    void FixedUpdate()
    {
        CheckHit();

        if(!attacking)
        {
            CheckMovement();
        }
        
    }

    // Update is called once per frame.
    // Is executed after FixedUpdate.
    void Update() 
    {
        CheckAttacks();
    }

    // Checks if the HurtBox has collided with a HitBox from another Object.
    private void CheckHit()
    {
        if(checkHit.isHurt)
        {
            GetHurt();
        }
    }

    // Checks Input and trigger animations. 
    private void CheckAttacks()
    {
        //meleeAttacking = Input.GetButtonDown("MeleeAttack");
        rangeAttacking = Input.GetButtonDown("RangeAttack");

        if((meleeAttacking || rangeAttacking) && !attacking)
        {
            Movement(0f, 0f);
            animator.SetBool("Moving", false);

            if(meleeAttacking)
            {
                animator.SetTrigger("MeleeAttacking");
                attacking = true;
            }

            if(rangeAttacking)
            {
                animator.SetTrigger("RangeAttacking");
                attacking = true;
            }

            attacking = true;
        }

    }

    // Checks Input, calls for Movement, and sets facing side.
    private void CheckMovement()
    {
        // Gets Axis Input
        float HorizontalAxis = Input.GetAxisRaw("Horizontal");
        float VerticalAxis = Input.GetAxisRaw("Vertical");

        if(HorizontalAxis == 0f && VerticalAxis == 0f)
        {
            animator.SetBool("Moving", false);
        }
        
        // If Left or Right is NOT pressed.
        if(HorizontalAxis == 0f)
        {
            Movement(0f, VerticalAxis);

            SetFacingSideAnimator();
        }

        // If Up or Down is NOT pressed.
        if (VerticalAxis == 0f)
        {
            Movement(HorizontalAxis, 0f);

            SetFacingSideAnimator();
        }

        // If both Horizontal and Vertical Axis are pressed, it should obey the last pressed Axis.
        if (Input.GetButtonDown("Vertical") && HorizontalAxis != 0f)
        {
            Movement(0f, VerticalAxis);

            if(VerticalAxis>0f)
            {
                SetFacingSide(false, true, false, false);
            }
            else if(VerticalAxis<0f)
            {
                SetFacingSide(true, false, false, false);
            }

            SetFacingSideAnimator();

            animator.SetBool("Moving", true);

            spriteRenderer.flipX = false;
        }
        else if (Input.GetButtonDown("Horizontal") && VerticalAxis != 0f)
        {
            Movement(HorizontalAxis, 0f);

            if(HorizontalAxis>0f)
            {
                SetFacingSide(false, false, false, true);
            }
            else if(HorizontalAxis<0f)
            {
                SetFacingSide(false, false, true, false);
            }

            SetFacingSideAnimator();

            animator.SetBool("Moving", true);

            FlipSprite(HorizontalAxis);
        }
        
        // If ONLY Left or Right is pressed.
        if (HorizontalAxis != 0f && VerticalAxis == 0f)
        {
            Movement(HorizontalAxis, 0f);

            if(HorizontalAxis>0f)
            {
                SetFacingSide(false, false, false, true);
            }
            else if(HorizontalAxis<0f)
            {
                SetFacingSide(false, false, true, false);
            }

            SetFacingSideAnimator();

            animator.SetBool("Moving", true);

            FlipSprite(HorizontalAxis);
        }

        // If ONLY Up or Down is pressed.
        if (VerticalAxis != 0f && HorizontalAxis == 0f)
        {
            Movement(0f, VerticalAxis);

            if(VerticalAxis>0f)
            {
                SetFacingSide(false, true, false, false);
            }
            else if(VerticalAxis<0f)
            {
                SetFacingSide(true, false, false, false);
            }

            SetFacingSideAnimator();

            animator.SetBool("Moving", true);

            spriteRenderer.flipX = false;
        }
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
    
    // Changes the velocity of the Rigidbody according to the values passed (-1f, 0f, or 1f).
    // Because it can recieve 0f as value, this function can stop movement.
    // moveHorizontal : X Value.
    // moveVertical : Y Value.
    private void Movement(float moveHorizontal, float moveVertical)
    {
        Vector2 movementDirection = new Vector2(moveHorizontal, moveVertical);
        rigidBody.velocity = (movementDirection*moveSpeed);
    }

    // Flips the sprite on the X Axis according to the Axis input.
    // Needs to be called by another function, which must check the facing sides first.
    private void FlipSprite(float HorizontalAxis)
    {
        if(HorizontalAxis > 0.1f)
        {
            spriteRenderer.flipX = false;
        }

        if(HorizontalAxis < 0.1f)
        {
            spriteRenderer.flipX = true;
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
        spriteRenderer.material = matDefault;
        hurtBox.enabled = true;
    }

    // Sets required variables to stop the attacking state.
    public void StopAttack()
    {
        attacking = false;
    }

    //...
    public void ShootArrow()
    {
        if(facingUp)
        {
            Quaternion arrowRotation = new Quaternion(0f, 0f, 0f, 0f);
            arrowRotation.eulerAngles = new Vector3(0f, 0f, 0f);
            Vector2 plusVector = new Vector2(0f, 1f);

            GameObject arrow = Instantiate(arrowPrefab, rigidBody.position+plusVector, arrowRotation);
            Rigidbody2D arrowRB = arrow.GetComponent<Rigidbody2D>();
            
            Vector2 arrowDirection = new Vector2(0f, 1f);
            arrowRB.AddForce(arrowDirection*arrowSpeed, ForceMode2D.Impulse);
        }

        if(facingDown)
        {
            Quaternion arrowRotation = new Quaternion(0f, 0f, 0f, 0f);
            arrowRotation.eulerAngles = new Vector3(0f, 0f, 180f);
            Vector2 plusVector = new Vector2(0f, -1f);

            GameObject arrow = Instantiate(arrowPrefab, rigidBody.position+plusVector, arrowRotation);
            Rigidbody2D arrowRB = arrow.GetComponent<Rigidbody2D>();
            
            Vector2 arrowDirection = new Vector2(0f, -1f);
            arrowRB.AddForce(arrowDirection*arrowSpeed, ForceMode2D.Impulse);
        }

        if(facingLeft)
        {
            Quaternion arrowRotation = new Quaternion(0f, 0f, 0f, 0f);
            arrowRotation.eulerAngles = new Vector3(0f, 0f, 90f);
            Vector2 plusVector = new Vector2(-1f, -0.25f);

            GameObject arrow = Instantiate(arrowPrefab, rigidBody.position+plusVector, arrowRotation);
            Rigidbody2D arrowRB = arrow.GetComponent<Rigidbody2D>();
            
            Vector2 arrowDirection = new Vector2(-1f, 0f);
            arrowRB.AddForce(arrowDirection*arrowSpeed, ForceMode2D.Impulse);
        }

        if(facingRight)
        {
            Quaternion arrowRotation = new Quaternion(0f, 0f, 0f, 0f);
            arrowRotation.eulerAngles = new Vector3(0f, 0f, 270);
            Vector2 plusVector = new Vector2(1f, -0.25f);

            GameObject arrow = Instantiate(arrowPrefab, rigidBody.position+plusVector, arrowRotation);
            Rigidbody2D arrowRB = arrow.GetComponent<Rigidbody2D>();
            
            Vector2 arrowDirection = new Vector2(1f, 0f);
            arrowRB.AddForce(arrowDirection*arrowSpeed, ForceMode2D.Impulse);
        }
        
    }

    void KillItself()
    {
        Invoke("StopHurt", 0.1f);

        //DETENER EL JUEGO!!!
    }
}
