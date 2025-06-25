using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmPlayer : MonoBehaviour
{
    private GameObject player;
    private Stress pStress;
    private Health pHealth;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pStress = player.GetComponent<Stress>();
        pHealth = player.GetComponent<Health>();
    }

    //harm player's stress levels upon collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        pStress.Hurt();
        if (pStress.playerStress >= 90)
        {
            pHealth.Hurt();
        }

        if (pStress.playerStress >= 70)
        {
            pHealth.Hurt();
        }
    }
}
