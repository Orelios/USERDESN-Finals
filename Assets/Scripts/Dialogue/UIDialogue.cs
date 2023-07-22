using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIDialogue : MonoBehaviour
{
    private DialogueSO script;

    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image arrowIndicator;
    
    [Header("Custom Values")]
    [SerializeField] private float typeSpeed;
    [SerializeField] private float animSpeed;

    private int index = 0;

    private void OnEnable()
    {
        //Trigger Open Animation
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
        if (index == 0) yield return new WaitForSeconds(animSpeed);
        foreach(char c in script.Lines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    private IEnumerator SetActiveFalse()
    {
        //Trigger Close Animation

        yield return new WaitForSeconds(animSpeed);
        gameObject.SetActive(false);
    }
}
