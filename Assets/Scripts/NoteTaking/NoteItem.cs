using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NoteItem : MonoBehaviour
{
    [SerializeField] private DialogueSO dialogue;
    public DialogueSO Dialogue { get => dialogue; set => dialogue = value; }
    private GameObject notePopUpPanel;

    private void Awake()
    {
        notePopUpPanel = FindObjectOfType<NotesManager>().NotesPopUp;
    }

    public void OpenPopUp()
    {
        if(!notePopUpPanel.activeSelf) notePopUpPanel.SetActive(true);

        notePopUpPanel.transform.GetChild(1).GetComponent<Image>().sprite = dialogue.Icon;

        notePopUpPanel.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.GetCompleteScript();
    }
    
}
