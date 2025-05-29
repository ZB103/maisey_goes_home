using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    private bool dialogueOpen;
    public GameObject dialogueBox;
    public DialogueInitializer initializer;
    public bool onLeaf;    //have we reached a leaf node?

    void Awake()
    {
        dialogueOpen = false;
        onLeaf = false;
        dialogueBox = GameObject.Find("Panel");
        dialogueBox.SetActive(false);
        initializer = FindObjectOfType<DialogueInitializer>();
    }

    // Update is called once per frame
    void Update()
    {
        //if dialogue box is closed and e is pressed, open dialogue
        if(!dialogueOpen && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E)))
        {
            toggleDialogue();
        }
        //if conversation is over and e is pressed, close dialogue
        else if (dialogueOpen && onLeaf && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse0)))
        {
            toggleDialogue();
        }
    }

    public void toggleDialogue()
    {
        onLeaf = false;
        //turn off
        if (dialogueOpen)
        {
            dialogueOpen = false;
            dialogueBox.SetActive(false);
        }
        //turn on
        else
        {
            initializer.InitializeDialogue();
            dialogueOpen = true;
            dialogueBox.SetActive(true);
        }
    }
}
