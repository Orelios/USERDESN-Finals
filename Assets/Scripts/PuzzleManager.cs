using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzleObject
{
    [SerializeField] private Vector2Int coordinate;
    public Vector2Int Coordinate { get => coordinate; }
    [SerializeField] private GameObject puzzleObject;
    public GameObject Obj { get => puzzleObject; }
    [SerializeField] private bool isInPosition;
    public bool IsInPosition { get => isInPosition; set => isInPosition = value; }
    [SerializeField] private DialogueSO puzzleHint;
    public DialogueSO PuzzleHint { get => puzzleHint; }
}

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private List<PuzzleObject> puzzleObjects = new List<PuzzleObject>();
    [SerializeField] private List<DialogueSO> puzzleHints = new List<DialogueSO>();
    private SceneLoader sceneLoader;
    private LivesCounter livesCounter;
    private NotesManager notesManager;
    private int puzzleHintIndex;
    private DialogueStarter dialogueStarter;

    void Awake()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        livesCounter = FindObjectOfType<LivesCounter>();
        notesManager = FindObjectOfType<NotesManager>();
        dialogueStarter = GetComponent<DialogueStarter>();
    }

    // Start is called before the first frame update
    private void CheckIfInPosition()
    {
        foreach(PuzzleObject puzzleObject in puzzleObjects)
        {
            if(puzzleObject.Obj.GetComponent<Coordinates>().PositionOnGrid == puzzleObject.Coordinate)
                puzzleObject.IsInPosition = true;
            else
                puzzleObject.IsInPosition = false;
        }
    }

    public void CheckIfCorrectAnswer()
    {
        bool isCorrectAnswer = true;
        
        foreach(PuzzleObject puzzleObject in puzzleObjects)
        {
            if(!puzzleObject.IsInPosition) isCorrectAnswer = false;
        }

        if(isCorrectAnswer)
        {
            //Next scene
            Debug.Log("Correct Answer! Moving to next scene.");
            notesManager.ClearSavedNotes();
            sceneLoader.PlayNextScene();
        }
        else
        {
            foreach(PuzzleObject puzzleObject in puzzleObjects)
            {
                if(!puzzleObject.IsInPosition)
                {
                    puzzleHints.Add(puzzleObject.PuzzleHint);
                }
            }
            StartDialogue();

            
        }
    }

    private void StartDialogue()
    {
        dialogueStarter.Script = puzzleHints[puzzleHintIndex];

        if(puzzleHintIndex != puzzleHints.Count - 1)
            UIDialogue.onDialogueExtend += QueueNextPuzzleHint;
        else
            UIDialogue.onFinishedDialogue += RestartLevel;

        if(puzzleHintIndex == 0)
            dialogueStarter.StartDialogue();
        else
            dialogueStarter.ForceStartDialogue();
    }

    private void QueueNextPuzzleHint()
    {
        puzzleHintIndex++;
        UIDialogue.onDialogueExtend -= QueueNextPuzzleHint;
        StartDialogue();
        if(puzzleHintIndex == puzzleHints.Count - 1)
            puzzleHints.Clear();
    }

    private void RestartLevel()
    {
        //Restart Level
        Debug.Log("Wrong Answer! Restarting Level.");
        notesManager.SaveNotes();
        livesCounter.MinusHealth();
        UIDialogue.onFinishedDialogue -= RestartLevel;
        if(livesCounter.health != 0)
            sceneLoader.ReplayCurrentScene();
    }

    void OnEnable() => ActionsManager.onPerformAction += CheckIfInPosition;
    void OnDisable() => ActionsManager.onPerformAction -= CheckIfInPosition;


}
