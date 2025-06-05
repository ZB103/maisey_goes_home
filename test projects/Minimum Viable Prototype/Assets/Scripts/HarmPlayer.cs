using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmPlayer : MonoBehaviour
{
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //harm player's stress levels upon collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        player.GetComponent<Stress>().Hurt();
    }
}
