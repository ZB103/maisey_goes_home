using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float playerHealth;
    public float startHealth = 100;
    public HealthBar healthBar;
    public int strikesLeft;    //number of times the player has died this attempt
    public int numLives;       //starting number of lives
    private ResetGame resetGame;
    

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = startHealth;
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        resetGame = GetComponent<ResetGame>();
        numLives = 4;
        strikesLeft = numLives;

        UpdateUI();
        StartCoroutine(CheckForDeath());
    }

    //debug controls
    //public void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        Hurt();
    //        print("playerHealth : " + playerHealth);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.Y))
    //    {
    //        Heal();
    //        print("playerHealth : " + playerHealth);
    //    }
    //}

    //subtract from player's health
    public void Hurt()
    {
        if (playerHealth >= 10)
            playerHealth -= 10;
        else if (playerHealth < 10)
            playerHealth = 0;

        UpdateUI();
    }

    //add to player's health
    public void Heal()
    {
        if (playerHealth <= 90)
            playerHealth += 10;
        else if (playerHealth > 90)
            playerHealth = 100;

        UpdateUI();
    }

    //check for death & update UI elements to match new health stat
    public void UpdateUI()
    {
        healthBar.SetHealth((int)playerHealth);
    }

    //Regularly checks for death condition
    public IEnumerator CheckForDeath()
    {
        while (true)
        {
            //check for strike condition
            if (playerHealth <= 0 || transform.position.y < -100)
            {
                strikesLeft--;
                if (strikesLeft <= 0)
                    StartCoroutine(resetGame.FadeToBlack("loss"));
                else
                    StartCoroutine(resetGame.FadeToBlack());
            }

            yield return new WaitForSeconds(1);
        }
    }
}
