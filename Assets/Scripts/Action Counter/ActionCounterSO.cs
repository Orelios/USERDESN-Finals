using UnityEngine;

[CreateAssetMenu(fileName = "NumberOfActions", menuName = "ActionCounterSO")]
public class ActionCounterSO : ScriptableObject
{
    [SerializeField] private int initialNumberOfActions;
    public int InitialNumberOfActions { get => initialNumberOfActions; set => initialNumberOfActions = value; }
}
