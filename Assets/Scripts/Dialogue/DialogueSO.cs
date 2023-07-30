using System.Collections.Generic;
using UnityEngine;

public enum NoteType
{
    hint,
    puzzle
}

[CreateAssetMenu]
public class DialogueSO : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private string speaker;
    [SerializeField] private List<string> lines = new List<string>();
    [SerializeField] private NoteType noteType;

    public Sprite Icon { get => icon; }
    public string Speaker { get => speaker; }
    public List<string> Lines { get => lines; }
    public NoteType TypeOfNote { get => noteType; }

    public string GetCompleteScript()
    {
        string newString = string.Empty;

        foreach(string line in lines)
        {
            newString += line + " ";
        }

        return newString;
    }
}
