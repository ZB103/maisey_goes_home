using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; //horizontal movement
    public float jumpForce; //vertical movement
    private Rigidbody2D rb;
    public bool movementOn; //player can move?
    private bool isTouchingGround;    //player is touching the ground?
    private float maxCoyoteTime;    //jump allowed for extra time after leaving platform
    private float coyoteTimer;   //timer used to determine whether jump is allowed at that moment
    private float jumpBufferStartTime;    //jump allowed for extra time before landing on platform
    private float jumpBufferTimer;   //timer used to determine whether jump is allowed at that moment

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isTouchingGround = false;
        Physics2D.gravity = new Vector2(0, -75f);
        movementOn = true;
        maxCoyoteTime = 0.1f;
        jumpBufferStartTime = 0.2f;
        jumpBufferTimer = jumpBufferStartTime;
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

            //start jump buffer countdown
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
                jumpBufferTimer = jumpBufferStartTime;
            //decrement jump buffer timer
            else
                jumpBufferTimer -= Time.deltaTime;

            //release jump early
            if (rb.velocity.y > 0f && (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W)))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.35f);
                
                print("abort jump " + jumpBufferTimer);
                jumpBufferTimer = 0f;
            }

            //execute jump if within buffer and coyote allowance
            if (jumpBufferTimer > 0f && coyoteTimer > 0f)
            {
                print("initiate jump " + jumpBufferTimer);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isTouchingGround = false;
                jumpBufferTimer = 0f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //land on platform
        if (collision.gameObject.tag == "Platform")
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            isTouchingGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isTouchingGround = false;
    }
}
