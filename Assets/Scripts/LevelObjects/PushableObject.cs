using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : LevelObject
{
    private float pushSpeed;
    private Vector3 targetPosition;
    private bool moving;

    private void Start()
    {
        pushSpeed = FindObjectOfType<PlayerMovement>().MovementSpeed;
    }

    private void BeMoved(Vector2Int playerMoveTarget, Direction playerFacingDirection)
    {
        if(coordinates == null) return;
        
        //If the player is moving to my space...
        if(playerMoveTarget == coordinates.PositionOnGrid) 
        {
            //...be pushed to the direction the player is facing.

            //Get the direction the player is facing.
            Direction pushDirection = playerFacingDirection;

            //Get the coordinate of the square in that direction.
            Vector2Int coordinate = GetCoordinateInDirection(playerFacingDirection);

            //Set the coordinate as the move target.
            targetPosition = coordinates.GroundTilemap.GetCellCenterWorld((Vector3Int)coordinate);

            //Start moving.
            moving = true;
        }
    }

    private void Update()
    {
        if(moving)
        {
            //Move to target
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, pushSpeed * Time.deltaTime);
            
            //When the object reaches the target position's square...
            if(coordinates.GroundTilemap.WorldToCell(transform.position) == coordinates.GroundTilemap.WorldToCell(targetPosition)) {
                
                coordinates.SetCurrentPositionOnGrid(); //...update the current position on grid.

                //When the object reaches the exact position of the target...
                if(transform.position == targetPosition)
                {
                    moving = false; //...stop moving.
                }
            }
        }
    }

    private Vector2Int GetCoordinateInDirection(Direction direction)
    {
        Vector2Int coordinate = coordinates.PositionOnGrid;
        switch(direction)
        {
            case Direction.North:
                coordinate += new Vector2Int(0,1);
                break;
            case Direction.South:
                coordinate += new Vector2Int(0,-1);
                break;
            case Direction.East:
                coordinate += new Vector2Int(1,0);
                break;
            case Direction.West:
                coordinate += new Vector2Int(-1,0);
                break;
        }
        return coordinate;
    }

    private void OnEnable()
    {
        PlayerMovement.onAboutToMoveToTarget += BeMoved;
    }

    private void OnDisable()
    {
        PlayerMovement.onAboutToMoveToTarget -= BeMoved;
    }
}
