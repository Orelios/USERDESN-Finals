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
    [SerializeField] private GameObject newNotesNotif;
    public GameObject NotesPopUp { get => notesPopUp; }
    private bool isNotesUIClosing;
    private bool isNotesPopUpClosing;
    private int newNotes;

    public void AddToNotes(DialogueSO dialogue)
    {
        if(dialogues.Contains(dialogue)) return;

        //Add dialogue to list
        dialogues.Add(dialogue);

        //Update UI
        AddNoteUI(dialogue);

        if(!notesUI.activeSelf)
        {
            newNotes++;
            UpdateNewNotesUI();
        }
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
            if(!isNotesUIClosing) 
            {
                //Play close animation
                notesUI.GetComponent<Animator>().SetTrigger("Close");
                isNotesUIClosing = true;
                StartCoroutine(SetUIInactive(notesUI, 0.25f));
                isNotesUIClosing = false;
            }
            if(!isNotesPopUpClosing && notesPopUp.activeSelf)
            {
                isNotesPopUpClosing = true;
                CloseNotePopUp();
                isNotesPopUpClosing = false;
            }
        }
        else
        {
            notesUI.SetActive(true);
            //Play open animation
            notesUI.GetComponent<Animator>().SetTrigger("Open");

            newNotes = 0;
            UpdateNewNotesUI();
        }
    }

    private IEnumerator SetUIInactive(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    private void UpdateNewNotesUI()
    {
        if(newNotes > 0)
        {
            //Enable new notes notification
            newNotesNotif.SetActive(true);

            //Set the text
            newNotesNotif.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = newNotes.ToString();
        }
        else
        {
            //Disable new notes notification
            newNotesNotif.SetActive(false);
        }
    }

    public void CloseNotePopUp()
    {
        //Play close animation
        notesPopUp.GetComponent<Animator>().SetTrigger("Close");
        StartCoroutine(SetUIInactive(notesPopUp, 0.25f));
    }
}
