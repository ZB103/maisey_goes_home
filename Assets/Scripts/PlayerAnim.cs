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
        anim.SetFloat("runSpeed", pm.moveSpeed/pm.maxMoveSpeed);    //% speed of player movement
        anim.SetFloat("animSpeed", 1);      //speed animation plays at
        anim.SetFloat("fallSpeed", rb.velocity.y);
    }

    private void Update()
    {
        //direction facing
        if (Input.GetKeyDown(KeyCode.D))
            anim.SetBool("facingRight", true);
        if (Input.GetKeyDown(KeyCode.A))
            anim.SetBool("facingRight", false);

        //idle -> running
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            anim.SetBool("isRunning", true);
        else
            anim.SetBool("isRunning", false);

        //idle/run -> jumping
        if (pm.jumpStart)
        {
            pm.jumpStart = false;
            anim.ResetTrigger("hitGround");
            anim.SetTrigger("jumpStart");
        }

        //end fall
        if (anim.GetBool("isFalling") && pm.IsGrounded())
        {
            anim.SetTrigger("hitGround");
        }

        //falling
        anim.SetFloat("fallSpeed", rb.velocity.y);
        if (anim.GetFloat("fallSpeed") < -1f)
            anim.SetBool("isFalling", true);
        else
            anim.SetBool("isFalling", false);
    }

    private void LateUpdate()
    {
        anim.ResetTrigger("hitGround");
        anim.ResetTrigger("jumpStart");
        anim.SetFloat("runSpeed", pm.moveSpeed / pm.maxMoveSpeed);
        
        //runSpeed changes animation speed
        //.4- -> hobble
        //.5-.6 -> run slower
        //.7+ -> run normal
        if ((anim.GetFloat("runSpeed") >= .5f && anim.GetFloat("runSpeed") <= .6f)
            || anim.GetFloat("runSpeed") <= .1f) { anim.SetFloat("animSpeed", .7f); }
        else { anim.SetFloat("animSpeed", 1); }

    }
}
