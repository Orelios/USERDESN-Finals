using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    private GroundTilemap groundTilemap;
    [SerializeField] private float movementSpeed;
    public float MovementSpeed { get => movementSpeed; }
    private Vector3 direction;
    private List<Vector3> targets = new List<Vector3>();
    private PlayerControls controls;
    private bool isMoving;
    private PlayerCoordinates coordinates;
    public PlayerCoordinates Coordinates { get => coordinates; }

    //Event for when the player intends to move to a space
    public static event Action<Vector2Int, Direction> onIntendToMoveToTarget;   //Things that happen before the player is about to move to a square should be subscribed to this event. (i.e. functions that would stop the player from moving to said space)

    //Event for when the player is about to move to the target
    public static event Action<Vector2Int, Direction> onAboutToMoveToTarget;   //Things that happen when the player is about to move to a square should be subscribed to this event. (i.e. object being pushed)
    //Passes a Vector2Int to all functions subscribed to it for the player's target position they are going to move to

    private bool announcedAboutToMoveToTarget = false;
    
    private void Awake()
    {
        controls = new PlayerControls();
        coordinates = GetComponent<PlayerCoordinates>();
    }

    private void Start()
    {
        groundTilemap = FindObjectOfType<GroundTilemap>();
        controls.Actions.Move.started += _ => LookDirection();
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
                onAboutToMoveToTarget?.Invoke((Vector2Int)groundTilemap.Tilemap.WorldToCell(targets[0]), coordinates.DirectionFacing);   
                announcedAboutToMoveToTarget = true;
            }

            //...move to target.
            transform.position = Vector3.MoveTowards(transform.position, targets[0], movementSpeed * Time.deltaTime);
        }
    }

    private void LookDirection()
    {
        Vector3 currentInput = (Vector3)controls.Actions.Move.ReadValue<Vector2>();
        coordinates.FaceToTarget(transform.position + currentInput);
    }

    private void StartMoving()
    {
        Vector3 currentInput = (Vector3)controls.Actions.Move.ReadValue<Vector2>();
        Vector3Int targetTile;

        onIntendToMoveToTarget?.Invoke((Vector2Int)groundTilemap.Tilemap.WorldToCell(transform.position + currentInput), coordinates.DirectionFacing);

        //If the player has not yet reached the target and input is not the same as the previous input...
        if(targets.Count != 0 && !HasReachedTarget() && direction != currentInput) 
        {
            direction = currentInput;
            //...add the next move to the buffer.
            targetTile = groundTilemap.Tilemap.WorldToCell(targets[0] + currentInput);
            if(groundTilemap.IsTargetTileFree(targetTile))
            {
                if(targets.Count > 1)
                    targets[1] = groundTilemap.Tilemap.GetCellCenterLocal(targetTile);
                else
                    targets.Add(groundTilemap.Tilemap.GetCellCenterLocal(targetTile));
            }
            
        }
        //If the player has reached the target or is still moving in the same direction...
        else 
        {
            if(isMoving) return;
            direction = currentInput;
            targetTile = groundTilemap.Tilemap.WorldToCell(transform.position + direction);

            if(groundTilemap.IsTargetTileFree(targetTile))
            {
                //...set the target point
                targets.Add(groundTilemap.Tilemap.GetCellCenterLocal(targetTile));
                isMoving = true;
            }
        }
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
