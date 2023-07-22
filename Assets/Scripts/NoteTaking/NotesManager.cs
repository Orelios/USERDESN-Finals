using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotesManager : MonoBehaviour
{
    [SerializeField] private List<DialogueSO> dialogues = new List<DialogueSO>();
    [Header("UI")]
    [SerializeField] private GameObject notesUI;
    [SerializeField] private GameObject noteItemPrefab;
    [SerializeField] private Transform noteItemParent;
    [SerializeField] private GameObject notesPopUp;
    public GameObject NotesPopUp { get => notesPopUp; }

    public void AddToNotes(DialogueSO dialogue)
    {
        if(dialogues.Contains(dialogue)) return;

        //Add dialogue to list
        dialogues.Add(dialogue);

        //Update UI
        AddNoteUI(dialogue);
    }

    private void AddNoteUI(DialogueSO dialogue)
    {
        GameObject instance = Instantiate(noteItemPrefab, noteItemPrefab.transform.position, Quaternion.identity, noteItemParent);

        //Set the image icon to the corresponding sprite in the DialogueSO
        instance.transform.GetChild(0).GetComponent<Image>().sprite = dialogue.Icon;

        //Set the text to the first line in the DialogueSO's script
        instance.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.Lines[0];

        //Set the NoteItem's DialogueSO
        instance.GetComponent<NoteItem>().Dialogue = dialogue;
    }

    public void ToggleNotesUI()
    {
        if(notesUI.activeSelf)
        {
            //Play close animation
            notesUI.SetActive(false);

            CloseNotePopUp();
        }
        else
        {
            //Play open animation
            notesUI.SetActive(true);
        }
    }

    public void CloseNotePopUp()
    {
        //Play close animation
        notesPopUp.SetActive(false);
    }
}
