using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpoonBar : MonoBehaviour
{
    public Slider spoonBar;
    public Spoons playerSpoons;

    // Start is called before the first frame update
    void Start()
    {
        playerSpoons = GameObject.FindGameObjectWithTag("Player").GetComponent<Spoons>();
        spoonBar.value = playerSpoons.playerStress;
    }

    public void SetHealth(int sp)
    {
        spoonBar.value = sp;
    }
}
