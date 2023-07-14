using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : LevelObject
{
    private NearObjects nearObjects;

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
                if(!nearObjects.ListOfNearObjects.Contains(gameObject))
                {
                    //Add this object to the list of near objects
                    FindObjectOfType<NearObjects>().ListOfNearObjects.Add(gameObject);
                }
            }
            else
            {
                if(nearObjects.ListOfNearObjects.Contains(gameObject))
                {
                    //Remove this object from the list of near objects
                    FindObjectOfType<NearObjects>().ListOfNearObjects.Remove(gameObject);
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
        if(newPos.x <= 1 && newPos.y <= 1)
        {
            return true;
        }
        else return false;
    }

    private void OnEnable()
    {
        PlayerMovement.onAboutToMoveToTarget += CheckPlayerPosition;
    }

    private void OnDisable()
    {
        PlayerMovement.onAboutToMoveToTarget -= CheckPlayerPosition;
    }
}
