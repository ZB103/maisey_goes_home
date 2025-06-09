using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stress : MonoBehaviour
{
    public float playerStress;
    public float startStress = 0;
    public StressBar stressBar;

    // Start is called before the first frame update
    void Awake()
    {
        playerStress = startStress;
        stressBar = GameObject.Find("StressBar").GetComponent<StressBar>();
    }

    //debug controls
    //public void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.G))
    //    {
    //        Hurt();
    //        print("playerStress : " + playerStress);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.H))
    //    {
    //        Heal();
    //        print("playerStress : " + playerStress);
    //    }
    //}

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

    //check for death & update UI elements to match new health stat
    public void UpdateUI()
    {
        stressBar.SetStress((int)playerStress);
    }
}
