using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Stress pStress;
    private float maxMoveSpeed; //horizontal movement max speed
    public float moveSpeed; //horizontal movement
    private float acc;  //horizontal acceleration
    private float dec;  //horizontal deceleration
    private float maxJumpForce; //vertical movement max force
    public float jumpForce; //vertical movement
    public bool movementOn; //player can move?
    public bool isTouchingGround;    //player is touching the ground?
    private int countTouchingGround;    //how many grounds is the player touching?
    public bool isJumping;         //jump is in progress?
    private float maxCoyoteTime;    //jump allowed for extra time after leaving platform
    private float coyoteTimer;   //timer used to determine whether jump is allowed at that moment
    private float apexFloat;    //amount of velocity adjustment at apex of a jump
    private float descentMultiplier;  //amount of velocity adjustment when falling

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pStress = GetComponent<Stress>();
        maxMoveSpeed = 15;
        moveSpeed = maxMoveSpeed;
        acc = 2f;
        dec = 2.5f;
        maxJumpForce = 30;
        jumpForce = maxJumpForce;
        isTouchingGround = false;
        countTouchingGround = 0;
        isJumping = false;
        Physics2D.gravity = new Vector2(0, -70f);
        movementOn = true;
        maxCoyoteTime = 0.1f;
        apexFloat = .8f;
        descentMultiplier = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (movementOn)
        {
            //right movement
            if (Input.GetKey(KeyCode.D) && rb.velocity.x < moveSpeed)
            {
                //same direction
                if (rb.velocity.x >= 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x + acc, rb.velocity.y);
                }
                //turn around
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x * -0.2f, rb.velocity.y);
                }
            }
            else if (rb.velocity.x > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x - dec, rb.velocity.y);
                if (rb.velocity.x < dec)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }

            //left movement
            if (Input.GetKey(KeyCode.A) && rb.velocity.x > -moveSpeed)
            {
                //same direction
                if (rb.velocity.x <= 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x - acc, rb.velocity.y);
                }
                //turn around
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x * -0.2f, rb.velocity.y);
                }
            }
            else if (rb.velocity.x < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x + dec, rb.velocity.y);
                if(rb.velocity.x > -dec)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }


            //start coyote buffer countdown
            if (isTouchingGround)
                coyoteTimer = maxCoyoteTime;
            //decrement coyote buffer
            coyoteTimer -= Time.deltaTime;

            //execute jump if within buffer and coyote allowance
            if ((isTouchingGround || coyoteTimer > 0f) && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
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
        print("movementOn " + movementOn +
            "\nmoveSpeed " + moveSpeed + 
            "\nisTouchingGround " + isTouchingGround +
            "\ncountTouchingGround " + countTouchingGround + 
            "\nisJumping " + isJumping +
            "\njumpForce " + jumpForce
            );
    }

    //change move/jump stats based on stress levels
    public void UpdateStats()
    {
        if (pStress.playerStress >= 90)
            jumpForce = maxJumpForce / 2;
        else
            jumpForce = maxJumpForce;

        if (pStress.playerStress >= 70)
            moveSpeed = maxMoveSpeed - (pStress.playerStress / 15);
        else
            moveSpeed = maxMoveSpeed;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Platform")
        {
            countTouchingGround--;
            isTouchingGround = countTouchingGround > 0 ? true : false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        //land on platform
        if (collision.gameObject.tag == "Platform")
        {
            countTouchingGround++;
            isTouchingGround = countTouchingGround > 0 ? true : false;
            isJumping = false;
        }
    }
}
