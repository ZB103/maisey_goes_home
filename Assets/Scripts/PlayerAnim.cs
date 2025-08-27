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

        anim.SetBool("facingRight", true);
        anim.SetBool("isRunning", false);
    }

    private void Update()
    {
        //direction facing
        if (Input.GetKeyDown(KeyCode.D))
            anim.SetBool("facingRight", true);
        if (Input.GetKeyDown(KeyCode.A))
            anim.SetBool("facingRight", false);

        //idle -> running
        if (rb.velocity.x != 0)
            anim.SetBool("isRunning", true);
        else
            anim.SetBool("isRunning", false);

        //idle/run -> jumping
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
        {
            anim.SetTrigger("jumpStart");
        }

        //falling
        if (rb.velocity.y < 0)
            anim.SetBool("isFalling", true);
        else
            anim.SetBool("isFalling", false);

        //end fall
        if (pm.isTouchingGround || rb.velocity.y == 0)
        {
            anim.SetTrigger("hitGround");
        }
    }

    private void LateUpdate()
    {
        anim.ResetTrigger("hitGround");
        anim.ResetTrigger("jumpStart");
    }
}
