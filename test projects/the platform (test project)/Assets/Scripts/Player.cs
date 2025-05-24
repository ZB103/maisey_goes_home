using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed; //horizontal movement
    public float jumpForce; //vertical movement
    private Rigidbody2D rb;
    private bool touchingGround;    //is touching ground? for jump
    public bool movementOn; //player can move?

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingGround = false;
        Physics2D.gravity = new Vector2(0, -75f);
        movementOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        float horizInput = Input.GetAxis("Horizontal");

        //horizontal movement
        if(movementOn)
            rb.velocity = new Vector2(horizInput * moveSpeed, rb.velocity.y);

        //jump
        if (movementOn && touchingGround && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            touchingGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //land on platform
        if(collision.gameObject.tag == "Platform")
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            touchingGround = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        touchingGround = false;
    }
}
