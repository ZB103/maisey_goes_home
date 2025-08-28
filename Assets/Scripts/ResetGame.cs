using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResetGame : MonoBehaviour
{
    private Stack<Vector3> spawnLocation = new Stack<Vector3>();
    public Vector2 resetVelocity = new Vector2();
    public GameObject blackScreen;
    //public GameObject deathScreen;
    public GameObject winScreen;
    public GameObject winScreenText;
    private ReachHome winCond;
    private Health pHealth;
    private Stress pStress;
    private PlayerMovement m;

    // Start is called before the first frame update
    void Start()
    {
        spawnLocation.Push(gameObject.transform.position);
        resetVelocity = new Vector2(0, 0);
        blackScreen = GameObject.Find("Black Panel");
        //deathScreen = GameObject.Find("Death Screen");
        winScreen = GameObject.Find("Win Screen");
        winScreenText = GameObject.Find("Win Screen Text");
        //deathScreen.SetActive(false);
        winScreen.SetActive(false);
        winCond = GameObject.Find("Home").GetComponent<ReachHome>();
        pHealth = gameObject.GetComponent<Health>();
        pStress = gameObject.GetComponent<Stress>();
        m = gameObject.GetComponent<PlayerMovement>();
    }

    public void ResetPlayer()
    {
        //deathScreen.SetActive(false);
        winScreen.SetActive(false);
        StartCoroutine(FadeFromBlack());

        //reset player stats
        pHealth.playerHealth = pHealth.startHealth;
        pHealth.UpdateUI();
        pStress.playerStress = pStress.startStress;
        pStress.UpdateUI();
        pHealth.strikesLeft = pHealth.numLives;
        winCond.reachedHome = false;
        m.movementOn = true;

        //reset spawn point
        while (spawnLocation.Count > 1)
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
        if (collision.gameObject.tag == "Respawn")
        {
            spawnLocation.Push(collision.transform.position);
        }
    }


    //fade out
    public IEnumerator FadeToBlack(string condition = "", float fadeSpeed = 2f)
    {
        float fadeAmount;
        Color objectColor;
        m.movementOn = false;

        //if player has reached the end, show win screen
        //otherwise spawn at last spawn point
        if (condition.Equals("win"))
        {
            winScreen.SetActive(true);
            objectColor = winScreenText.GetComponent<TextMeshProUGUI>().color;
            //fade word in
            while (objectColor.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                winScreenText.GetComponent<TextMeshProUGUI>().color = objectColor;
                yield return null;
            }
            //winScreen.SetActive(true);
            StartCoroutine(FadeToMenu());
        }
        else
        {
            //fade to black to respawn
            objectColor = blackScreen.GetComponent<Image>().color;

            //fade out
            while (blackScreen.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackScreen.GetComponent<Image>().color = objectColor;
                yield return null;
            }

            //reset spawn point to beginning of level
            if (pHealth.strikesLeft <= 0)
            {
                while (spawnLocation.Count > 1)
                    spawnLocation.Pop();
                pHealth.strikesLeft = pHealth.numLives;
            }

            //begin transition to spawn point
            StartCoroutine(FadeFromBlack());
        }
        yield break;
    }

    //fade in
    public IEnumerator FadeFromBlack(int fadeSpeed = 2)
    {
        //tp player
        gameObject.GetComponent<Rigidbody2D>().velocity = resetVelocity;
        gameObject.transform.position = spawnLocation.Peek();
        //reset player stats
        pHealth.playerHealth = pHealth.startHealth;
        pHealth.UpdateUI();
        pStress.playerStress = pStress.startStress;
        pStress.UpdateUI();


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

    //fade to main menu upon winning game
    public IEnumerator FadeToMenu(int fadeSpeed = 2)
    {
        //when implemented it will go to main menu scene
        //gradually fading in so "maisey goes " is visible in front of "home."
        
        yield return new WaitForSeconds(2);
        winScreenText.GetComponent<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, 0f);
        ResetPlayer();
        StartCoroutine(FadeFromBlack());
    }
}
