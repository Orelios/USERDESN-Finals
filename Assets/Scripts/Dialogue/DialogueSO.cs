using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueSO : ScriptableObject
{
    [SerializeField] private string speaker;
    [SerializeField] private List<string> lines = new List<string>();

    public string Speaker { get => speaker; }
    public List<string> Lines { get => lines; }
}
