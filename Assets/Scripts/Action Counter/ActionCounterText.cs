using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ActionCounterText : MonoBehaviour
{
    private TextMeshProUGUI numActionsText;
    private ActionsManager actionsManager;
    
    private void Awake()
    {
        numActionsText = GetComponent<TextMeshProUGUI>();
        actionsManager = FindObjectOfType<ActionsManager>();
    }

    private void SetNumActionsText()
    {
        if(actionsManager == null) return;
        numActionsText.text = actionsManager.NumActions.ToString();
    }

    private void OnEnable()
    {
        //Calls SetNumActionsText function whenever the Action Counter is updated (when the level initializes and when the player makes an action)
        ActionsManager.onActionCounterUpdated += SetNumActionsText;
    }

    private void OnDisable()
    {
        ActionsManager.onActionCounterUpdated -= SetNumActionsText;
    }
}
