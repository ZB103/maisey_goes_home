using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spoons : MonoBehaviour
{
    public int playerStress;
    //public SpoonBar spoonBar;
    public SpriteRenderer sr;
    private PlayerMovement m;

    // Start is called before the first frame update
    void Awake()
    {
        playerStress = 0;
        sr = gameObject.GetComponent<SpriteRenderer>();
        m = GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    //add to player's sensory overload
    public void Hurt()
    {
        if (playerStress <= 90)
            playerStress += 10;
        else if (playerStress < 100)
            playerStress = 100;

        UpdateUI();
    }

    //remove from the player's sensory overload
    public void Heal()
    {
        if (playerStress >= 10)
            playerStress -= 10;
        else if (playerStress > 0)
            playerStress = 0;

        UpdateUI();
    }

    //update UI elements to match new health stat
    private void UpdateUI()
    {
        //spoonBar.SetHealth(playerStress);
        
        print("playerStress : " + playerStress);

        //check for death condition
        if (playerStress <= 0)
            PlayerDeath();
    }

    //stop movement and show death animation, then reset
    private void PlayerDeath()
    {
        //player can't move
        m.movementOn = false;

        //reset game
        //PLACEHOLDER
    }
}
