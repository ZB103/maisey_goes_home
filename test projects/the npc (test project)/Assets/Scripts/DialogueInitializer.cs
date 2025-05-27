using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInitializer : MonoBehaviour
{
    public DialogueNode startNode;

    // Start is called before the first frame update
    void Start()
    {
        //Start dialogue manager
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.StartDialogue(startNode);
    }

    //Create dialogue nodes
    private void Awake()
    {
        DialogueNode n1 = new DialogueNode
        {
            dialogueText = "Hello!",
            options = new List<DialogueResponse>()
        };

        DialogueNode n2 = new DialogueNode
        {
            dialogueText = "Sorry?",
            options = new List<DialogueResponse>()
        };

        DialogueNode n3 = new DialogueNode
        {
            dialogueText = "Ah- don't you feel that's a bit uncalled for?",
            options = new List<DialogueResponse>()
        };

        n1.options.Add(new DialogueResponse { responseText = "Good afternoon!", nextNode = n2 });
        n1.options.Add(new DialogueResponse { responseText = "How are you?", nextNode = n3 });

        DialogueNode n4 = new DialogueNode
        {
            dialogueText = "Okay...",
            options = new List<DialogueResponse>()
        };

        DialogueNode n5 = new DialogueNode
        {
            dialogueText = "Was there something I could help you with?",
            options = new List<DialogueResponse>()
        };

        n2.options.Add(new DialogueResponse { responseText = "Just wishing you a good afternoon!", nextNode = n4 });
        n2.options.Add(new DialogueResponse { responseText = "Huh?", nextNode = n5 });

        DialogueNode n6 = new DialogueNode
        {
            dialogueText = "Don't worry about it. What can I do for you?",
            options = new List<DialogueResponse>()
        };

        DialogueNode n7 = new DialogueNode
        {
            dialogueText = "I see..in that case..what can I do for you?",
            options = new List<DialogueResponse>()
        };

        n3.options.Add(new DialogueResponse { responseText = "What do you mean?", nextNode = n6 });
        n3.options.Add(new DialogueResponse { responseText = "Sorry, I didn't mean to be rude.", nextNode = n7 });

        DialogueNode n8 = new DialogueNode
        {
            dialogueText = "No, no...what can I do for you?",
            options = new List<DialogueResponse>()
        };

        DialogueNode n9 = new DialogueNode
        {
            dialogueText = "What can I do for you?",
            options = new List<DialogueResponse>()
        };

        n4.options.Add(new DialogueResponse { responseText = "Did I do something wrong?", nextNode = n8 });
        n4.options.Add(new DialogueResponse { responseText = "Okay?", nextNode = n9});

        startNode = n1;
    }
}
