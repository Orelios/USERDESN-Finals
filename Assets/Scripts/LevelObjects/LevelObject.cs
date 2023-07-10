using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    protected Coordinates coordinates;
    public virtual void Awake()
    {
        coordinates = GetComponent<Coordinates>();
    }

    //Sample object functionality
    //Feel free to use this as a base for object actions
    private void DoLevelObjectAction(Vector2Int playerMoveTarget, Direction playerFacingDirection)
    {
        if(coordinates == null) return;
        Direction playerComingFrom = DeterminePlayerPositionRelativeToObject(playerFacingDirection);
        //If the player is moving to my space...
        if(playerMoveTarget == coordinates.PositionOnGrid) 
        {
            //...do stuff.
            Debug.Log("Player is moving to the space of " + gameObject.name + " from the " + GetDirectionString(playerComingFrom) + ".");
        }
    }

    private Direction DeterminePlayerPositionRelativeToObject(Direction playerFacingDirection)
    {
        switch(playerFacingDirection)
        {
            case Direction.North: return Direction.South;
            case Direction.South: return Direction.North;
            case Direction.East: return Direction.West;
            case Direction.West: return Direction.East;
            default: return Direction.North;
        }
    }

    private string GetDirectionString(Direction direction)
    {
        switch(direction)
        {
            case Direction.North: return "North";
            case Direction.South: return "South";
            case Direction.East: return "East";
            case Direction.West: return "West";
            default: return "South";
        }
    }

    private void OnEnable()
    {
        PlayerMovement.onAboutToMoveToTarget += DoLevelObjectAction;
    }

    private void OnDisable()
    {
        PlayerMovement.onAboutToMoveToTarget -= DoLevelObjectAction;
    }
}
