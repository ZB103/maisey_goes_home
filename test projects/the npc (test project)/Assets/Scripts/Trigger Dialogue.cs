using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    private bool dialogueOpen;
    public GameObject dialogueBox;

    void Awake()
    {
        dialogueOpen = false;
        dialogueBox = GameObject.Find("Panel");
    }

    // Update is called once per frame
    void Update()
    {
        //if dialogue box is closed and e is pressed, open dialogue
        if(!dialogueOpen && Input.GetKeyDown(KeyCode.E))
        {
            toggleDialogue();
        }
    }

    public void toggleDialogue()
    {
        //turn off
        if (dialogueOpen)
        {
            dialogueOpen = false;
            dialogueBox.SetActive(false);
        }
        //turn on
        else
        {
            dialogueOpen = true;
            dialogueBox.SetActive(true);
        }
    }
}
