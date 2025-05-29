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

    public Button responseButtonPrefab;
    public Transform buttonContainer;
    private DialogueNode currentNode;

    void Start()
    {
        trigger = GameObject.Find("EventSystem").GetComponent<TriggerDialogue>();
    }

    private void Update()
    {
        //If a button is pressed, fastforward text scrolling
        if (Input.anyKeyDown)
        {
            StopAllCoroutines();
            dialogueText.text = currentNode.dialogueText;
        }
    }

    public void StartDialogue(DialogueNode startNode)
    {
        currentNode = startNode;
        DisplayDialogue();
    }

    private void DisplayDialogue()
    {
        StartCoroutine(TypeLine(currentNode.dialogueText));
        ClearButtons();

        //check if reached end of dialogue branch
        if (currentNode.options.ToArray().Length == 0)
        {
            trigger.onLeaf = true;
        }
        else
        {
            trigger.onLeaf = false;
            //display response options using button prefab
            foreach (var response in currentNode.options)
            {
                Button button = Instantiate(responseButtonPrefab, buttonContainer);
                button.GetComponentInChildren<TextMeshProUGUI>().text = response.responseText;
                button.onClick.AddListener(() => OnResponseSelected(response));
            }
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

    IEnumerator TypeLine(String line)
    {
        //reset text box
        dialogueText.text = string.Empty;

        //type 1 letter at a time
        foreach (char c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
