using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int playerHealth;
    public HealthBar healthBar;
    public SpriteRenderer sr;

    // Start is called before the first frame update
    void Awake()
    {
        playerHealth = 100;
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = Color.white;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //check to encounter with heal
        if (collision.gameObject.CompareTag("Heal"))
            Heal();

        //check for encounter with hurt
        else if (collision.gameObject.CompareTag("Hurt"))
            Hurt();
    }

    //add to the player's health
    private void Heal()
    {
        if (playerHealth <= 90)
            playerHealth += 10;
        else if (playerHealth < 100)
            playerHealth = 100;

        UpdateUI();
    }

    //remove from the player's health
    private void Hurt()
    {
        if (playerHealth >= 10)
            playerHealth -= 10;
        else if (playerHealth > 0)
            playerHealth = 0;

        UpdateUI();
    }

    //update UI elements to match new health stat
    private void UpdateUI()
    {
        healthBar.SetHealth(playerHealth);
        sr.color = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), 1);
        print("playerHealth : " + playerHealth);
    }
}
