using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        gameObject.SetActive(false);
    }

    public void StartDialogue(DialogueNode startNode)
    {
        currentNode = startNode;
        DisplayDialogue();
    }

    private void DisplayDialogue()
    {
        dialogueText.text = currentNode.dialogueText;
        ClearButtons();

        foreach (var response in currentNode.options)
        {
            Button button = Instantiate(responseButtonPrefab, buttonContainer); 
            button.GetComponentInChildren<Text>().text = response.responseText;
            button.onClick.AddListener(() => OnResponseSelected(response));
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
}
