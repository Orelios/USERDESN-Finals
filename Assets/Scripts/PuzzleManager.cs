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
    public List<DialogueSO> PuzzleHints { get => puzzleHints; }
    private SceneLoader sceneLoader;
    private LivesCounter livesCounter;
    private NotesManager notesManager;
    private int puzzleHintIndex;
    private DialogueStarter dialogueStarter;
    private GameObject submissionPrompt;

    void Awake()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        livesCounter = FindObjectOfType<LivesCounter>();
        notesManager = FindObjectOfType<NotesManager>();
        dialogueStarter = GetComponent<DialogueStarter>();
        submissionPrompt = FindObjectOfType<SubmissionPrompt>(true).gameObject;
    }

    // Start is called before the first frame update
    public void CheckIfInPosition()
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
            livesCounter.SetHealthToMaxHealth();
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
    }

    public void OpenSubmissionPrompt()
    {
        puzzleHintIndex = 0;
        UIDialogue.onFinishedDialogue -= OpenSubmissionPrompt;
        submissionPrompt.gameObject.SetActive(true);
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


}
