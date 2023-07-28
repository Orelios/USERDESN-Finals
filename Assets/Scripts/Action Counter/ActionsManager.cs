using UnityEngine;
using System;

public class ActionsManager : MonoBehaviour
{
    private ActionCounterSO actionCounterSO;    //This is where the Initial Number of Actions can be found.
    public ActionCounterSO ActionCounterSO { get => actionCounterSO; }
    
    private int numActions;    //This handles the current number of actions the player has in the current level.
    public int NumActions 
    { 
        get => numActions; 
        set
        {
            numActions = value;

            onActionCounterUpdated?.Invoke();   //Triggers onActionPerformed event and calls all functions subscribed to it when numActions value is changed.

            if(IsOutOfActions()) onOutOfActions?.Invoke();   //Triggers onOutOfActions event and calls all functions subscribed to it if the player runs out of actions.
        }
    }
    //Event for when the number of actions is initialized
    public static event Action onActionCounterInitialized;

    //Event (delegate) for when the player makes an action
    public delegate void OnPerformAction(); //Delegate so that it can be invoked in other scripts.
    public static OnPerformAction onPerformAction;  //Things that happen when the player performs an action should be subscribed to this event.

    //Event for when the number of actions changes
    public static event Action onActionCounterUpdated;   //Things that happen when the number of actions changes during play (like when the player makes an action) should be subscribed to this event.

    //Event for when player runs out of actions
    public static event Action onOutOfActions;  //Things that happen when the player runs out of actions should be subscribed to this event.
    
    private void Awake()
    {
        actionCounterSO = Resources.Load<ActionCounterSO>("NumberOfActions");
    }

    private void Start()    //This can be removed once there is a Level Start event that InitializeActionCount can be subscribed to.
    {
        InitializeActionCount();
    }

    private bool IsOutOfActions() 
    {
        return (numActions <= 0) ? true : false;
    }

    public void InitializeActionCount()    //Call this whenever the level starts, or subscribe it to an event
    {
        NumActions = actionCounterSO.InitialNumberOfActions;
        onActionCounterInitialized?.Invoke();
    }

    public void DecrementActionCounter()    //Call this whenever an action is made, or subscribe it to an event
    {
        if(IsOutOfActions()) return; 
        NumActions--; 
    }

    private void OnEnable()
    {
        /*

        (1) Subscribe InitializeActionCount function to a Start Level event.

        Example: LevelManager.onStartLevel += InitializeActionCount;
            Note: this expects there to be a LevelManager script with
            a public static event called "onStartLevel"

        */

        onPerformAction += DecrementActionCounter; //Subscribe DecrementActionCounter function to a Perform Action event.

        onOutOfActions += OutOfActions; //Feel free to remove this part. This is just a sample for the onOutOfActions event.
    }

    private void OnDisable()
    {
        /*
        
        If an event was used, don't forget to unsubscribe the function from the event.

        Example: 
        LevelManager.onStartLevel -= InitializeActionCount;  

        */

        onPerformAction -= DecrementActionCounter;

        onOutOfActions -= OutOfActions; //Feel free to remove this part. This is just a sample for the onOutOfActions event.
    }

    //Feel free to remove this part. This is just a sample for the onOutOfActions event.
    private void OutOfActions()
    {
        LivesCounter.instance.MinusHealth();
        NumActions = actionCounterSO.InitialNumberOfActions;
        Debug.Log("You are out of actions!");
    }
}
