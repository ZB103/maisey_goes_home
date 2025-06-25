using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Reset : MonoBehaviour
{
    public GameObject player;
    public GameObject blackScreen;
    private bool hasDied;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        blackScreen = GameObject.Find("Black Panel");
        hasDied = false;
    }

    //Resets character position to start upon falling
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            if (!hasDied)
            {
                StartCoroutine(FadeToBlack());
                hasDied = true;
            }
        }
    }

    //fade to black to reset character
    public IEnumerator FadeToBlack(bool fadeOut = true, int fadeSpeed = 2)
    {
        Color objectColor = blackScreen.GetComponent<Image>().color;
        float fadeAmount;


            print("player death. resetting.");
            while (blackScreen.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackScreen.GetComponent<Image>().color = objectColor;
                yield return null;
            }

            //tp player and fade back in
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            player.transform.position = new Vector3(-60, 20, 0);
            //yield return new WaitForEndOfFrame();
        
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
