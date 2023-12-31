using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmObject : InteractableObject
{
    private PuzzleManager puzzleManager;

    public override void Awake()
    {
        base.Awake();
        puzzleManager = FindObjectOfType<PuzzleManager>();
    }

    public override void Interact()
    {
        if(FindObjectOfType<UIDialogue>(true).gameObject.activeSelf) return;
        //base.Interact();
        Invoke("ConfirmAnswer", 0.1f);
        puzzleManager.CheckIfInPosition();
    }

    private void ConfirmAnswer() => puzzleManager.OpenSubmissionPrompt();
}
