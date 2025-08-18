using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement pm;
    private Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    /* SPACE -> jump -> NEG VEL -> fall (loop) -> IS GROUNDED -> land -> D ? idle : run */
    private void Update()
    {
        //idle/run
        if (Input.GetKey(KeyCode.D))
            anim.SetBool("isRunningRight", true);
        else
            anim.SetBool("isRunningRight", false);

        //idle/run -> jumping
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
            anim.SetTrigger("jumpStart");

        //jumping -> falling
        if (pm.isJumping && rb.velocity.y <= 0)
            anim.SetBool("isFalling", true);

        //falling -> landing
        if (pm.isTouchingGround)
        {
            anim.SetBool("isFalling", false);
            anim.ResetTrigger("jumpStart");
        }
    }
}
