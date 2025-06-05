using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Reset : MonoBehaviour
{
    public GameObject player;
    public GameObject blackScreen;
    private Vector3 resetPosition = new Vector3();
    private Vector2 resetVelocity = new Vector2();
    private bool hasDied;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        resetPosition = player.transform.position;
        resetVelocity = new Vector2(0, 0);
        blackScreen = GameObject.Find("Black Panel");
        hasDied = false;
    }

    //Resets character position to start upon falling
    private void OnTriggerEnter2D(Collider2D collision)
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
    private IEnumerator FadeToBlack(bool fadeOut = true, int fadeSpeed = 2)
    {
        player.GetComponent<PlayerMovement>().PrintAll();
        print("player fell. resetting.");
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
        player.GetComponent<Rigidbody2D>().velocity = resetVelocity;
        player.transform.position = resetPosition;

        //reset player stats
        player.GetComponent<Health>().playerHealth = player.GetComponent<Health>().startHealth;
        player.GetComponent<Health>().UpdateUI();
        player.GetComponent<Stress>().playerStress = player.GetComponent<Stress>().startStress;
        player.GetComponent<Stress>().UpdateUI();

        //fade in
        while (blackScreen.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackScreen.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        hasDied = false;
        player.GetComponent<PlayerMovement>().PrintAll();
        yield break;
    }
}
