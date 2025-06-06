using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float playerHealth;
    public float startHealth = 100;
    public HealthBar healthBar;
    public SpriteRenderer sr;
    private PlayerMovement m;

    // Start is called before the first frame update
    void Awake()
    {
        playerHealth = startHealth;
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        m = GetComponent<PlayerMovement>();
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

    //update UI elements to match new health stat
    public void UpdateUI()
    {
        healthBar.SetHealth((int)playerHealth);

        //check for death condition
        if (playerHealth <= 0)
            PlayerDeath();
    }

    //stop movement and show death animation, then reset
    private void PlayerDeath()
    {
        //player can't move
        //m.movementOn = false;

        //reset game
        //PLACEHOLDER
    }
}
