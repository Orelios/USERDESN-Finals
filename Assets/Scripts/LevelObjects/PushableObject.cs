using System.Collections;
using UnityEngine;

public class PushableObject : LevelObject
{
    private float pushSpeed;
    private Vector3 targetPosition;
    private bool moving;
    private GroundTilemap groundTilemap;
    private bool isPlayerPushing;

    private void Start()
    {
        pushSpeed = FindObjectOfType<PlayerMovement>().MovementSpeed;
        groundTilemap = FindObjectOfType<GroundTilemap>();

        //Add me to the list of pushable objects
        if(!groundTilemap.PushableObjectCoordinates.Contains(coordinates.PositionOnGrid))
        {
            groundTilemap.PushableObjectCoordinates.Add(coordinates.PositionOnGrid);
        }
    }

    private void BeMoved(Vector2Int moveTarget, Direction playerFacingDirection, bool isPlayer)
    {
        if(coordinates == null) return;
        
        //If the player or an object is moving to my space...
        if(moveTarget == coordinates.PositionOnGrid) 
        {
            isPlayerPushing = isPlayer;
            StopAllCoroutines();

            //...move me to towards the next coordinate in the direction the player is facing
            Move((Vector3Int)GetCoordinateInDirection(playerFacingDirection), playerFacingDirection);
        }
    }

    private void Move(Vector3Int targetCoordinate, Direction direction)
    {
        //Tell other objects that I am about to move to the target coordinate
        PlayerMovement.onAboutToMoveToTarget?.Invoke((Vector2Int)targetCoordinate, direction, false);

        //If the target coordinate is within the grid and has no obstacles...
        if(groundTilemap.IsTargetTileFree(targetCoordinate))
        {
            //...set the coordinate as the move target...
            targetPosition = coordinates.GroundTilemap.GetCellCenterWorld(targetCoordinate);

            if(groundTilemap.PushableObjectCoordinates.Contains(coordinates.PositionOnGrid))
            {
                groundTilemap.PushableObjectCoordinates.Remove(coordinates.PositionOnGrid);
            }

            //...start moving.
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
                
                //Add me to the list of pushable objects
                if(!groundTilemap.PushableObjectCoordinates.Contains(coordinates.PositionOnGrid))
                {
                    groundTilemap.PushableObjectCoordinates.Add(coordinates.PositionOnGrid);
                }
                
                //When the object reaches the exact position of the target...
                if(transform.position == targetPosition)
                {
                    moving = false; //...stop moving.
                    if(isPlayerPushing) StartCoroutine(CallPerformAction()); //Call Perform Action event.
                }
            }
        }
    }

    private IEnumerator CallPerformAction()
    {
        yield return new WaitForSeconds(0.1f);  //Delay to make sure that the player is no longer moving the object.
        ActionsManager.onPerformAction?.Invoke();
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

    private void CantMoveStopPlayer(Vector2Int targetTile, Direction playerFacingDirection, bool isPlayer)
    {
        if(coordinates == null) return;
        //If the player or an object intends to move to my space...
        if(targetTile == coordinates.PositionOnGrid) 
        {
            //...tell other objects that I intend to move to the target coordinate.
            PlayerMovement.onIntendToMoveToTarget?.Invoke(GetCoordinateInDirection(playerFacingDirection), playerFacingDirection, false);

            isPlayerPushing = isPlayer;
            //If I can make space for the player or the object...
            if(groundTilemap.IsTargetTileFree((Vector3Int)GetCoordinateInDirection(playerFacingDirection)))
            {
                //...make sure I'm not an obstacle.
                groundTilemap.ObstacleCoordinates.Remove(coordinates.PositionOnGrid);
            }
            else
            {
                //If I'm not already an obstacle...
                if(!groundTilemap.ObstacleCoordinates.Contains(coordinates.PositionOnGrid))
                {
                    //...make me an obstacle.
                    groundTilemap.ObstacleCoordinates.Add(coordinates.PositionOnGrid);
                }
            }
        }
    }

    private void OnEnable()
    {
        PlayerMovement.onIntendToMoveToTarget += CantMoveStopPlayer;
        PlayerMovement.onAboutToMoveToTarget += BeMoved;
    }

    private void OnDisable()
    {
        PlayerMovement.onIntendToMoveToTarget -= CantMoveStopPlayer;
        PlayerMovement.onAboutToMoveToTarget -= BeMoved;
    }
}
