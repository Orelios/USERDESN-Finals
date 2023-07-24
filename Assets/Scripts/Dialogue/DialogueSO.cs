using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueSO : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private string speaker;
    [SerializeField] private List<string> lines = new List<string>();

    public Sprite Icon { get => icon; }
    public string Speaker { get => speaker; }
    public List<string> Lines { get => lines; }

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
