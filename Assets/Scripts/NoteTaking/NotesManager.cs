using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotesManager : MonoBehaviour
{
    [SerializeField] private List<DialogueSO> hintNotes = new List<DialogueSO>();
    public List<DialogueSO> HintNotes { get => hintNotes; }
    [SerializeField] private List<DialogueSO> puzzleNotes = new List<DialogueSO>();
    public List<DialogueSO> PuzzleNotes { get => puzzleNotes; }
    [SerializeField] private NotesSavedSO hintNotesSaved;
    [SerializeField] private NotesSavedSO puzzleNotesSaved;
    [Header("UI")]
    [SerializeField] private GameObject notesUI;
    [SerializeField] private GameObject hintsUI;
    [SerializeField] private GameObject puzzleUI;
    [SerializeField] private GameObject noteItemPrefab;
    [SerializeField] private Transform hintNoteItemParent;
    [SerializeField] private Transform puzzleNoteItemParent;
    [SerializeField] private GameObject notesPopUp;
    [SerializeField] private GameObject newNotesNotif;
    [SerializeField] private GameObject newHintsNotif;
    [SerializeField] private GameObject newPuzzleNotif;
    public GameObject NotesPopUp { get => notesPopUp; }
    private bool isNotesUIClosing;
    private bool isNotesPopUpClosing;
    private int newNotes;
    [SerializeField] private bool newHintNotes, newPuzzleNotes;
    private PuzzleManager puzzleManager;

    private void Awake()
    {
        puzzleManager = FindObjectOfType<PuzzleManager>();
    }

    private void Start()
    {
        InitializeNotes();
    }

    private void InitializeNotes()
    {
        foreach(DialogueSO dialogueSO in hintNotesSaved.Notes)
        {
            AddToNotes(hintNotes, dialogueSO);
        }

        foreach(DialogueSO dialogueSO in puzzleNotesSaved.Notes)
        {
            AddToNotes(puzzleNotes, dialogueSO);
        }
    }

    public void AddToNotes(List<DialogueSO> dialogueList, DialogueSO dialogue)
    {
        if(dialogueList.Contains(dialogue)) return;

        //Add dialogue to list
        dialogueList.Add(dialogue);

        //Update UI
        AddNoteUI(dialogueList == hintNotes ? hintNoteItemParent : puzzleNoteItemParent, dialogue);

        if(!notesUI.activeSelf)
        {
            newNotes++;
            UpdateNewNotesUI();
        }

        if(dialogueList == hintNotes)
        {
            newHintNotes = !hintsUI.activeSelf;
            newHintsNotif.SetActive(newHintNotes);
        }
        else
        {
            newPuzzleNotes = !puzzleUI.activeSelf;
            newPuzzleNotif.SetActive(newPuzzleNotes);
        }
    }

    public void DisableHintsNotif()
    {
        newHintNotes = false;
        newHintsNotif.SetActive(false);
    }

    public void DisablePuzzleNotif()
    {
        newPuzzleNotes = false;
        newPuzzleNotif.SetActive(false);
    }

    private void AddNoteUI(Transform noteParent, DialogueSO dialogue)
    {
        GameObject instance = Instantiate(noteItemPrefab, noteItemPrefab.transform.position, Quaternion.identity, noteParent);

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

    public void SaveNotes()
    {
        hintNotesSaved.Notes = hintNotes;
        puzzleNotesSaved.Notes = puzzleManager.PuzzleHints;
    }

    public void ClearSavedNotes()
    {
        hintNotesSaved.ClearSavedNotes();
        puzzleNotesSaved.ClearSavedNotes();
    }
}
