using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealPlayer : MonoBehaviour
{
    public GameObject skillImage;            //Image object of skill use button
    public GameObject skillImageDisabled;    //Image object of skill use button disabled
    private Stress pStress;             //Player stress metric
    private float coolDownTime; //how long between uses of skill
    private bool skillUsable;   //can we use the skill now?

    // Start is called before the first frame update
    void Start()
    {
        pStress = GameObject.FindGameObjectWithTag("Player").GetComponent<Stress>();
        skillImage.SetActive(false);
        coolDownTime = 2.5f;
        skillUsable = false;
        StartCoroutine(CountDownSkill());
    }

    // Update is called once per frame
    void Update()
    {
        if (skillUsable && Input.GetKeyDown(KeyCode.E))
            StartCoroutine(CountDownSkill());
    }

    IEnumerator CountDownSkill()
    {
        skillImage.SetActive(false);
        skillUsable = false;
        yield return new WaitForSeconds(coolDownTime / 2);
        pStress.Heal();
        pStress.Heal();
        yield return new WaitForSeconds(coolDownTime / 2);
        skillImage.SetActive(true);
        skillUsable = true;
    }
}
