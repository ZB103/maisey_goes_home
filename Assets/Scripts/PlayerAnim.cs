using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        {
            //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
             //   anim.SetBool("isJumping", true);
            if (Input.GetKey(KeyCode.D))
                anim.SetBool("isRunningRight", true);
            else
                anim.SetBool("isRunningRight", false);
        }
    }
}
