using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    private float jumpForce; //vertical movement
    public bool movementOn; //player can move?
    private float maxCoyoteTime;    //jump allowed for extra time after leaving platform
    public float coyoteTimer;   //timer used to determine whether jump is allowed at that moment
    private float maxJumpBufferTime;    //jump is allowed before hitting the ground
    public float jumpBufferTimer;      //timer used to determine whether jump allowed at that moment
    public bool jumpStart;      //used to trigger jump animation
    private float descentMultiplier;  //amount of velocity adjustment when falling
    private bool right;
    private bool left;
    private bool jump;
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
        Physics2D.gravity = new Vector2(0, -70f);
        movementOn = true;
        maxCoyoteTime = 0.15f;
        maxJumpBufferTime = 0.5f;
        jumpStart = false;
        descentMultiplier = 0.1f;
        right = false;
        left = false;
        jump = false;
    }

    private void Update()
    {
        //key presses checked for in update for consistency
        if (movementOn)
        {
            //right movement
            if (Input.GetKey(KeyCode.D) && rb.velocity.x < moveSpeed) { right = true; }
            else { right = false; }
            //left movement
            if (Input.GetKey(KeyCode.A) && rb.velocity.x > -moveSpeed) { left = true; }
            else { left = false; }

            //coyote buffer countdown
            if (IsGrounded()) { coyoteTimer = maxCoyoteTime; }
            else { coyoteTimer -= Time.deltaTime; }

            //start jump buffer countdown
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
            { jumpBufferTimer = maxJumpBufferTime; }
            else { jumpBufferTimer -= Time.deltaTime; }
            
            //jump
            if (jumpBufferTimer > 0f && coyoteTimer > 0f ) { jumpStart = true; jump = true; }
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W)) { coyoteTimer = 0f; }
        }
    }

    private void FixedUpdate()
    {
        //right movement
        if (right)
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
        if (left)
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

        //execute jump if within jump buffer and coyote allowance
        if (jump)
        {
            jump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferTimer = 0f;
        }

        //reduced gravity at top of jump (apex float)
        if (!IsGrounded() && Mathf.Abs(rb.velocity.y)<= 0.5f)
        {
            rb.gravityScale = 0.3f;
        }
        else if (!IsGrounded())
        {
            rb.gravityScale = 1f;
        }

        //fall quickly
        if (rb.velocity.y < -1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - descentMultiplier);
        }
    }

    //determines whether the player is touching the ground
    public bool IsGrounded()
    {
        return Physics2D.BoxCast(pCollider.bounds.center, pCollider.bounds.size, 0f, Vector2.down, 0.5f, jumpableGround);
    }

    //determines whether the player is just above the ground, for purposes of queueing a jump
    public bool IsAlmostGrounded()
    {
        return Physics2D.BoxCast(pCollider.bounds.center, pCollider.bounds.size, 0f, Vector2.down, 0.85f, jumpableGround);
    }

    //change move/jump stats based on stress levels
    public void UpdateStats()
    {
        if (pStress.playerStress >= 90)
            jumpForce = maxJumpForce * .75f;
        else
            jumpForce = maxJumpForce;

        if (pStress.playerStress >= 40)
            moveSpeed = maxMoveSpeed * ((100-pStress.playerStress) / 100);
        else
            moveSpeed = maxMoveSpeed;
    }
}
