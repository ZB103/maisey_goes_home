using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement pm;
    private Rigidbody2D rb;
    //private float runTime;

    void Start()
    {
        anim = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();

        anim.SetBool("facingRight", true);
        anim.SetBool("isRunning", false);
        anim.SetFloat("runSpeed", pm.moveSpeed/pm.maxMoveSpeed);    //% speed of player movement
        anim.SetFloat("animSpeed", 1);      //speed animation plays at
        anim.SetFloat("fallTime", 0);
        //runTime = 0f;
    }

    private void Update()
    {
        //direction facing
        if (Input.GetKeyDown(KeyCode.D))
            anim.SetBool("facingRight", true);
        if (Input.GetKeyDown(KeyCode.A))
            anim.SetBool("facingRight", false);

        //idle -> running
        //print(Mathf.Abs(rb.velocity.x));
        //if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) { runTime += Time.deltaTime; }
        //if (Mathf.Abs(rb.velocity.x) > 0f) { runTime += Time.deltaTime; }
        //else { runTime = 0f; }
        //if (runTime > 0)
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
        if (rb.velocity.y < -0.2f)
            anim.SetBool("isFalling", true);
        else
            anim.SetBool("isFalling", false);
        if (anim.GetBool("isFalling"))
            anim.SetFloat("fallTime", anim.GetFloat("fallTime") + Time.deltaTime);
        else
            anim.SetFloat("fallTime", 0);
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
