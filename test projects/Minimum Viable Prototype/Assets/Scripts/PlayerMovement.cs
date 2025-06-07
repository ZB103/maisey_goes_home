using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Health pHealth;
    private Stress pStress;
    private float maxMoveSpeed; //horizontal movement max speed
    private float moveSpeed; //horizontal movement
    private float maxJumpForce; //vertical movement max force
    private float jumpForce; //vertical movement
    public bool movementOn; //player can move?
    private bool isTouchingGround;    //player is touching the ground?
    private int countTouchingGround;    //how many grounds is the player touching?
    private bool isJumping;         //jump is in progress?
    private float maxCoyoteTime;    //jump allowed for extra time after leaving platform
    private float coyoteTimer;   //timer used to determine whether jump is allowed at that moment
    private float apexFloat;    //amount of velocity adjustment at apex of a jump
    private float descentMultiplier;  //amount of velocity adjustment when falling
    //death and reset
    public GameObject blackScreen;
    private Vector3 resetPosition = new Vector3();
    private Vector2 resetVelocity = new Vector2();
    private bool hasDied;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pHealth = gameObject.GetComponent<Health>();
        pStress = gameObject.GetComponent<Stress>();
        maxMoveSpeed = 10;
        moveSpeed = maxMoveSpeed;
        maxJumpForce = 30;
        jumpForce = maxJumpForce;
        isTouchingGround = false;
        countTouchingGround = 0;
        isJumping = false;
        Physics2D.gravity = new Vector2(0, -70f);
        movementOn = true;
        maxCoyoteTime = 0.1f;
        apexFloat = .8f;
        descentMultiplier = 0.1f;
        resetPosition = gameObject.transform.position;
        resetVelocity = new Vector2(0, 0);
        blackScreen = GameObject.Find("Black Panel");
        hasDied = false;

        InvokeRepeating("CheckForDeath", 0f, 1f);
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

            //execute jump if within buffer and coyote allowance
            if ((isTouchingGround || coyoteTimer > 0f) && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isTouchingGround = false;
                isJumping = true;
            }

            //release jump early
            if (rb.velocity.y > 0f && (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W)))
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.4f);
            }

            //simulate reduced gravity at top of jump
            if (rb.velocity.y <= 0.1f && rb.velocity.y >= -0.1f && isJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - apexFloat);
            }

            //fall quickly
            if (rb.velocity.y < -1f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - descentMultiplier);
            }
        }
    }

    public void PrintAll()
    {
        print("isTouchingGround " + isTouchingGround +
            "\nisJumping " + isJumping + 
            "\nmovementOn " + movementOn + 
            "\njumpForce " + jumpForce +
            "\nmoveSpeed " + moveSpeed);
    }

    //change move/jump stats based on stress levels
    public void UpdateStats()
    {
        if (pStress.playerStress >= 90)
            jumpForce = maxJumpForce / 2;
        else
            jumpForce = maxJumpForce;

        if (pStress.playerStress >= 70)
            moveSpeed = maxMoveSpeed - (pStress.playerStress / 15);
        else
            moveSpeed = maxMoveSpeed;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            countTouchingGround -= 1;
            isTouchingGround = countTouchingGround > 0 ? true : false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //land on platform
        if (collision.gameObject.tag == "Platform")
        {
            countTouchingGround += 1;
            isTouchingGround = countTouchingGround > 0 ? true : false;
            isJumping = false;

            UpdateStats();
        }
    }

    //periodically check if character is dead
    void CheckForDeath()
    {
        if (pHealth.playerHealth <= 0 || gameObject.transform.position.y < -100)
        {
            if (!hasDied)
            {
                StartCoroutine(FadeToBlack());
                hasDied = true;
            }
        }
    }

    //fade to black to reset character
    private IEnumerator FadeToBlack(bool fadeOut = true, int fadeSpeed = 2)
    {
        Color objectColor = blackScreen.GetComponent<Image>().color;
        float fadeAmount;

        //fade out
        while (blackScreen.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackScreen.GetComponent<Image>().color = objectColor;
            yield return null;
        }

        //tp player
        gameObject.GetComponent<Rigidbody2D>().velocity = resetVelocity;
        gameObject.transform.position = resetPosition;

        //reset gameObject stats
        gameObject.GetComponent<Health>().playerHealth = gameObject.GetComponent<Health>().startHealth;
        gameObject.GetComponent<Health>().UpdateUI();
        gameObject.GetComponent<Stress>().playerStress = gameObject.GetComponent<Stress>().startStress;
        gameObject.GetComponent<Stress>().UpdateUI();

        //fade in
        while (blackScreen.GetComponent<Image>().color.a > 0)
        {
            fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackScreen.GetComponent<Image>().color = objectColor;
            yield return null;
        }
        hasDied = false;
        yield break;
    }
}
