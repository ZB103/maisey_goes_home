using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Reflection;

public class DialogueManager : MonoBehaviour
{
    private TriggerDialogue trigger;
    public TextMeshProUGUI dialogueText;
    public float textSpeed;
    public bool typingInProgress;

    public Button responseButtonPrefab;
    public Transform buttonContainer;
    private DialogueNode currentNode;
    private Coroutine typingCoroutine;

    void Start()
    {
        trigger = GameObject.Find("EventSystem").GetComponent<TriggerDialogue>();
    }

    private void Update()
    {
        //If a button is pressed, fastforward text scrolling
        if (Input.anyKeyDown)
        {
            if(typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            dialogueText.text = currentNode.dialogueText;
            EnableButtons();
            typingInProgress = false;
        }
    }

    public void StartDialogue(DialogueNode startNode)
    {
        currentNode = startNode;
        DisplayDialogue();
    }

    private void DisplayDialogue()
    {
        typingCoroutine = StartCoroutine(TypeLine(currentNode.dialogueText));
        ClearButtons();

        //display response options using button prefab
        foreach (var response in currentNode.options)
        {
            Button button = Instantiate(responseButtonPrefab, buttonContainer);
            button.GetComponentInChildren<TextMeshProUGUI>().text = response.responseText;
            button.onClick.AddListener(() => OnResponseSelected(response));
            button.interactable = false;
            button.enabled = false;
        }
    }

    private void OnResponseSelected(DialogueResponse response)
    {
        currentNode = response.nextNode;
        DisplayDialogue();
    }

    private void ClearButtons()
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void EnableButtons()
    {
        //check if reached end of dialogue branch
        if (currentNode.options.ToArray().Length == 0)
        {
            trigger.onLeaf = true;
        }
        
        
            //enable buttons
            foreach (Transform child in buttonContainer)
            {
                child.GetComponent<Button>().interactable = true;
                child.GetComponent<Button>().enabled = true;
            }
        
    }

    //type current line of dialogue one letter at a time unless fastforwarded
    IEnumerator TypeLine(String line)
    {
        typingInProgress = true;
        
        //reset text box
        dialogueText.text = string.Empty;

        //type one letter at a time
        foreach (char c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        typingInProgress = false;

        //enable buttons
        EnableButtons();
        yield break;
    }
}
