using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    private Tilemap groundTilemap;
    [SerializeField] private float movementSpeed;
    private Vector3 direction;
    private List<Vector3> targets = new List<Vector3>();
    private PlayerControls controls;
    private bool isMoving;
    private PlayerCoordinates coordinates;
    public PlayerCoordinates Coordinates { get => coordinates; }

    //Event for when the player is about to move to the target
    public static event Action<Vector2Int, Direction> onAboutToMoveToTarget;   //Things that happen when the player is about to move to a square should be subscribed to this event. (i.e. object being pushed)
    //Passes a Vector2Int to all functions subscribed to it for the player's target position they are going to move to

    private bool announcedAboutToMoveToTarget = false;

    //Obstacles
    private List<Vector2Int> obstacleCoordinates = new List<Vector2Int>();
    public List<Vector2Int> ObstacleCoordinates { get => obstacleCoordinates; }
    
    private void Awake()
    {
        controls = new PlayerControls();
        coordinates = GetComponent<PlayerCoordinates>();
    }

    private void Start()
    {
        groundTilemap = coordinates.GroundTilemap;
        controls.Actions.Move.started += _ => StartMoving();
    }

    private void Update()
    {
        //If there is a target, and the player has reached that target...
        if(targets.Count != 0 && HasReachedTarget())
        {
            //...remove that target.
            targets.RemoveAt(0);
            isMoving = false;

            announcedAboutToMoveToTarget = false;   //Reset bool value
            coordinates.SetCurrentPositionOnGrid(); //Update the player's current position value

            //If the player is still pressing down on the same input...
            if(direction == (Vector3)controls.Actions.Move.ReadValue<Vector2>() && direction != Vector3.zero)
            {
                //...keep moving.
                StartMoving();
            }
            //Otherwise...
            else
            {
                //...stop moving.
                return;
            }
        }

        //If there is (still) a target to move to...
        if(targets.Count != 0) 
        {
            //...set the player's facing direction to where the target is...
            coordinates.FaceToTarget(targets[0]);

            //...announce about to move to target...
            if(!announcedAboutToMoveToTarget)
            {
                //Pass in the specific coordinate the player will move to and the direction they are facing.
                onAboutToMoveToTarget?.Invoke((Vector2Int)groundTilemap.WorldToCell(targets[0]), coordinates.DirectionFacing);   
                announcedAboutToMoveToTarget = true;
            }

            //...move to target.
            transform.position = Vector3.MoveTowards(transform.position, targets[0], movementSpeed * Time.deltaTime);
        }
    }

    private void StartMoving()
    {
        Vector3 currentInput = (Vector3)controls.Actions.Move.ReadValue<Vector2>();
        Vector3Int targetTile;

        //If the player has not yet reached the target and input is not the same as the previous input...
        if(targets.Count != 0 && !HasReachedTarget() && direction != currentInput) 
        {
            direction = currentInput;
            //...add the next move to the buffer.
            targetTile = groundTilemap.WorldToCell(targets[0] + currentInput);
            if(IsTargetTileFree(targetTile))
            {
                if(targets.Count > 1)
                    targets[1] = groundTilemap.GetCellCenterLocal(targetTile);
                else
                    targets.Add(groundTilemap.GetCellCenterLocal(targetTile));
            }
            
        }
        //If the player has reached the target or is still moving in the same direction...
        else 
        {
            if(isMoving) return;
            direction = currentInput;
            targetTile = groundTilemap.WorldToCell(transform.position + direction);

            if(IsTargetTileFree(targetTile))
            {
                //...set the target point
                targets.Add(groundTilemap.GetCellCenterLocal(targetTile));
                isMoving = true;
            }
        }
    }

    private bool IsTargetTileFree(Vector3Int tilePosition)
    {
        //Check if tilePosition has an obstacle
        foreach(Vector2Int obstacleCoordinate in obstacleCoordinates)
        {
            //If there is an obstacle in the way...
            if(obstacleCoordinate == (Vector2Int)tilePosition)
            {
                //...don't let the player move to that space.
                return false;   
            }
        }
        return (groundTilemap.HasTile(tilePosition));
    }

    private bool HasReachedTarget()
    {
        return Vector3.Distance(transform.position, targets[0]) == 0;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
