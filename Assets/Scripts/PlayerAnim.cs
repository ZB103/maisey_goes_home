using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        anim.SetFloat("runSpeed", pm.moveSpeed/pm.maxMoveSpeed);
    }

    private void Update()
    {
        //direction facing
        if (Input.GetKeyDown(KeyCode.D))
            anim.SetBool("facingRight", true);
        if (Input.GetKeyDown(KeyCode.A))
            anim.SetBool("facingRight", false);

        //idle -> running
        if (Mathf.Abs(rb.velocity.x) < 0.1)
            anim.SetBool("isRunning", false);
        else
            anim.SetBool("isRunning", true);

        //idle/run -> jumping
        if (pm.jumpStart)
        {
            pm.jumpStart = false;
            anim.SetTrigger("jumpStart");
        }

        //end fall
        if (anim.GetBool("isFalling") && pm.IsGrounded())
        {
            anim.SetTrigger("hitGround");
        }

        //falling
        if (rb.velocity.y < -.2f)
            anim.SetBool("isFalling", true);
        else
            anim.SetBool("isFalling", false);
    }

    private void LateUpdate()
    {
        anim.ResetTrigger("hitGround");
        anim.ResetTrigger("jumpStart");
        anim.SetFloat("runSpeed", pm.moveSpeed / pm.maxMoveSpeed);
    }
}
