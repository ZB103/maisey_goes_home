using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SHG.AnimatorCoder;
using Unity.Burst;

public class PlayerAnims : AnimatorCoder
{
    [SerializeField] private float movementSpeed;
    public static PlayerAnims instance;
    private BoxCollider2D pCollider;
    private Rigidbody2D rb;
    public bool flipAnims = false;  //p2's anims will be flipped, defined by LoadArena.cs
    [SerializeField] private LayerMask jumpableGround;  //layer of ground able to be jumped from

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        jumpableGround = GetComponent<PlayerMovement>().jumpableGround;
        Initialize();
        pCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckJump();
        DefaultAnimation(0);

        void CheckJump()
        {
            if (GetBool(Parameters.GROUNDED))
            {
                if (!flipAnims && Input.GetKeyDown(KeyCode.W)) { Play(new(Animations.JumpR, true, new(Animations.LandR))); }
                else if (flipAnims && Input.GetKeyDown(KeyCode.W)) { Play(new(Animations.JumpL, true, new(Animations.LandL))); }
                
            }
        }
    }

    private void FixedUpdate()
    {
        SetBool(Parameters.GROUNDED, Physics2D.BoxCast(pCollider.bounds.center, pCollider.bounds.size, 0f, Vector2.down, 0.5f, jumpableGround));
        SetBool(Parameters.FALLING, !GetBool(Parameters.GROUNDED) && rb.velocity.y < -25);
        print(rb.velocity.y);
    }

    //basic logic for animations
    public override void DefaultAnimation(int layer)
    {
        //determine direction
        if (Input.GetKey(KeyCode.D)) { flipAnims = false; }
        else if (Input.GetKey(KeyCode.A)) { flipAnims = true; }


        //falling
        if (GetBool(Parameters.FALLING))
        {
            if (!flipAnims) { Play(new(Animations.FallR)); }
            else { Play(new(Animations.FallL)); }
        }

        //forward
        else if (GetBool(Parameters.GROUNDED) && Input.GetKey(KeyCode.D)) { Play(new(Animations.RunR)); }
        //backward
        else if (GetBool(Parameters.GROUNDED) && Input.GetKey(KeyCode.A)) { Play(new(Animations.RunL)); }

        //idle
        else if (GetBool(Parameters.GROUNDED))
        {
            if (!flipAnims) { Play(new(Animations.IdleR)); }
            else { Play(new(Animations.IdleL)); }
        }
    }
}
