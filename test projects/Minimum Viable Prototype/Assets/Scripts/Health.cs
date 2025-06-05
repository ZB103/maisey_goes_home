using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int playerHealth;
    public int startHealth = 100;
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
    //    }
    //    else if (Input.GetKeyDown(KeyCode.Y))
    //    {
    //        Heal();
    //    }
    //}

    //add to player's sensory overload
    public void Hurt()
    {
        if (playerHealth <= 90)
            playerHealth += 10;
        else if (playerHealth < 100)
            playerHealth = 100;

        UpdateUI();
    }

    //remove from the player's sensory overload
    public void Heal()
    {
        if (playerHealth >= 10)
            playerHealth -= 10;
        else if (playerHealth > 0)
            playerHealth = 0;

        UpdateUI();
    }

    //update UI elements to match new health stat
    public void UpdateUI()
    {
        healthBar.SetHealth(playerHealth);

        print("playerHealth : " + playerHealth);

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
