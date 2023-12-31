using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField] private DialogueSO script;
    public DialogueSO Script { get => script; set => script = value; }
    private UIDialogue dialogueUI;

    private void Awake()
    {
        dialogueUI = FindObjectOfType<UIDialogue>(true);
    }

    public void StartDialogue()
    {
        if (dialogueUI.gameObject.activeSelf) return;
        dialogueUI.gameObject.SetActive(true);
        dialogueUI.StartNewDialogue(script);
    }

    public void ForceStartDialogue()
    {
        dialogueUI.StartNewDialogue(script);
    }
}
