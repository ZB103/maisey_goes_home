using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int playerHealth;
    public HealthBar healthBar;
    public ItemSpawner itemS;
    public SpriteRenderer sr;
    private PlayerMovement m;

    // Start is called before the first frame update
    void Awake()
    {
        playerHealth = 100;
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = Color.white;
        m = GetComponent<PlayerMovement>();
        itemS = GameObject.Find("Floor").GetComponent<ItemSpawner>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //invulnerable during death sequence/reset
        if (m.movementOn)
        {
            //check to encounter with heal
            if (collision.gameObject.CompareTag("Heal"))
                Heal();

            //check for encounter with hurt
            else if (collision.gameObject.CompareTag("Hurt"))
                Hurt();
        }
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
        sr.color = new Color(playerHealth / 100f, playerHealth / 100f, playerHealth / 100f, 1);
        print("playerHealth : " + playerHealth);

        //check for death condition
        if (playerHealth <= 0)
            PlayerDeath();
    }

    //stop movement and show death animation, then reset
    private void PlayerDeath()
    {
        //player can't move
        m.movementOn = false;

        //stop spawning new items
        itemS.spawningOn = false;

        //player color changes to indicate death
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        sr.color = new Color(0,.7f, 0, 1);  //Green
        yield return new WaitForSecondsRealtime(0.4f);
        sr.color = new Color(0, 0, 0, 1);  //Black
        yield return new WaitForSecondsRealtime(0.4f);

        sr.color = new Color(.7f, .7f, 0, 1);  //Yellow
        yield return new WaitForSecondsRealtime(0.4f);
        sr.color = new Color(0, 0, 0, 1);  //Black
        yield return new WaitForSecondsRealtime(0.4f);
    
        sr.color = new Color(.7f, 0, 0, 1);  //Red
        yield return new WaitForSecondsRealtime(0.4f);
        sr.color = new Color(0, 0, 0, 1);  //Black
        yield return new WaitForSecondsRealtime(1.2f);

        ResetBoard();
    }

    //reset game after death for game loop
    void ResetBoard()
    {
        //player
        gameObject.transform.position = new Vector2(0, 0);
        playerHealth = 100;
        m.movementOn = true;
        UpdateUI();

        //healers
        foreach(var e in itemS.HealObjList)
        {
            Destroy(e);
        }
        if(itemS.HealObjList.Count > 0)
            itemS.HealObjList.Clear();

        //hurters
        foreach (var e in itemS.HurtObjList)
        {
            Destroy(e);
        }
        if (itemS.HurtObjList.Count > 0)
            itemS.HurtObjList.Clear();

        itemS.spawningOn = true;
    }
}
