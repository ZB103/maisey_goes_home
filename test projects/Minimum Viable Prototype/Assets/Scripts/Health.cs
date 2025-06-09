using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float playerHealth;
    public float startHealth = 100;
    public HealthBar healthBar;
    private PlayerMovement m;
    public GameObject blackScreen;
    public GameObject deathScreen;
    private Stack<Vector3> spawnLocation = new Stack<Vector3>();
    private Vector2 resetVelocity = new Vector2();
    public int strikesLeft;    //number of times the player has died this attempt
    public int numLives;       //starting number of lives

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = startHealth;
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        m = GetComponent<PlayerMovement>();
        blackScreen = GameObject.Find("Black Panel");
        deathScreen = GameObject.Find("Death Screen");
        deathScreen.SetActive(false);
        spawnLocation.Push(gameObject.transform.position);
        resetVelocity = new Vector2(0, 0);
        numLives = 3;
        strikesLeft = numLives;

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
        m.UpdateStats();

        //check for strike condition
        if (playerHealth <= 0 || m.transform.position.y < -100)
        {
            strikesLeft--;
            StartCoroutine(FadeToBlack());
        }
    }

    public void ResetPlayer()
    {
        deathScreen.SetActive(false);
        StartCoroutine(FadeFromBlack());

        //reset player stats
        gameObject.GetComponent<Health>().playerHealth = gameObject.GetComponent<Health>().startHealth;
        UpdateUI();
        gameObject.GetComponent<Stress>().playerStress = gameObject.GetComponent<Stress>().startStress;
        gameObject.GetComponent<Stress>().UpdateUI();
        m.movementOn = true;
        strikesLeft = numLives;

        //reset spawn point
        while(spawnLocation.Count > 1)
        {
            spawnLocation.Pop();
        }

        //tp player
        gameObject.GetComponent<Rigidbody2D>().velocity = resetVelocity;
        gameObject.transform.position = spawnLocation.Peek();
    }

    //set spawn point
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Respawn")
        {
            spawnLocation.Push(collision.transform.position);
        }
    }

    //Regularly checks for death condition
    public IEnumerator CheckForDeath()
    {
        while (true)
        {
            UpdateUI();
            yield return new WaitForSeconds(1);
        }
    }

    //fade out
    IEnumerator FadeToBlack(int fadeSpeed = 2)
    {
        m.movementOn = false;
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

        //if player has used all lives, go to death screen
        //otherwise spawn at last spawn point
        if(strikesLeft <= 0)
            deathScreen.SetActive(true);
        else
            StartCoroutine(FadeFromBlack());
        yield break;
    }

    //fade in
    public IEnumerator FadeFromBlack(int fadeSpeed = 2)
    {
        //tp player
        gameObject.GetComponent<Rigidbody2D>().velocity = resetVelocity;
        gameObject.transform.position = spawnLocation.Peek();
        //reset player stats
        gameObject.GetComponent<Health>().playerHealth = gameObject.GetComponent<Health>().startHealth;
        UpdateUI();
        gameObject.GetComponent<Stress>().playerStress = gameObject.GetComponent<Stress>().startStress;
        gameObject.GetComponent<Stress>().UpdateUI();


        Color objectColor = blackScreen.GetComponent<Image>().color;
        float fadeAmount;

        //fade in
        while (blackScreen.GetComponent<Image>().color.a > 0)
        {
            fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackScreen.GetComponent<Image>().color = objectColor;
            yield return null;
        }
        m.movementOn = true;
        yield break;
    }
}
