using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Stress pStress;
    public BoxCollider2D pCollider;
    public float maxMoveSpeed; //horizontal movement max speed
    public float moveSpeed; //horizontal movement
    private float acc;  //horizontal acceleration
    private float dec;  //horizontal deceleration
    private float maxJumpForce; //vertical movement max force
    public float jumpForce; //vertical movement
    public bool movementOn; //player can move?
    public bool isTouchingGround;    //player is touching the ground?
    public bool isJumping;         //jump is in progress?
    private float maxCoyoteTime;    //jump allowed for extra time after leaving platform
    private float coyoteTimer;   //timer used to determine whether jump is allowed at that moment
    private float descentMultiplier;  //amount of velocity adjustment when falling
    [SerializeField] private LayerMask jumpableGround;  //layer of ground able to be jumped from

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pStress = GetComponent<Stress>();
        pCollider = GetComponent<BoxCollider2D>();
        maxMoveSpeed = 5;
        moveSpeed = maxMoveSpeed;
        acc = 2f;
        dec = 2.5f;
        maxJumpForce = 15;
        jumpForce = maxJumpForce;
        isTouchingGround = false;
        isJumping = false;
        Physics2D.gravity = new Vector2(0, -70f);
        movementOn = true;
        maxCoyoteTime = 0.1f;
        descentMultiplier = 0.1f;
    }

    private void FixedUpdate()
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
                if (rb.velocity.x > -dec)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }


            //start coyote buffer countdown
            if (IsGrounded())
                coyoteTimer = maxCoyoteTime;
            //decrement coyote buffer
            coyoteTimer -= Time.deltaTime;

            //execute jump if within buffer and coyote allowance
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && (IsGrounded() || coyoteTimer > 0f))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isJumping = true;
            }

            //release jump early
            if (rb.velocity.y > 0f && (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W)))
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.4f);
            }

            //reduced gravity at top of jump (apex float)
            if (!(IsGrounded()) && Mathf.Abs(rb.velocity.y)<= 0.5f)
            {
                rb.gravityScale = 0.1f;
            }
            else if (!(IsGrounded()))
            {
                rb.gravityScale = 1f;
            }

            //fall quickly
            if (rb.velocity.y < -1f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - descentMultiplier);
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(pCollider.bounds.center, pCollider.bounds.size, 0f, Vector2.down, 0.5f, jumpableGround);
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
}
