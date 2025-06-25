using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressBar : MonoBehaviour
{
    public Slider stressBar;
    public Stress playerStress;

    // Start is called before the first frame update
    void Start()
    {
        playerStress = GameObject.FindGameObjectWithTag("Player").GetComponent<Stress>();
        stressBar.value = playerStress.playerStress;
    }

    public void SetStress(int sp)
    {
        stressBar.value = sp;
    }
}
