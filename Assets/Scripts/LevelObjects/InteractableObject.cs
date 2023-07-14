using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : LevelObject
{
    private NearObjects nearObjects;
    private Direction directionFromPlayer;
    public Direction DirectionFromPlayer { get => directionFromPlayer; }

    [Header("Interact Prompt")]
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] Vector3 offset;
    public override void Awake()
    {
        base.Awake();
        nearObjects = FindObjectOfType<NearObjects>();
    }
    private void CheckPlayerPosition(Vector2Int playerMoveTarget, Direction playerFacingDirection, bool isPlayer)
    {
        if(isPlayer)
        {
            if(IsAdjacentToObject(playerMoveTarget))
            {
                if(!nearObjects.ListOfNearObjects.Contains(this))
                {
                    //Add this object to the list of near objects
                    FindObjectOfType<NearObjects>().ListOfNearObjects.Add(this);
                }
            }
            else
            {
                if(nearObjects.ListOfNearObjects.Contains(this))
                {
                    //Remove this object from the list of near objects
                    FindObjectOfType<NearObjects>().ListOfNearObjects.Remove(this);
                    SetInteractPrompt(false);
                }
            }
        }
    }

    public void SetInteractPrompt(bool state)
    {
        if(state) interactPrompt.transform.position = transform.position + offset;
        interactPrompt.SetActive(state);
    }

    private bool IsAdjacentToObject(Vector2Int otherObjectPos)
    {
        Vector2Int newPos = coordinates.PositionOnGrid - otherObjectPos;
        if(Mathf.Abs(newPos.x) <= 1 && Mathf.Abs(newPos.y) <= 1)
        {
            //If the object is diagonal from the other object, don't count it.
            if(Mathf.Abs(newPos.x) == 1 && Mathf.Abs(newPos.y) == 1) return false;
            //If the object is on the position of the other object, don't count it.
            if(newPos == Vector2.zero) return false;

            SetDirectionFromPlayer(newPos);
            return true;
        }
        else return false;
    }

    private void SetDirectionFromPlayer(Vector2Int posRelativeToPlayer)
    {
        if(posRelativeToPlayer == Vector2Int.right) directionFromPlayer = Direction.East;
        else if(posRelativeToPlayer == Vector2Int.left) directionFromPlayer = Direction.West;
        else if(posRelativeToPlayer == Vector2Int.up) directionFromPlayer = Direction.North;
        else if(posRelativeToPlayer == Vector2Int.down) directionFromPlayer = Direction.South;
    }

    private void OnEnable()
    {
        PlayerMovement.onAboutToMoveToTarget += CheckPlayerPosition;
    }

    private void OnDisable()
    {
        PlayerMovement.onAboutToMoveToTarget -= CheckPlayerPosition;
    }

    public virtual void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}
