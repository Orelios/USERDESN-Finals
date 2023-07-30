using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIDialogue : MonoBehaviour
{
    [SerializeField] private DialogueSO script;

    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image arrowIndicator;
    
    [Header("Custom Values")]
    [SerializeField] private float typeSpeed;
    [SerializeField] private float animSpeed;
    
    private int index = 0;
    private Animator animator;
    private bool isClosing;
    private NotesManager notesManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        notesManager = FindObjectOfType<NotesManager>();
    }

    private void OnEnable()
    {
        //Trigger Open Animation
        animator.SetTrigger("Open");
    }

    // Update is called once per frame
    void Update()
    {
        //If the line is complete, display the arrow indicator
        arrowIndicator.gameObject.SetActive(dialogueText.text == script.Lines[index]);
    }

    public void StartNewDialogue(DialogueSO newScript)
    {
        script = newScript;
        InitializeDialogue();
    }

    private void InitializeDialogue()
    {
        dialogueText.text = string.Empty;
        StartDialogue();
    }

    private void OnNextLine()
    {
        if (dialogueText.text == script.Lines[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            dialogueText.text = script.Lines[index];
        }
    }

    private void NextLine()
    {
        if (index < script.Lines.Count - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            if(isClosing) return;

            switch(script.TypeOfNote)
            {
                case NoteType.hint:
                    notesManager.AddToNotes(notesManager.HintNotes, script);
                    break;
                case NoteType.puzzle:
                    notesManager.AddToNotes(notesManager.PuzzleNotes, script);
                    break;
                default:
                    break;
            }
            
            StartCoroutine(SetActiveFalse());
        }
    }

    private void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        foreach(char c in script.Lines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    private IEnumerator SetActiveFalse()
    {
        isClosing = true;
        //Trigger Close Animation
        animator.SetTrigger("Close");

        yield return new WaitForSeconds(animSpeed);
        isClosing = false;
        gameObject.SetActive(false);
    }
}
