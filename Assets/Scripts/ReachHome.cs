using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachHome : MonoBehaviour
{
    private GameObject player;
    private Stress pStress;
    private PlayerMovement m;
    private Rigidbody2D rb;
    private ResetGame resetGame;
    public bool reachedHome;   //disable trigger
    public Vector3 parkCoords;  //spot player Lerps to

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        m = player.GetComponent<PlayerMovement>();
        pStress = player.GetComponent<Stress>();
        rb = player.GetComponent<Rigidbody2D>();
        resetGame = player.GetComponent<ResetGame>();
        parkCoords = GameObject.Find("Park Spot").transform.position;
        reachedHome = false;
    }

    //Detect player reaches house and deploy win condition
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !reachedHome)
        {
            reachedHome = true;
            m.movementOn = false;
            rb.velocity *= .2f;
            StartCoroutine(EnterHouse());
        }
    }

    //disable player controls and put square in house
    IEnumerator EnterHouse()
    {
        //time movement started
        float startTime = Time.time;
        float distance = Vector3.Distance(m.transform.position, parkCoords);
        rb.velocity = new Vector2(0, 0);

        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            float distCovered = (Time.time - startTime);
            float fractionOfJourney = distCovered / distance;
            m.transform.position = Vector3.Lerp(m.transform.position, parkCoords, fractionOfJourney);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        while (pStress.playerStress > 0)
        {
            pStress.Heal();
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(.5f);
        StartCoroutine(resetGame.FadeToBlack("win"));
    }
}
