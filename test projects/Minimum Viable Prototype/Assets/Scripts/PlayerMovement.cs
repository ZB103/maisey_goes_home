using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed; //horizontal movement
    private float jumpForce; //vertical movement
    public bool movementOn; //player can move?
    private bool isTouchingGround;    //player is touching the ground?
    private bool isJumping;         //jump is in progress?
    private float maxCoyoteTime;    //jump allowed for extra time after leaving platform
    private float coyoteTimer;   //timer used to determine whether jump is allowed at that moment
    private bool inJumpBuffer;  //player is within jump buffer range?
    private float apexFloat;    //amount of velocity adjustment at apex of a jump
    private float descentMultiplier;  //amount of velocity adjustment when falling

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = 10;
        jumpForce = 30;
        isTouchingGround = false;
        isJumping = false;
        inJumpBuffer = true;
        Physics2D.gravity = new Vector2(0, -70f);
        movementOn = true;
        maxCoyoteTime = 0.1f;
        apexFloat = .8f;
        descentMultiplier = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        float horizInput = Input.GetAxis("Horizontal");
        if (movementOn)
        {
            //horizontal movement
            rb.velocity = new Vector2(horizInput * moveSpeed, rb.velocity.y);

            //start coyote buffer countdown
            if (isTouchingGround)
                coyoteTimer = maxCoyoteTime;
            //decrement coyote buffer
            coyoteTimer -= Time.deltaTime;

            //execute jump if within buffer and coyote allowance
            if ((isTouchingGround || inJumpBuffer || coyoteTimer > 0f) && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isTouchingGround = false;
                isJumping = true;
            }

            //release jump early
            if (rb.velocity.y > 0f && (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W)))
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.4f);
            }

            //simulate reduced gravity at top of jump
            if (rb.velocity.y <= 0.1f && rb.velocity.y >= -0.1f && isJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - apexFloat);
            }

            //fall quickly
            if (rb.velocity.y < -1f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - descentMultiplier);
            }
        }
    }

    public void PrintAll()
    {
        print("isTouchingGround " + isTouchingGround +
            "\nisJumping " + isJumping + 
            "\nmovementOn " + movementOn);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //land on platform
        if (collision.gameObject.tag == "Platform")
        {
            isTouchingGround = true;
            isJumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Platform")
        {
            isTouchingGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            inJumpBuffer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Platform")
        {
            inJumpBuffer = false;
        }
    }
}
