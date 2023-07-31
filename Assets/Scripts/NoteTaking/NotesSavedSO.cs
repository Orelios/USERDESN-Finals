using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class NotesSavedSO : ScriptableObject
{
    [SerializeField] private List<DialogueSO> notes;
    public List<DialogueSO> Notes { get => notes; set => notes = value; }

    public void ClearSavedNotes()
    {
        notes.Clear();
    }
}
