using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinCondition : MonoBehaviour
{
    public float jumpForce; //vertical movement
    private Rigidbody2D rb;
    public GameObject blackScreen;
    public GameObject text;
    private bool jumpComplete;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpForce = 15;
        blackScreen = GameObject.Find("Black Panel");
        text = GameObject.Find("Text (TMP)");
        text.SetActive(false);
    }

    //When come in contact with win obj
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Platform")
        {
            jumpComplete = true;
        }
        else if (collision.gameObject.tag == "Finish")
        {
            print("goal achieved.");

            //destroy goal obj
            Destroy(collision.gameObject);

            //disable player movement
            gameObject.GetComponent<Player>().movementOn = false;

            //celebratory jumps
            StartCoroutine(JumpThrice());

            //fade to black
            StartCoroutine(FadeToBlack());
        }
    }

    public IEnumerator JumpThrice()
    {
        for (int i = 0; i < 3; i++)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpComplete = false;
            yield return new WaitUntil(() => jumpComplete);
        }
        yield break;
    }

    public IEnumerator FadeToBlack(bool fadeOut = true, float fadeSpeed = 0.75f)
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
        //display text
        text.SetActive(true);

        yield break;
    }
}
