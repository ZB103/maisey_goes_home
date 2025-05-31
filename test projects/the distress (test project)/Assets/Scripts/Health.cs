using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 100;
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
        print("healed player : " + playerHealth);
    }

    //remove from the player's health
    private void Hurt()
    {
        if (playerHealth >= 10)
            playerHealth -= 10;
        else if (playerHealth > 0)
            playerHealth = 0;
        print("hurt player : " + playerHealth);
    }
}
