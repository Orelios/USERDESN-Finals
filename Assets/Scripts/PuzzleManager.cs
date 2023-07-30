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
}

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private List<PuzzleObject> puzzleObjects = new List<PuzzleObject>();
    private SceneLoader sceneLoader;
    private LivesCounter livesCounter;
    private NotesManager notesManager;

    void Awake()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        livesCounter = FindObjectOfType<LivesCounter>();
        notesManager = FindObjectOfType<NotesManager>();
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
            //Restart Level
            Debug.Log("Wrong Answer! Restarting Level.");
            notesManager.SaveNotes();
            livesCounter.MinusHealth();
            if(livesCounter.health != 0)
                sceneLoader.ReplayCurrentScene();
        }
    } 

    void OnEnable() => ActionsManager.onPerformAction += CheckIfInPosition;
    void OnDisable() => ActionsManager.onPerformAction -= CheckIfInPosition;


}
